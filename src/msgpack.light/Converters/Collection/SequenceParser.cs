using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ProGaudi.MsgPack.Converters.Collection
{
    public sealed class SequenceParser<TCollection, TElement> : IMsgPackSequenceParser<TCollection>
        where TCollection : ICollection<TElement>, new()
    {
        private readonly IMsgPackSequenceParser<TElement> _elementSequenceParser;

        public SequenceParser(MsgPackContext context)
        {
            _elementSequenceParser = context.GetRequiredSequenceParser<TElement>();
        }

        public TCollection Parse(ReadOnlySequence<byte> source, out int readSize)
        {
            return MsgPackSpec.TryReadNil(source, out readSize) ? default : Read(source, out readSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private TCollection Read(ReadOnlySequence<byte> source, out int readSize)
        {
            var length = MsgPackSpec.ReadArrayHeader(source, out readSize);
            var array = new TCollection();
            for (var i = 0; i < length; i++)
            {
                array.Add(_elementSequenceParser.Parse(source.Slice(readSize), out var temp));
                readSize += temp;
            }

            return array;
        }
    }
}
