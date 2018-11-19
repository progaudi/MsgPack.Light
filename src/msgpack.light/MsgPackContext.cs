using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ProGaudi.MsgPack
{
    public sealed class MsgPackContext
    {
        private readonly Dictionary<Type, Type> _genericFormatters = new Dictionary<Type, Type>();

        private readonly Dictionary<Type, Type> _genericParsers = new Dictionary<Type, Type>();

        static MsgPackContext()
        {
            Cache<IMsgPackFormatter<byte[]>>.Instance = Converters.Binary.Converter.Compatibility;
            Cache<IMsgPackFormatter<ReadOnlyMemory<byte>>>.Instance = Converters.Binary.Converter.Compatibility;
            Cache<IMsgPackParser<byte[]>>.Instance = Converters.Binary.Converter.Compatibility;
            Cache<IMsgPackParser<IMemoryOwner<byte>>>.Instance = Converters.Binary.Converter.Compatibility;

            Cache<IMsgPackFormatter<DateTime>>.Instance = Converters.Date.Ticks.Instance;
            Cache<IMsgPackFormatter<DateTimeOffset>>.Instance = Converters.Date.Ticks.Instance;
            Cache<IMsgPackFormatter<TimeSpan>>.Instance = Converters.Date.Ticks.Instance;
            Cache<IMsgPackParser<DateTimeOffset>>.Instance = Converters.Date.Ticks.Instance;
            Cache<IMsgPackParser<DateTime>>.Instance = Converters.Date.Ticks.Instance;
            Cache<IMsgPackParser<TimeSpan>>.Instance = Converters.Date.Ticks.Instance;

            Cache<IMsgPackFormatter<byte>>.Instance = Converters.Number.UsualFormatter.Instance;
            Cache<IMsgPackFormatter<sbyte>>.Instance = Converters.Number.UsualFormatter.Instance;
            Cache<IMsgPackFormatter<short>>.Instance = Converters.Number.UsualFormatter.Instance;
            Cache<IMsgPackFormatter<ushort>>.Instance = Converters.Number.UsualFormatter.Instance;
            Cache<IMsgPackFormatter<int>>.Instance = Converters.Number.UsualFormatter.Instance;
            Cache<IMsgPackFormatter<uint>>.Instance = Converters.Number.UsualFormatter.Instance;
            Cache<IMsgPackFormatter<long>>.Instance = Converters.Number.UsualFormatter.Instance;
            Cache<IMsgPackFormatter<ulong>>.Instance = Converters.Number.UsualFormatter.Instance;
            Cache<IMsgPackFormatter<float>>.Instance = Converters.Number.UsualFormatter.Instance;
            Cache<IMsgPackFormatter<double>>.Instance = Converters.Number.UsualFormatter.Instance;

            Cache<IMsgPackParser<byte>>.Instance = Converters.Number.Parser.Instance;
            Cache<IMsgPackParser<sbyte>>.Instance = Converters.Number.Parser.Instance;
            Cache<IMsgPackParser<short>>.Instance = Converters.Number.Parser.Instance;
            Cache<IMsgPackParser<ushort>>.Instance = Converters.Number.Parser.Instance;
            Cache<IMsgPackParser<int>>.Instance = Converters.Number.Parser.Instance;
            Cache<IMsgPackParser<uint>>.Instance = Converters.Number.Parser.Instance;
            Cache<IMsgPackParser<long>>.Instance = Converters.Number.Parser.Instance;
            Cache<IMsgPackParser<ulong>>.Instance = Converters.Number.Parser.Instance;
            Cache<IMsgPackParser<float>>.Instance = Converters.Number.Parser.Instance;
            Cache<IMsgPackParser<double>>.Instance = Converters.Number.Parser.Instance;

            Cache<IMsgPackFormatter<string>>.Instance = new Converters.String.UsualFormatter();
            Cache<IMsgPackParser<string>>.Instance = new Converters.String.Parser();

            Cache<IMsgPackFormatter<bool>>.Instance = Converters.BoolConverter.Instance;
            Cache<IMsgPackParser<bool>>.Instance = Converters.BoolConverter.Instance;
        }

        public void RegisterGenericFormatter(Type type) => RegisterGenericMapper(type, typeof(IMsgPackFormatter<>), _genericFormatters);

        public void RegisterGenericParser(Type type) => RegisterGenericMapper(type, typeof(IMsgPackParser<>), _genericParsers);

        public IMsgPackFormatter<T> RegisterFormatter<T>(Func<MsgPackContext, IMsgPackFormatter<T>> func) => RegisterFormatter(func(this));

        public IMsgPackFormatter<T> RegisterFormatter<T>(IMsgPackFormatter<T> formatter) => Cache<IMsgPackFormatter<T>>.Instance = formatter;

        public IMsgPackFormatter<T> GetFormatter<T>()
        {
            var result = Cache<IMsgPackFormatter<T>>.Instance;
            if (result != null)
                return result;

            var type = typeof(T);
            return Cache<IMsgPackFormatter<T>>.Instance = (IMsgPackFormatter<T>)(
                TryGenerateEnumMapper(type) ??
                TryGenerateArrayMapper(type, typeof(Converters.Array.UsualFormatter<>)) ??
                TryGenerateStructMapper(type, typeof(ReadOnlyMemory<>), typeof(Converters.Array.UsualFormatter<>)) ??
                TryGenerateStructMapper(type, typeof(Memory<>), typeof(Converters.Array.UsualFormatter<>)) ??
                TryGenerateStructMapper(type, typeof(Nullable<>), typeof(Converters.NullableConverter<>)) ??
                TryGenerateInterfaceMapper(type, typeof(IList<>), typeof(Converters.List.UsualFormatter<,>)) ??
                TryGenerateInterfaceMapper(type, typeof(IReadOnlyList<>), typeof(Converters.ReadOnlyList.UsualFormatter<,>)) ??
                TryGenerateInterfaceMapper(type, typeof(IDictionary<,>), typeof(Converters.Map.UsualFormatter<,,>)) ??
                TryGenerateInterfaceMapper(type, typeof(IReadOnlyDictionary<,>), typeof(Converters.ReadOnlyMap.UsualFormatter<,,>)) ??
                TryGenerateInterfaceMapper(type, typeof(ICollection<>), typeof(Converters.Collection.UsualFormatter<,>)) ??
                TryGenerateInterfaceMapper(type, typeof(IReadOnlyCollection<>), typeof(Converters.ReadOnlyCollection.UsualFormatter<,>)) ??
                TryGenerateInterfaceMapper(type, typeof(IEnumerable<>), typeof(Converters.Enumerable.UsualFormatter<,>)) ??
                TryGenerateGenericMapper(type, _genericFormatters)
            );
        }

        public IMsgPackParser<T> RegisterParser<T>(Func<MsgPackContext, IMsgPackParser<T>> func) => RegisterParser(func(this));

        public IMsgPackParser<T> RegisterParser<T>(IMsgPackParser<T> parser) => Cache<IMsgPackParser<T>>.Instance = parser;

        public IMsgPackParser<T> GetParser<T>()
        {
            var result = Cache<IMsgPackParser<T>>.Instance;
            if (result != null)
                return result;

            var type = typeof(T);
            return Cache<IMsgPackParser<T>>.Instance = (IMsgPackParser<T>)(
                TryGenerateEnumMapper(type) ??
                TryGenerateArrayMapper(type, typeof(Converters.Array.Parser<>)) ??
                TryGenerateStructMapper(type, typeof(ReadOnlyMemory<>), typeof(Converters.Array.Parser<>)) ??
                TryGenerateStructMapper(type, typeof(Memory<>), typeof(Converters.Array.Parser<>)) ??
                TryGenerateStructMapper(type, typeof(Nullable<>), typeof(Converters.NullableConverter<>)) ??
                TryGenerateInterfaceMapper(type, typeof(IList<>), typeof(Converters.List.Parser<,>)) ??
                TryGenerateInterfaceMapper(type, typeof(IDictionary<,>), typeof(Converters.Map.Parser<,,>)) ??
                TryGenerateInterfaceMapper(type, typeof(ICollection<>), typeof(Converters.Collection.Parser<,>)) ??
                TryGenerateGenericMapper(type, _genericParsers)
            );
        }

        private void RegisterGenericMapper(Type mapper, Type baseInterface, Dictionary<Type, Type> cache)
        {
            var converterType = GetGenericInterface(mapper, baseInterface)
                ?? throw new ArgumentException($"Error registering generic mapper. Expected {baseInterface.Name} implementation, but got {mapper}");

            var convertedType = converterType.GenericTypeArguments.Single().GetGenericTypeDefinition();
            cache[convertedType] = mapper;
        }

        private object TryGenerateInterfaceMapper(Type type, Type generic, Type mapper)
        {
            bool Predicate(Type x) => x.IsConstructedGenericType && x.GetGenericTypeDefinition() == generic;
            var @interface = type.GetTypeInfo().IsInterface && Predicate(type)
                ? type
                : type
                    .GetInterfaces()
                    .FirstOrDefault(Predicate);

            if (@interface == null)
                return null;

            return mapper
                .MakeGenericType(new [] { type }.Concat(type.GetGenericArguments()).ToArray())
                .GetContextActivator()(this);
        }

        private object TryGenerateGenericMapper(Type type, Dictionary<Type, Type> mappers)
        {
            if (!type.GetTypeInfo().IsGenericType)
            {
                return null;
            }
            var genericType = type.GetGenericTypeDefinition();

            if (!mappers.TryGetValue(genericType, out var genericConverterType))
            {
                return null;
            }

            var converterType = genericConverterType.MakeGenericType(type.GenericTypeArguments);
            var contextActivator = converterType.GetContextActivator();
            return contextActivator != null ? contextActivator(this) : converterType.GetDefaultActivator()();
        }

        private object TryGenerateStructMapper(Type type, Type generic, Type mapper)
        {
            if (!type.GetTypeInfo().IsValueType)
                return null;

            if (!type.IsConstructedGenericType)
                return null;

            if (type.GetGenericTypeDefinition() != generic)
                return null;

            return mapper
                .MakeGenericType(type.GetGenericArguments())
                .GetContextActivator()(this);
        }

        private object TryGenerateArrayMapper(Type type, Type mapper)
        {
            if (!type.IsArray || type.GetArrayRank() != 1)
                return null;

            var element = type.GetElementType();
            return mapper
                .MakeGenericType(element)
                .GetContextActivator()(this);
        }

        private static object TryGenerateEnumMapper(Type type)
        {
            if (!type.GetTypeInfo().IsEnum) return null;
            return typeof(Converters.Enum.String<>)
                .MakeGenericType(type)
                .GetDefaultActivator()();
        }

        private static TypeInfo GetGenericInterface(Type type, Type genericInterfaceType)
        {
            var info = type.GetTypeInfo();
            if (info.IsInterface && info.IsGenericType && info.GetGenericTypeDefinition() == genericInterfaceType)
            {
                return info;
            }

            return info
                .ImplementedInterfaces
                .Select(x => x.GetTypeInfo())
                .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == genericInterfaceType);
        }

        private static class Cache<TFormatter>
        {
            public static TFormatter Instance;
        }
    }
}
