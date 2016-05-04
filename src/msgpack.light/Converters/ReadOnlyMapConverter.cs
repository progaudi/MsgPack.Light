using System.Collections.Generic;

namespace MsgPack.Light.Converters
{
    internal class ReadOnlyMapConverter<TMap, TKey, TValue> : MapConverterBase<TMap, TKey, TValue>
        where TMap : IReadOnlyDictionary<TKey, TValue>
    {
        public override void Write(TMap value, IMsgPackWriter writer)
        {
            if (value == null)
            {
                Context.NullConverter.Write(value, writer);
                return;
            }

            writer.WriteMapHeader((uint) value.Count);

            foreach (var element in value)
            {
                KeyConverter.Write(element.Key, writer);
                ValueConverter.Write(element.Value, writer);
            }
        }

        public override TMap Read(IMsgPackReader reader)
        {
            throw ExceptionUtils.CantReadReadOnlyCollection(typeof(TMap));
        }
    }
}