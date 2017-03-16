using System.Collections.Generic;

namespace ProGaudi.MsgPack.Light.Converters
{
    public class ReadOnlyMapTokenConverter<TMap, TKey, TValue> : MapTokenConverterBase<TMap, TKey, TValue>
        where TMap : IReadOnlyDictionary<TKey, TValue>
    {
        public override MsgPackToken ConvertFrom(TMap map)
        {
            if (map == null)
            {
                return null;
            }

            var result = new KeyValuePair<MsgPackToken, MsgPackToken>[map.Count];
            var index = 0;
            foreach (var element in map)
            {
                var key = KeyTokenConverter.ConvertFrom(element.Key);
                var value = ValueTokenConverter.ConvertFrom(element.Value);

                result[index++] = new KeyValuePair<MsgPackToken, MsgPackToken>(key, value);
            }

            return (MsgPackToken)result;
        }

        public override TMap ConvertTo(MsgPackToken token)
        {
            throw ExceptionUtils.CantReadReadOnlyCollection(typeof(TMap));
        }
    }
}