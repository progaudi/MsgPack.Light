using System.Collections.Generic;

namespace ProGaudi.MsgPack.Light.Converters
{
    internal class MapTokenConverter<TMap, TKey, TValue> : MapTokenConverterBase<TMap, TKey, TValue>
        where TMap : IDictionary<TKey, TValue>
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

            return (MsgPackToken) result;
        }

        public override TMap ConvertTo(MsgPackToken token)
        {
            var mapElements = (KeyValuePair<MsgPackToken, MsgPackToken>[]) token;
            if (mapElements == null)
            {
                return default(TMap);
            }

            var length = mapElements.Length;
            var result = (TMap)Context.GetObjectActivator(typeof(TMap))();

            for (var i = 0u; i < length; i++)
            {
                var key = KeyTokenConverter.ConvertTo(mapElements[i].Key);
                var value = ValueTokenConverter.ConvertTo(mapElements[i].Value);

                result[key] = value;
            }

            return result;
        }
    }
}