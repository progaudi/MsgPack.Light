using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ProGaudi.MsgPack.Converters.List
{
    public sealed class Parser<TList, TElement> : IMsgPackParser<TList>
        where TList : IList<TElement>, new()
    {
        private readonly IMsgPackParser<TElement> _elementParser;

        public Parser(MsgPackContext context)
        {
            _elementParser = context.GetRequiredParser<TElement>();
        }

        public TList Parse(ReadOnlySpan<byte> source, out int readSize)
        {
            return MsgPackSpec.TryReadNil(source, out readSize) ? default : Read(source, out readSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private TList Read(ReadOnlySpan<byte> source, out int readSize)
        {
            var length = MsgPackSpec.ReadArrayHeader(source, out readSize);
            var array = new TList();
            for (var i = 0; i < length; i++)
            {
                array.Add(_elementParser.Parse(source.Slice(readSize), out var temp));
                readSize += temp;
            }

            return array;
        }
    }
}
