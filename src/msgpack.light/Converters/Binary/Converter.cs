using System;
using System.Buffers;

namespace ProGaudi.MsgPack.Converters.Binary
{
    public abstract class Converter :
        IMsgPackFormatter<byte[]>,
        IMsgPackFormatter<ReadOnlyMemory<byte>>,
        IMsgPackFormatter<ReadOnlyMemory<byte>?>,
        IMsgPackParser<byte[]>,
        IMsgPackParser<IMemoryOwner<byte>>
    {
        public static readonly Converter Current = new CurrentSpec();

        public static readonly Converter Compatibility = new CompatibilitySpec();

        public abstract int GetBufferSize(ReadOnlyMemory<byte>? value);

        public int GetBufferSize(byte[] value) => GetBufferSize((ReadOnlyMemory<byte>?)value);

        public int GetBufferSize(ReadOnlyMemory<byte> value) => GetBufferSize((ReadOnlyMemory<byte>?)value);

        public abstract bool HasConstantSize { get; }

        public int Format(Span<byte> destination, ReadOnlyMemory<byte> value) => Format(destination, (ReadOnlyMemory<byte>?)value);

        public int Format(Span<byte> destination, byte[] value) => Format(destination, (ReadOnlyMemory<byte>?)value);

        public abstract int Format(Span<byte> destination, ReadOnlyMemory<byte>? value);

        public abstract IMemoryOwner<byte> Parse(ReadOnlySpan<byte> source, out int readSize);

        byte[] IMsgPackParser<byte[]>.Parse(ReadOnlySpan<byte> source, out int readSize)
        {
            using (var owner = Parse(source, out readSize))
            {
                return owner?.Memory.ToArray();
            }
        }
    }
}
