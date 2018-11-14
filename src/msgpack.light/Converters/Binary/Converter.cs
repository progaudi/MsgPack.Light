using System;
using System.Buffers;

namespace ProGaudi.MsgPack.Converters.Binary
{
    public abstract class Converter : IMsgPackFormatter<ReadOnlyMemory<byte>?>, IMsgPackParser<IMemoryOwner<byte>>
    {
        public static readonly Converter Current = new CurrentSpec();

        public static readonly Converter Compatibility = new CompatibilitySpec();

        public abstract int GetBufferSize(ReadOnlyMemory<byte>? value);

        public abstract bool HasConstantSize { get; }

        public abstract int Format(Span<byte> destination, ReadOnlyMemory<byte>? value);

        public abstract IMemoryOwner<byte> Parse(ReadOnlySpan<byte> source, out int readSize);
    }
}
