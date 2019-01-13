using System;
using System.Buffers;
using System.Collections.Generic;

namespace ProGaudi.MsgPack.Converters.Map
{
    public sealed class SequenceParser<TMap, TKey, TValue> : IMsgPackSequenceParser<TMap>
        where TMap : IDictionary<TKey, TValue>, new()
    {
        private readonly IMsgPackSequenceParser<TKey> _keySequenceParser;
        private readonly IMsgPackSequenceParser<TValue> _valueSequenceParser;

        public SequenceParser(MsgPackContext context)
        {
            _keySequenceParser = context.GetRequiredSequenceParser<TKey>();
            _valueSequenceParser = context.GetRequiredSequenceParser<TValue>();
        }

        public TMap Parse(ReadOnlySequence<byte> source, out int readSize)
        {
            if (MsgPackSpec.TryReadNil(source, out readSize)) return default;

            var count = MsgPackSpec.ReadMapHeader(source, out readSize);
            var result = new TMap();

            for (var i = 0; i < count; i++)
            {
                var key = _keySequenceParser.Parse(source.Slice(readSize), out var read);
                readSize += read;
                var value = _valueSequenceParser.Parse(source.Slice(readSize), out read);
                readSize += read;
                result[key] = value;
            }

            return result;
        }
    }
}
