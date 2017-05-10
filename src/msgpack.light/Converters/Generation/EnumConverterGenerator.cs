// <copyright file="EnumConverterGenerator.cs" company="eVote">
//   Copyright © eVote
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Emit;

namespace ProGaudi.MsgPack.Light.Converters.Generation
{
    internal class EnumConverterGenerator
    {
        private readonly ModuleBuilder _moduleBuilder;
        private readonly string _namespaceToPlace;

        public EnumConverterGenerator(ModuleBuilder moduleBuilder, string namespaceToPlace)
        {
            _moduleBuilder = moduleBuilder;
            _namespaceToPlace = namespaceToPlace;
        }

        public Type Generate(Type typeToWrap, bool convertEnumsAsStrings)
        {
            var interfaceToImplement = typeof(IMsgPackConverter<>).MakeGenericType(typeToWrap);

            var typeBuilder = _moduleBuilder.DefineType(
                $"{_namespaceToPlace}.{typeToWrap.GetNormalizedName()}_EnumConverter",
                TypeAttributes.Public,
                typeof(object),
                new[] { interfaceToImplement });

            var underlyingType = convertEnumsAsStrings ? typeof(string) : Enum.GetUnderlyingType(typeToWrap);

            var underlyingTypeConverter = typeBuilder.DefineField(
                $"_{underlyingType.Name}_Converter",
                typeof(Lazy<>).MakeGenericType(typeof(IMsgPackConverter<>).MakeGenericType(underlyingType)),
                FieldAttributes.Private);

            EmitInitializeMethod(typeBuilder, underlyingType, underlyingTypeConverter);
            EmitWriteMethod(typeBuilder, underlyingType, underlyingTypeConverter);
            EmitReadMethod(typeBuilder, interfaceToImplement, underlyingType, underlyingTypeConverter);

            return typeBuilder.ToType();
        }

        private static void EmitReadMethod(
            TypeBuilder typeBuilder,
            Type @interface,
            Type underlyingType,
            FieldBuilder underlyingTypeConverter)
        {
            var mb = typeBuilder.DefineMethod(
                $"{nameof(IMsgPackConverter<object>.Read)}",
                MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig,
                CallingConventions.HasThis);
            mb.SetReturnType(@interface.GenericTypeArguments[0]);
            mb.SetParameters(typeof(IMsgPackReader));

            mb.SetReturnType(underlyingType);
            mb.SetParameters(typeof(IMsgPackReader));

            var generator = mb.GetILGenerator();

            var instance = generator.DeclareLocal(underlyingType);
            
            generator.Emit(OpCodes.Ldloc, instance);
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldfld, underlyingTypeConverter);
            generator.Emit(OpCodes.Callvirt, underlyingTypeConverter.FieldType.GetTypeInfo().GetProperty(nameof(Lazy<object>.Value)).GetMethod);
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(
                OpCodes.Callvirt,
                underlyingTypeConverter.FieldType.GenericTypeArguments[0].GetTypeInfo().GetMethod(nameof(IMsgPackConverter<object>.Read), new[] { typeof(IMsgPackReader) }));

            generator.Emit(OpCodes.Ret);
        }

        private static void EmitWriteMethod(
            TypeBuilder typeBuilder,
            Type @interface,
            FieldBuilder underlyingTypeConverter)
        {
            var mb = typeBuilder.DefineMethod(
                $"{nameof(IMsgPackConverter<object>.Write)}",
                MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig,
                CallingConventions.HasThis);
            mb.SetReturnType(typeof(void));
            mb.SetParameters(@interface.GenericTypeArguments[0], typeof(IMsgPackWriter));

            var generator = mb.GetILGenerator();

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldfld, underlyingTypeConverter);
            generator.Emit(OpCodes.Callvirt, underlyingTypeConverter.FieldType.GetTypeInfo().GetProperty(nameof(Lazy<object>.Value)).GetMethod);
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Ldarg_2);
            generator.Emit(
                OpCodes.Callvirt,
                underlyingTypeConverter.FieldType.GenericTypeArguments[0].GetTypeInfo().GetMethod(nameof(IMsgPackConverter<object>.Write), new[] { @interface, typeof(IMsgPackWriter) }));

            generator.Emit(OpCodes.Ret);
        }

        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        private static void EmitInitializeMethod(
            TypeBuilder typeBuilder,
            Type underlyingType,
            FieldBuilder underlyingTypeConverter)
        {
            var mb = typeBuilder.DefineMethod(
                nameof(IMsgPackConverter.Initialize),
                MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig,
                CallingConventions.HasThis);
            mb.SetReturnType(typeof(void));
            mb.SetParameters(typeof(MsgPackContext));

            var generator = mb.GetILGenerator();

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(
                OpCodes.Ldftn,
                typeof(MsgPackContext).GetTypeInfo().GetMethod(nameof(MsgPackContext.GetConverter))
                    .MakeGenericMethod(underlyingType));

            var converterType = underlyingTypeConverter.FieldType.GenericTypeArguments[0];
            generator.Emit(
                OpCodes.Newobj,
                typeof(Func<>)
                    .MakeGenericType(converterType)
                    .GetTypeInfo()
                    .GetConstructor(new[] { typeof(object), typeof(IntPtr) }));
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
            generator.Emit(OpCodes.Stfld, underlyingTypeConverter);

            generator.Emit(OpCodes.Ret);
        }
    }
}