using System;
using System.Collections.Generic;

namespace ProGaudi.MsgPack.Converters.ReadOnlyMap
{
    public sealed class UsualFormatter<TMap, TKey, TValue> : IMsgPackFormatter<TMap>
        where TMap : IReadOnlyDictionary<TKey, TValue>
    {
        private readonly IMsgPackFormatter<TKey> _keyFormatter;
        private readonly IMsgPackFormatter<TValue> _valueFormatter;

        public UsualFormatter(MsgPackContext context)
        {
            _keyFormatter = context.GetRequiredFormatter<TKey>();
            _valueFormatter = context.GetRequiredFormatter<TValue>();
        }

        public int GetBufferSize(TMap value) => value == null
            ? DataLengths.Nil
            : value.GetMapBufferSize(value.Count, _keyFormatter, _valueFormatter);

        public bool HasConstantSize => false;

        public int Format(Span<byte> destination, TMap value) => value == null
            ? MsgPackSpec.WriteNil(destination)
            : value.FormatTo(destination, value.Count, _keyFormatter, _valueFormatter);
    }
}
