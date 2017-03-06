using System.Collections.Generic;

namespace ProGaudi.MsgPack.Light.Converters
{
    internal class MapConverter<TMap, TKey, TValue> : MapConverterBase<TMap, TKey, TValue>
        where TMap : IDictionary<TKey, TValue>
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

            return (MsgPackToken) result;
        }

        public override TMap Read(MsgPackToken token)
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
                var key = KeyConverter.Read(mapElements[i].Key);
                var value = ValueConverter.Read(mapElements[i].Value);

                result[key] = value;
            }

            return result;
        }
    }
}