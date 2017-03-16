using System.Collections.Generic;

namespace ProGaudi.MsgPack.Light.Converters
{
    public class ReadOnlyListTokenConverter<TArray, TElement> : ArrayTokenConverterBase<TArray, TElement>
        where TArray : IReadOnlyList<TElement>
    {
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

        public override TArray ConvertTo(MsgPackToken reader)
        {
            throw ExceptionUtils.CantReadReadOnlyCollection(typeof(TArray));
        }
    }
}