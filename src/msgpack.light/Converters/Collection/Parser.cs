using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ProGaudi.MsgPack.Converters.Collection
{
    public sealed class Parser<TCollection, TElement> : IMsgPackParser<TCollection>
        where TCollection : ICollection<TElement>, new()
    {
        private readonly IMsgPackParser<TElement> _elementParser;

        public Parser(MsgPackContext context)
        {
            _elementParser = context.GetRequiredParser<TElement>();
        }

        public TCollection Parse(ReadOnlySpan<byte> source, out int readSize)
        {
            return MsgPackSpec.TryReadNil(source, out readSize) ? default : Read(source, out readSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private TCollection Read(ReadOnlySpan<byte> source, out int readSize)
        {
            var length = MsgPackSpec.ReadArrayHeader(source, out readSize);
            var array = new TCollection();
            for (var i = 0; i < length; i++)
            {
                array.Add(_elementParser.Parse(source.Slice(readSize), out var temp));
                readSize += temp;
            }

            return array;
        }
    }
}
