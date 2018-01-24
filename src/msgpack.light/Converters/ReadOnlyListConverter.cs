using System;
using System.Collections.Generic;

namespace ProGaudi.MsgPack.Light.Converters
{
    public class ReadOnlyListConverter<TArray, TElement> : ArrayConverterBase<TArray, TElement>
        where TArray : IReadOnlyList<TElement>
    {
        public override void Write(TArray value, IMsgPackWriter writer)
        {
            if (value == null)
            {
                Context.NullConverter.Write(value, writer);
                return;
            }

            writer.WriteArrayHeader((uint) value.Count);

            foreach (var element in value)
            {
                ElementConverter.Write(element, writer);
            }
        }

        public override TArray Read(IMsgPackReader reader)
        {
            throw ExceptionUtils.CantReadReadOnlyCollection(typeof(TArray));
        }

        public override int GuessByteArrayLength(TArray value)
        {
            if (value == null)
            {
                return 1;
            }

            if (value.Count <= 15) return GetResult(1);
            if (Context.PreciseMapLength) return GetResult(GetHeaderLength(value.Count));

            // since we're guessing, let's add 5 bytes as length
            return 5 + value.Count * Math.Max(
                ElementConverter.GuessByteArrayLength(value[0]),
                ElementConverter.GuessByteArrayLength(value[value.Count - 1]));


            int GetResult(int headerLength)
            {
                for (var i = 0; i < value.Count; i++)
                    headerLength += ElementConverter.GuessByteArrayLength(value[i]);

                return headerLength;
            }
        }
    }
}
