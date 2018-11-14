using System;
using System.Collections.Generic;

namespace ProGaudi.MsgPack.Converters.Collection
{
    public sealed class UsualFormatter<TCollection, TElement> : IMsgPackFormatter<TCollection>
        where TCollection : ICollection<TElement>
    {
        private readonly IMsgPackFormatter<TElement> _elementFormatter;

        public UsualFormatter(MsgPackContext context)
        {
            _elementFormatter = default;
        }

        int IMsgPackFormatter<TCollection>.GetBufferSize(TCollection value) => value.GetBufferSize(_elementFormatter);

        public bool HasConstantSize => false;

        int IMsgPackFormatter<TCollection>.Format(Span<byte> destination, TCollection value)
        {
            if (value == null) return MsgPackSpec.WriteNil(destination);

            var result = MsgPackSpec.WriteArrayHeader(destination, value.Count);
            foreach (var element in value)
            {
                result += _elementFormatter.Format(destination.Slice(result), element);
            }

            return result;
        }
    }
}