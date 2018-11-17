using System;

namespace ProGaudi.MsgPack.Converters.Array
{
    public sealed class UsualFormatter<TElement> : IMsgPackFormatter<ReadOnlyMemory<TElement>?>
    {
        private readonly IMsgPackFormatter<TElement> _elementFormatter;

        public UsualFormatter(MsgPackContext context)
        {
            _elementFormatter = context.GetRequiredFormatter<TElement>();
        }

        public int GetBufferSize(ReadOnlyMemory<TElement>? value) => value.GetBufferSize(_elementFormatter);

        public bool HasConstantSize => false;

        public int Format(Span<byte> destination, ReadOnlyMemory<TElement>? value)
        {
            if (value == null) return MsgPackSpec.WriteNil(destination);

            var span = value.Value.Span;
            var result = MsgPackSpec.WriteArrayHeader(destination, span.Length);
            for (var i = 0; i < span.Length; i++)
            {
                result += _elementFormatter.Format(destination.Slice(result), span[i]);
            }

            return result;
        }
    }
}
