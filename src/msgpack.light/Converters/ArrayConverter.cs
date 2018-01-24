using System;
using System.Collections.Generic;

namespace ProGaudi.MsgPack.Light.Converters
{
    internal class ArrayConverter<TArray, TElement> : ArrayConverterBase<TArray, TElement>
        where TArray : IList<TElement>
    {
        private static readonly bool IsSingleDimensionArray;

        static ArrayConverter()
        {
            var type = typeof(TArray);
            IsSingleDimensionArray = type.IsArray && type.GetArrayRank() == 1 && type.GetElementType() == typeof(TElement);
        }

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
            var length = reader.ReadArrayLength();
            return length.HasValue ? ReadArray(reader, length.Value) : default(TArray);
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

        private TArray ReadArray(IMsgPackReader reader, uint length)
        {
            if (!IsSingleDimensionArray)
                return ReadList(reader, length);

            // ReSharper disable once RedundantCast
            var result = (TArray)(object)new TElement[length];

            for (var i = 0; i < length; i++)
            {
                result[i] = ElementConverter.Read(reader);
            }

            return result;
        }

        private TArray ReadList(IMsgPackReader reader, uint length)
        {
            var array = (TArray)Context.GetObjectActivator(typeof (TArray))();

            for (var i = 0u; i < length; i++)
            {
                array.Add(ElementConverter.Read(reader));
            }

            return array;
        }
    }
}