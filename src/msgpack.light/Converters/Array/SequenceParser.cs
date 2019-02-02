using System;
using System.Buffers;
using System.Runtime.CompilerServices;

using ProGaudi.Buffers;

namespace ProGaudi.MsgPack.Converters.Array
{
    public sealed class SequenceParser<TElement> : IMsgPackSequenceParser<IMemoryOwner<TElement>>, IMsgPackSequenceParser<TElement[]>
    {
        private readonly IMsgPackSequenceParser<TElement> _elementSequenceParser;

        public SequenceParser(MsgPackContext context)
        {
            _elementSequenceParser = context.GetRequiredSequenceParser<TElement>();
        }

        IMemoryOwner<TElement> IMsgPackSequenceParser<IMemoryOwner<TElement>>.Parse(ReadOnlySequence<byte> source, out int readSize)
        {
            if (MsgPackSpec.TryReadNil(source, out readSize)) return null;

            var length = MsgPackSpec.ReadArrayHeader(source, out readSize);
            var result = FixedLengthMemoryPool<TElement>.Shared.Rent(length);
            Read(source, result.Memory.Span, ref readSize);

            return result;
        }

        TElement[] IMsgPackSequenceParser<TElement[]>.Parse(ReadOnlySequence<byte> source, out int readSize)
        {
            if (MsgPackSpec.TryReadNil(source, out readSize)) return null;

            var length = MsgPackSpec.ReadArrayHeader(source, out readSize);
            var array = new TElement[length];
            Read(source, array, ref readSize);

            return array;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Read(ReadOnlySequence<byte> source, Span<TElement> array, ref int readSize)
        {
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = _elementSequenceParser.Parse(source.Slice(readSize), out var temp);
                readSize += temp;
            }
        }
    }
}
