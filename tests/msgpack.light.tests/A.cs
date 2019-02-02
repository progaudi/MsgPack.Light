using System;
using System.Buffers;

namespace ProGaudi.MsgPack.Light.Tests
{
    public struct A<T>
    {
        public T F { get; set; }
    }

    public class GenericFormatter<T> : IMsgPackFormatter<A<T>>
    {
        private readonly IMsgPackFormatter<T> _formatter;

        public GenericFormatter(MsgPackContext context) => _formatter = context.GetRequiredFormatter<T>();

        public int GetBufferSize(A<T> value) => _formatter.GetBufferSize(value.F);

        public bool HasConstantSize => false;

        public int Format(Span<byte> destination, A<T> value) => _formatter.Format(destination, value.F);
    }

    public class GenericParser<T> : IMsgPackParser<A<T>>
    {
        private readonly IMsgPackParser<T> _parser;

        public GenericParser(MsgPackContext context) => _parser = context.GetRequiredParser<T>();

        public A<T> Parse(ReadOnlySpan<byte> source, out int readSize)
        {
            return new A<T> {F = _parser.Parse(source, out readSize)};
        }
    }

    public class GenericSequenceParser<T> : IMsgPackSequenceParser<A<T>>
    {
        private readonly IMsgPackSequenceParser<T> _parser;

        public GenericSequenceParser(MsgPackContext context) => _parser = context.GetRequiredSequenceParser<T>();

        public A<T> Parse(ReadOnlySequence<byte> source, out int readSize)
        {
            return new A<T> {F = _parser.Parse(source, out readSize)};
        }
    }
}
