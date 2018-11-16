using System;
using System.Collections.Generic;

namespace ProGaudi.MsgPack.Converters.Map
{
    public sealed class Parser<TMap, TKey, TValue> : IMsgPackParser<TMap>
        where TMap : IDictionary<TKey, TValue>, new()
    {
        private readonly IMsgPackParser<TKey> _keyParser;
        private readonly IMsgPackParser<TValue> _valueParser;

        public Parser(MsgPackContext context)
        {
            _keyParser = default;
            _valueParser = default;
        }

        public TMap Parse(ReadOnlySpan<byte> source, out int readSize)
        {
            if (MsgPackSpec.TryReadNil(source, out readSize)) return default;

            var count = MsgPackSpec.ReadMapHeader(source, out readSize);
            var result = new TMap();

            for (var i = 0; i < count; i++)
            {
                var key = _keyParser.Parse(source.Slice(readSize), out var read);
                readSize += read;
                var value = _valueParser.Parse(source.Slice(readSize), out read);
                readSize += read;
                result[key] = value;
            }

            return result;
        }
    }
}