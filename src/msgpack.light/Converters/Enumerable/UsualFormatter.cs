using System;
using System.Collections.Generic;

namespace ProGaudi.MsgPack.Converters.Enumerable
{
    public sealed class UsualFormatter<TEnumerable, TElement> : IMsgPackFormatter<TEnumerable>
        where TEnumerable : IEnumerable<TElement>
    {
        private readonly IMsgPackFormatter<TEnumerable> _impl;

        public UsualFormatter(MsgPackContext context)
        {
            _impl = new ConstrainedFormatter<TEnumerable, TElement>(context, DataCodes.Array32);
        }

        public int GetBufferSize(TEnumerable value) => _impl.GetBufferSize(value);

        public bool HasConstantSize => _impl.HasConstantSize;

        public int Format(Span<byte> destination, TEnumerable value) => _impl.Format(destination, value);
    }
}