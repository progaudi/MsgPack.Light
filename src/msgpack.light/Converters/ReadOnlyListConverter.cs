using System.Collections.Generic;

namespace ProGaudi.MsgPack.Light.Converters
{
    public class ReadOnlyListConverter<TArray, TElement> : ArrayConverterBase<TArray, TElement>
        where TArray : IReadOnlyList<TElement>
    {
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

        public override TArray Read(MsgPackToken reader)
        {
            throw ExceptionUtils.CantReadReadOnlyCollection(typeof(TArray));
        }
    }
}