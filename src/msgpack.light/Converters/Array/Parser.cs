using System;
using System.Buffers;
using System.Runtime.CompilerServices;

using ProGaudi.Buffers;

namespace ProGaudi.MsgPack.Converters.Array
{
    public sealed class Parser<TElement> : IMsgPackParser<IMemoryOwner<TElement>>, IMsgPackParser<TElement[]>
    {
        private readonly IMsgPackParser<TElement> _elementParser;

        public Parser(MsgPackContext context)
        {
            _elementParser = default;
        }

        IMemoryOwner<TElement> IMsgPackParser<IMemoryOwner<TElement>>.Parse(ReadOnlySpan<byte> source, out int readSize)
        {
            if (MsgPackSpec.TryReadNil(source, out readSize)) return null;

            var length = MsgPackSpec.ReadArrayHeader(source, out readSize);
            var result = FixedLengthMemoryPool<TElement>.Shared.Rent(length);
            Read(source, result.Memory.Span, ref readSize);

            return result;
        }

        TElement[] IMsgPackParser<TElement[]>.Parse(ReadOnlySpan<byte> source, out int readSize)
        {
            if (MsgPackSpec.TryReadNil(source, out readSize)) return null;

            var length = MsgPackSpec.ReadArrayHeader(source, out readSize);
            var array = new TElement[length];
            Read(source, array, ref readSize);

            return array;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Read(ReadOnlySpan<byte> source, Span<TElement> array, ref int readSize)
        {
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = _elementParser.Parse(source.Slice(readSize), out var temp);
                readSize += temp;
            }
        }
    }
}
