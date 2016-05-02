using System;
using System.Collections.Generic;

namespace MsgPack.Light.Converters
{
    internal class MapConverter<TMap, TKey, TValue> : MapConverterBase<TMap, TKey, TValue>
        where TMap : IDictionary<TKey, TValue>
    {
        public override void Write(TMap value, IMsgPackWriter writer)
        {
            if (value == null)
            {
                Context.NullConverter.Write(value, writer);
                return;
            }

            writer.WriteMapHeaderAndLength((uint) value.Count);

            foreach (var element in value)
            {
                KeyConverter.Write(element.Key, writer);
                ValueConverter.Write(element.Value, writer);
            }
        }

        public override TMap Read(IMsgPackReader reader, Func<TMap> creator)
        {
            var length = reader.ReadMapLength();
            return length.HasValue ? ReadMap(reader, creator, length.Value) : default(TMap);
        }

        private TMap ReadMap(IMsgPackReader reader, Func<TMap> creator, uint length)
        {
            var map = creator == null ? (TMap) Context.GetObjectActivator(typeof(TMap))() : creator();

            for (var i = 0u; i < length; i++)
            {
                var key = KeyConverter.Read(reader, null);
                var value = ValueConverter.Read(reader, null);

                map[key] = value;
            }

            return map;
        }
    }
}