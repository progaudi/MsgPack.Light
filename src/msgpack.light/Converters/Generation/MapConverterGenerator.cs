using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using ProGaudi.MsgPack.Light.Converters.Generation.Exceptions;

namespace ProGaudi.MsgPack.Light.Converters.Generation
{
    public class MapConverterGenerator
    {
        private readonly ModuleBuilder _moduleBuilder;
        private readonly string _namespaceToPlace;

        public MapConverterGenerator(ModuleBuilder moduleBuilder, string namespaceToPlace)
        {
            _moduleBuilder = moduleBuilder;
            _namespaceToPlace = namespaceToPlace;
        }

        public Type Generate(Type typeToWrap, Type typeToInstantinate)
        {
            var propsToWrap = GetDistinctProperties(typeToWrap).ToImmutableArray();

            var interfaces = typeToInstantinate == typeToWrap
                ? new[] {typeof(IMsgPackConverter<>).MakeGenericType(typeToWrap)}
                : new[]
                {
                    typeof(IMsgPackConverter<>).MakeGenericType(typeToWrap),
                    typeof(IMsgPackConverter<>).MakeGenericType(typeToInstantinate)
                };

            var typeBuilder = _moduleBuilder.DefineType(
                $"{_namespaceToPlace}.{typeToWrap.GetNormalizedName()}_{typeToInstantinate.GetNormalizedName()}_Converter",
                TypeAttributes.Public,
                typeof(object),
                interfaces);

            var converters = propsToWrap
                .Select(x => x.PropertyType)
                .Distinct()
                .Union(new[] {typeof(string)})
                .Select(
                    x => (x, typeBuilder.DefineField(
                        $"_{x.Name}_Converter",
                        typeof(Lazy<>).MakeGenericType(typeof(IMsgPackConverter<>).MakeGenericType(x)),
                        FieldAttributes.Private)))
                .ToImmutableDictionary(
                    x => x.Item1,
                    x => x.Item2
                );

            EmitInitializeMethod(typeBuilder, converters);
            var writeImpl = EmitWriteImplMethod(typeBuilder, typeToWrap, propsToWrap, converters);
            var readImpl = EmitReadImplMethod(typeBuilder, typeToInstantinate, propsToWrap, converters);

            foreach (var @interface in interfaces)
            {
                EmitWriteMethod(typeBuilder, @interface, writeImpl);
                EmitReadMethod(typeBuilder, @interface, readImpl);
            }

            return typeBuilder.ToType();
        }

        private static IEnumerable<PropertyInfo> GetDistinctProperties(Type typeToWrap)
        {
            var allProperties = typeToWrap.GetTypeInfo().IsInterface
                ? typeToWrap.GetMembersFromInterface(x => x.GetTypeInfo().DeclaredProperties)
                : typeToWrap.GetMembersFromClass(x => x.GetTypeInfo().DeclaredProperties);

            var properties = allProperties
                .Where(x => x.GetCustomAttribute<MsgPackMapElementAttribute>() != null)
                .GroupBy(x => x.GetMapElementName())
                .ToDictionary(x => x.Key, x => x.ToArray());

            foreach (var pair in properties)
            {
                if (pair.Value.Length > 1)
                {
                    throw ExceptionUtils.DuplicateMapElement(typeToWrap, pair);
                }

                yield return pair.Value[0];
            }
        }

        private static void EmitReadMethod(TypeBuilder typeBuilder, Type @interface, MethodBuilder readImpl)
        {
            var mb = typeBuilder.DefineMethod(
                $"{nameof(IMsgPackConverter<object>.Read)}",
                MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig,
                CallingConventions.HasThis);
            mb.SetReturnType(@interface.GenericTypeArguments[0]);
            mb.SetParameters(typeof(IMsgPackReader));

            var generator = mb.GetILGenerator();

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Call, readImpl);
            generator.Emit(OpCodes.Ret);
        }

        private static void EmitWriteMethod(TypeBuilder typeBuilder, Type @interface, MethodBuilder writeImpl)
        {
            var mb = typeBuilder.DefineMethod(
                $"{nameof(IMsgPackConverter<object>.Write)}",
                MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig,
                CallingConventions.HasThis);
            mb.SetReturnType(typeof(void));
            mb.SetParameters(@interface.GenericTypeArguments[0], typeof(IMsgPackWriter));

            var generator = mb.GetILGenerator();

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Ldarg_2);
            generator.Emit(OpCodes.Call, writeImpl);
            generator.Emit(OpCodes.Ret);
        }

        private static MethodBuilder EmitReadImplMethod(
            TypeBuilder typeBuilder,
            Type typeToInstantinate,
            ImmutableArray<PropertyInfo> propsToWrap,
            ImmutableDictionary<Type, FieldBuilder> converters)
        {
            var mb = typeBuilder.DefineMethod(
                $"{nameof(IMsgPackConverter<object>.Read)}Impl",
                MethodAttributes.Family | MethodAttributes.Virtual | MethodAttributes.HideBySig,
                CallingConventions.HasThis);
            mb.SetReturnType(typeToInstantinate);
            mb.SetParameters(typeof(IMsgPackReader));

            var generator = mb.GetILGenerator();

            var instance = generator.DeclareLocal(typeToInstantinate);
            var length = generator.DeclareLocal(typeof(uint?));
            var propertyName = generator.DeclareLocal(typeof(string));
            var index = generator.DeclareLocal(typeof(int));

            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Callvirt, typeof(IMsgPackReader).GetTypeInfo().GetMethod(nameof(IMsgPackReader.ReadMapLength)));
            generator.Emit(OpCodes.Stloc, length);

            var nonNullLabel = generator.DefineLabel();
            generator.Emit(OpCodes.Ldloca, length);
            generator.Emit(OpCodes.Call, typeof(uint?).GetTypeInfo().GetProperty(nameof(Nullable<uint>.HasValue)).GetMethod);
            generator.Emit(OpCodes.Brtrue, nonNullLabel);
            generator.Emit(OpCodes.Ldnull);
            generator.Emit(OpCodes.Ret);

            generator.MarkLabel(nonNullLabel);
            generator.Emit(OpCodes.Newobj, typeToInstantinate.GetTypeInfo().GetDefaultConstructor());
            generator.Emit(OpCodes.Stloc, instance);

            var beginOfIteration = generator.DefineLabel();
            var conditionLabel = generator.DefineLabel();
            var incrementLabel = generator.DefineLabel();
            generator.Emit(OpCodes.Ldc_I4_0);
            generator.Emit(OpCodes.Stloc, index);
            generator.Emit(OpCodes.Br, conditionLabel);

            void EmitRead(Type type)
            {
                var converter = converters[type];
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldfld, converter);
                generator.Emit(OpCodes.Callvirt, converter.FieldType.GetTypeInfo().GetProperty(nameof(Lazy<object>.Value)).GetMethod);
                generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(
                    OpCodes.Callvirt,
                    converter.FieldType.GenericTypeArguments[0].GetTypeInfo().GetMethod(nameof(IMsgPackConverter<object>.Read), new[] {typeof(IMsgPackReader)}));
            }

            generator.MarkLabel(beginOfIteration);
            EmitRead(typeof(string));
            generator.Emit(OpCodes.Stloc, propertyName);

            var next = default(Label?);
            foreach (var property in propsToWrap)
            {
                if (next.HasValue)
                {
                    generator.MarkLabel(next.Value);
                }

                next = generator.DefineLabel();
                generator.Emit(OpCodes.Ldloc, propertyName);
                generator.Emit(OpCodes.Ldstr, property.GetMapElementName());
                generator.Emit(OpCodes.Ldc_I4, (int) StringComparison.OrdinalIgnoreCase);
                generator.Emit(
                    OpCodes.Call,
                    typeof(string).GetTypeInfo().GetMethod(
                        nameof(string.Equals),
                        new[] {typeof(string), typeof(string), typeof(StringComparison)}));
                generator.Emit(OpCodes.Brfalse, next.Value);

                generator.Emit(OpCodes.Ldloc, instance);
                EmitRead(property.PropertyType);
                // if property has setter, we will call it. Otherwise we'll try to find it on implementation.
                generator.Emit(OpCodes.Callvirt, property.SetMethod ?? typeToInstantinate.GetTypeInfo().GetProperty(property.Name).SetMethod);

                generator.Emit(OpCodes.Br, incrementLabel);
            }

            if (next.HasValue)
            {
                generator.MarkLabel(next.Value);
            }

            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Callvirt, typeof(IMsgPackReader).GetTypeInfo().GetMethod(nameof(IMsgPackReader.SkipToken)));

            generator.MarkLabel(incrementLabel);
            generator.Emit(OpCodes.Ldloc, index);
            generator.Emit(OpCodes.Ldc_I4_1);
            generator.Emit(OpCodes.Add);
            generator.Emit(OpCodes.Stloc, index);

            generator.MarkLabel(conditionLabel);
            generator.Emit(OpCodes.Ldloc, index);
            generator.Emit(OpCodes.Conv_I8);
            generator.Emit(OpCodes.Ldloca, length);
            generator.Emit(OpCodes.Call, length.LocalType.GetTypeInfo().GetProperty(nameof(Nullable<int>.Value)).GetMethod);
            generator.Emit(OpCodes.Conv_U8);
            generator.Emit(OpCodes.Blt, beginOfIteration);

            generator.Emit(OpCodes.Ldloc, instance);
            generator.Emit(OpCodes.Ret);

            return mb;
        }

        private static MethodBuilder EmitWriteImplMethod(
            TypeBuilder typeBuilder,
            Type typeToWrap,
            ImmutableArray<PropertyInfo> propsToWrap,
            ImmutableDictionary<Type, FieldBuilder> converters)
        {
            var mb = typeBuilder.DefineMethod(
                $"{nameof(IMsgPackConverter<object>.Write)}Impl",
                MethodAttributes.Family | MethodAttributes.Virtual | MethodAttributes.HideBySig,
                CallingConventions.HasThis);
            mb.SetReturnType(typeof(void));
            mb.SetParameters(typeToWrap, typeof(IMsgPackWriter));

            var generator = mb.GetILGenerator();

            var notNullCode = generator.DefineLabel();
            var value = OpCodes.Ldarg_1;
            generator.Emit(value);
            generator.Emit(OpCodes.Brtrue, notNullCode);

            // emit null version
            var writer = OpCodes.Ldarg_2;
            generator.Emit(writer);
            generator.Emit(OpCodes.Ldc_I4, (int) DataTypes.Null);
            generator.Emit(
                OpCodes.Callvirt,
                typeof(IMsgPackWriter).GetTypeInfo().GetMethod(nameof(IMsgPackWriter.Write), new[] {typeof(DataTypes)}));
            generator.Emit(OpCodes.Ret);

            // emit not-null version
            generator.MarkLabel(notNullCode);
            generator.Emit(writer);
            generator.Emit(OpCodes.Ldc_I4, propsToWrap.Length);
            generator.Emit(OpCodes.Callvirt, typeof(IMsgPackWriter).GetTypeInfo().GetMethod(nameof(IMsgPackWriter.WriteMapHeader)));

            void EmitWrite(Type type, Action<ILGenerator> valueLoader)
            {
                var converter = converters[type];
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldfld, converter);
                generator.Emit(OpCodes.Callvirt, converter.FieldType.GetTypeInfo().GetProperty(nameof(Lazy<object>.Value)).GetMethod);
                valueLoader(generator);
                generator.Emit(writer);
                generator.Emit(
                    OpCodes.Callvirt,
                    converter.FieldType.GenericTypeArguments[0].GetTypeInfo().GetMethod(nameof(IMsgPackConverter<object>.Write), new[] {type, typeof(IMsgPackWriter)}));
            }

            foreach (var property in propsToWrap)
            {
                EmitWrite(
                    typeof(string),
                    x => x.Emit(OpCodes.Ldstr, property.GetMapElementName()));
                EmitWrite(
                    property.PropertyType,
                    x =>
                    {
                        x.Emit(OpCodes.Ldarg_1);
                        x.Emit(OpCodes.Callvirt, property.GetMethod);
                    });
            }

            generator.Emit(OpCodes.Ret);

            return mb;
        }

        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        private static void EmitInitializeMethod(
            TypeBuilder typeBuilder,
            ImmutableDictionary<Type, FieldBuilder> converters)
        {
            var mb = typeBuilder.DefineMethod(
                nameof(IMsgPackConverter.Initialize),
                MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig,
                CallingConventions.HasThis);
            mb.SetReturnType(typeof(void));
            mb.SetParameters(typeof(MsgPackContext));

            var generator = mb.GetILGenerator();

            foreach (var converter in converters)
            {
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(
                    OpCodes.Ldftn,
                    typeof(MsgPackContext).GetTypeInfo().GetMethod(nameof(MsgPackContext.GetConverter))
                        .MakeGenericMethod(converter.Key));

                var converterType = converter.Value.FieldType.GenericTypeArguments[0];
                generator.Emit(
                    OpCodes.Newobj,
                    typeof(Func<>)
                        .MakeGenericType(converterType)
                        .GetTypeInfo()
                        .GetConstructor(new[] {typeof(object), typeof(IntPtr)}));
                generator.Emit(
                    OpCodes.Newobj,
                    typeof(Lazy<>)
                        .MakeGenericType(converterType)
                        .GetTypeInfo()
                        .GetConstructor(
                            new[]
                            {
                                typeof(Func<>).MakeGenericType(converterType)
                            }));
                generator.Emit(OpCodes.Stfld, converter.Value);
            }

            generator.Emit(OpCodes.Ret);
        }
    }
}
