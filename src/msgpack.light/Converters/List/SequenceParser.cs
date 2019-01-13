using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ProGaudi.MsgPack.Converters.List
{
    public sealed class SequenceParser<TList, TElement> : IMsgPackSequenceParser<TList>
        where TList : IList<TElement>, new()
    {
        private readonly IMsgPackSequenceParser<TElement> _elementSequenceParser;

        public SequenceParser(MsgPackContext context)
        {
            _elementSequenceParser = context.GetRequiredSequenceParser<TElement>();
        }

        public TList Parse(ReadOnlySequence<byte> source, out int readSize)
        {
            return MsgPackSpec.TryReadNil(source, out readSize) ? default : Read(source, out readSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private TList Read(ReadOnlySequence<byte> source, out int readSize)
        {
            var length = MsgPackSpec.ReadArrayHeader(source, out readSize);
            var array = new TList();
            for (var i = 0; i < length; i++)
            {
                array.Add(_elementSequenceParser.Parse(source.Slice(readSize), out var temp));
                readSize += temp;
            }

            return array;
        }
    }
}
