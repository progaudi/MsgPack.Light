using System;
using System.Collections.Generic;
using System.Linq;

namespace ProGaudi.MsgPack.Light.Converters
{
    public class ReadOnlyMapConverter<TMap, TKey, TValue> : MapConverterBase<TMap, TKey, TValue>
        where TMap : IReadOnlyDictionary<TKey, TValue>
    {
        public override void Write(TMap value, IMsgPackWriter writer)
        {
            if (value == null)
            {
                Context.NullConverter.Write(value, writer);
                return;
            }

            writer.WriteMapHeader((uint) value.Count);

            foreach (var element in value)
            {
                KeyConverter.Write(element.Key, writer);
                ValueConverter.Write(element.Value, writer);
            }
        }

        public override TMap Read(IMsgPackReader reader)
        {
            throw ExceptionUtils.CantReadReadOnlyCollection(typeof(TMap));
        }

        public override int GuessByteArrayLength(TMap value)
        {
            if (value == null)
            {
                return 1;
            }

            if (value.Count <= 15) return GetResult(1);
            if (Context.PreciseMapLength) return GetResult(GetHeaderLength(value.Count));

            // since we're guessing, let's add 5 bytes as length
            var first = value.First();
            var last = value.Last();
            return 5 + value.Count * (
                Math.Max(KeyConverter.GuessByteArrayLength(first.Key), KeyConverter.GuessByteArrayLength(last.Key)) +
                Math.Max(ValueConverter.GuessByteArrayLength(first.Value), ValueConverter.GuessByteArrayLength(last.Value)));

            int GetResult(int headerLength)
            {
                foreach (var x in value)
                {
                    for (var i = 0; i < value.Count; i++)
                        headerLength += KeyConverter.GuessByteArrayLength(x.Key) + ValueConverter.GuessByteArrayLength(x.Value);
                }

                return headerLength;
            }
        }
    }
}