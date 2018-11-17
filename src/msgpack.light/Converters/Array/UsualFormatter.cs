using System;

namespace ProGaudi.MsgPack.Converters.Array
{
    public sealed class UsualFormatter<TElement> : IMsgPackFormatter<ReadOnlyMemory<TElement>>, IMsgPackFormatter<TElement[]>
    {
        private readonly IMsgPackFormatter<TElement> _elementFormatter;

        public UsualFormatter(MsgPackContext context)
        {
            _elementFormatter = context.GetRequiredFormatter<TElement>();
        }

        public int GetBufferSize(ReadOnlyMemory<TElement> value) => value.GetBufferSize(_elementFormatter);

        public int GetBufferSize(TElement[] value) => GetBufferSize((ReadOnlyMemory<TElement>)value);

        public bool HasConstantSize => false;

        public int Format(Span<byte> destination, TElement[] value) => Format(destination, (ReadOnlyMemory<TElement>)value);

        public int Format(Span<byte> destination, ReadOnlyMemory<TElement> value)
        {
            var span = value.Span;
            var result = MsgPackSpec.WriteArrayHeader(destination, span.Length);
            for (var i = 0; i < span.Length; i++)
            {
                result += _elementFormatter.Format(destination.Slice(result), span[i]);
            }

            return result;
        }
    }
}
