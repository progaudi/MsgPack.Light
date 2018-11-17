using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ProGaudi.MsgPack
{
    public class MsgPackContext
    {
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
                TryGenerateInterfaceMapper(type, typeof(IEnumerable<>), typeof(Converters.Enumerable.UsualFormatter<,>))
            );
        }

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
                TryGenerateInterfaceMapper(type, typeof(ICollection<>), typeof(Converters.Collection.Parser<,>))
            );
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

        private static class Cache<TFormatter>
        {
            public static TFormatter Instance;
        }
    }
}
