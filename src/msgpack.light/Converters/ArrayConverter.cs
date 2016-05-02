using System;
using System.Collections.Generic;

namespace MsgPack.Light.Converters
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

        public override TArray Read(IMsgPackReader reader, Func<TArray> creator)
        {
            var length = reader.ReadArrayLength();
            return length.HasValue ? ReadArray(reader, creator, length.Value) : default(TArray);
        }

        private TArray ReadArray(IMsgPackReader reader, Func<TArray> creator, uint length)
        {
            if (IsSingleDimensionArray && creator == null)
                return ReadArray(reader, length);

            return ReadList(reader, creator, length);
        }

        private TArray ReadArray(IMsgPackReader reader, uint length)
        {
            // ReSharper disable once RedundantCast
            var result = (TArray)(object)new TElement[length];

            for (var i = 0; i < length; i++)
            {
                result[i] = ElementConverter.Read(reader, null);
            }

            return result;
        }

        private TArray ReadList(IMsgPackReader reader, Func<TArray> creator, uint length)
        {
            var array = creator == null ? (TArray)Context.GetObjectActivator(typeof (TArray))() : creator();

            for (var i = 0u; i < length; i++)
            {
                array.Add(ElementConverter.Read(reader, null));
            }

            return array;
        }
    }
}