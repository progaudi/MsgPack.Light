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

        public override MsgPackToken Write(TArray value)
        {
            if (value == null)
            {
                return null;
            }

            var arrayElements = new MsgPackToken[value.Count];

            for (var index = 0; index < value.Count; index++)
            {
                arrayElements[index] = ElementConverter.Write(value[index]);
            }

            return (MsgPackToken) arrayElements;
        }

        public override TArray Read(MsgPackToken token)
        {
            var elements = (MsgPackToken[])token;

            if (elements == null)
            {
                return default(TArray);
            }

            var length = elements.Length;

            if (!IsSingleDimensionArray)
            {
                var array = (TArray)Context.GetObjectActivator(typeof(TArray))();

                for (var i = 0u; i < length; i++)
                {
                    array.Add(ElementConverter.Read(elements[i]));
                }

                return array;
            }

            // ReSharper disable once RedundantCast
            var result = (TArray)(object)new TElement[length];

            for (var i = 0; i < length; i++)
            {
                result[i] = ElementConverter.Read(elements[i]);
            }

            return result;
        }
    }
}