using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ProGaudi.MsgPack.Converters.Enum
{
    internal class String<T> : IMsgPackFormatter<T>, IMsgPackParser<T>, IMsgPackSequenceParser<T>
        where T : struct
    {
        private static readonly Dictionary<T, (int length, byte[] blob)> ValueToLabel;

        private static readonly Dictionary<string, T> LabelToValue;

        private static readonly Dictionary<T, int> ValueToLength;

        static String()
        {
            if (!typeof(T).GetTypeInfo().IsEnum)
            {
                throw new ArgumentException($"{typeof(T)} is not an enum.");
            }

            var values = System.Enum.GetValues(typeof(T)).Cast<T>().Distinct().ToArray();
            ValueToLabel = values.ToDictionary(x => x, x =>
            {
                var name = System.Enum.GetName(typeof(T), x);
                return (name.Length, MsgPackSpec.DefaultEncoding.GetBytes(name));
            });
            LabelToValue = values.ToDictionary(x => System.Enum.GetName(typeof(T), x));
            ValueToLength = LabelToValue.ToDictionary(
                x => x.Value,
                x =>
                {
                    var length = MsgPackSpec.DefaultEncoding.GetByteCount(x.Key);
                    return length + DataLengths.GetStringHeaderLengthByBytesCount(length);
                });
        }

        public int GetBufferSize(T value) => ValueToLength.TryGetValue(value, out var x ) ? x : MsgPackSpec.DefaultEncoding.GetByteCount(value.ToString());

        public bool HasConstantSize => false;

        public int Format(Span<byte> destination, T value)
        {
            if (ValueToLabel.TryGetValue(value, out var x))
            {
                var result = MsgPackSpec.WriteStringHeader(destination, x.length);
                x.blob.CopyTo(destination.Slice(result));
                return result + x.blob.Length;
            }

            return MsgPackSpec.WriteString(destination, value.ToString().AsSpan());
        }

        public T Parse(ReadOnlySpan<byte> source, out int readSize)
        {
            var value = MsgPackSpec.ReadString(source, out readSize);
            return LabelToValue.TryGetValue(value, out var x)
                ? x
                : (T)System.Enum.Parse(
                    typeof(T),
                    value,
                    true);
        }

        public T Parse(ReadOnlySequence<byte> source, out int readSize)
        {
            var value = MsgPackSpec.ReadString(source, out readSize);
            return LabelToValue.TryGetValue(value, out var x)
                ? x
                : (T)System.Enum.Parse(
                    typeof(T),
                    value,
                    true);
        }
    }
}
