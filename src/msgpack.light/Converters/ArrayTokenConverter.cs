using System.Collections.Generic;

namespace ProGaudi.MsgPack.Light.Converters
{
    internal class ArrayTokenConverter<TArray, TElement> : ArrayTokenConverterBase<TArray, TElement>
        where TArray : IList<TElement>
    {
        private static readonly bool IsSingleDimensionArray;

        static ArrayTokenConverter()
        {
            var type = typeof(TArray);
            IsSingleDimensionArray = type.IsArray && type.GetArrayRank() == 1 && type.GetElementType() == typeof(TElement);
        }

        public override MsgPackToken ConvertFrom(TArray value)
        {
            if (value == null)
            {
                return null;
            }

            var arrayElements = new MsgPackToken[value.Count];

            for (var index = 0; index < value.Count; index++)
            {
                arrayElements[index] = ElementTokenConverter.ConvertFrom(value[index]);
            }

            return (MsgPackToken) arrayElements;
        }

        public override TArray ConvertTo(MsgPackToken token)
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
                    array.Add(ElementTokenConverter.ConvertTo(elements[i]));
                }

                return array;
            }

            // ReSharper disable once RedundantCast
            var result = (TArray)(object)new TElement[length];

            for (var i = 0; i < length; i++)
            {
                result[i] = ElementTokenConverter.ConvertTo(elements[i]);
            }

            return result;
        }
    }
}