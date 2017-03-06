using System.Collections.Generic;

namespace ProGaudi.MsgPack.Light.Converters
{
    public class ReadOnlyMapConverter<TMap, TKey, TValue> : MapConverterBase<TMap, TKey, TValue>
        where TMap : IReadOnlyDictionary<TKey, TValue>
    {
        public override MsgPackToken Write(TMap map)
        {
            if (map == null)
            {
                return null;
            }

            var result = new KeyValuePair<MsgPackToken, MsgPackToken>[map.Count];
            var index = 0;
            foreach (var element in map)
            {
                var key = KeyConverter.Write(element.Key);
                var value = ValueConverter.Write(element.Value);

                result[index++] = new KeyValuePair<MsgPackToken, MsgPackToken>(key, value);
            }

            return (MsgPackToken)result;
        }

        public override TMap Read(MsgPackToken token)
        {
            throw ExceptionUtils.CantReadReadOnlyCollection(typeof(TMap));
        }
    }
}