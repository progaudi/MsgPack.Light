using System;
using System.Buffers;

namespace ProGaudi.MsgPack.Converters.Binary
{
    public abstract class Converter :
        IMsgPackFormatter<byte[]>,
        IMsgPackFormatter<ReadOnlyMemory<byte>>,
        IMsgPackParser<byte[]>,
        IMsgPackParser<IMemoryOwner<byte>>
    {
        public static readonly Converter Current = new CurrentSpec();

        public static readonly Converter Compatibility = new CompatibilitySpec();

        public int GetBufferSize(byte[] value) => value == null
            ? DataLengths.Nil
            : GetBufferSize((ReadOnlyMemory<byte>)value);

        public abstract int GetBufferSize(ReadOnlyMemory<byte> value);

        public abstract bool HasConstantSize { get; }

        public abstract int Format(Span<byte> destination, ReadOnlyMemory<byte> value);

        public int Format(Span<byte> destination, byte[] value) => value == null
            ? MsgPackSpec.WriteNil(destination)
            : Format(destination, (ReadOnlyMemory<byte>)value);

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
