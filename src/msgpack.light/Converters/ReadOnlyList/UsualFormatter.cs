using System;
using System.Collections.Generic;

namespace ProGaudi.MsgPack.Converters.ReadOnlyList
{
    public sealed class UsualFormatter<TList, TElement> : IMsgPackFormatter<TList>
        where TList : IReadOnlyList<TElement>
    {
        private readonly IMsgPackFormatter<TElement> _elementFormatter;

        public UsualFormatter(MsgPackContext context)
        {
            _elementFormatter = context.GetRequiredFormatter<TElement>();
        }

        int IMsgPackFormatter<TList>.GetBufferSize(TList value) => value.GetBufferSize(_elementFormatter);

        public bool HasConstantSize => false;

        int IMsgPackFormatter<TList>.Format(Span<byte> destination, TList value)
        {
            if (value == null) return MsgPackSpec.WriteNil(destination);

            var result = MsgPackSpec.WriteArrayHeader(destination, value.Count);
            for (var i = 0; i < value.Count; i++)
            {
                result += _elementFormatter.Format(destination.Slice(result), value[i]);
            }

            return result;
        }
    }
}
