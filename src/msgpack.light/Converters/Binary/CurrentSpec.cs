using System;
using System.Buffers;

namespace ProGaudi.MsgPack.Converters.Binary
{
    internal sealed class CurrentSpec : Converter
    {
        public override int GetBufferSize(ReadOnlyMemory<byte>? value)
        {
            if (value == null)
                return DataLengths.Nil;

            var length = value.Value.Length;
            return DataLengths.GetBinaryHeaderLength(length) + length;
        }

        public override bool HasConstantSize => false;

        public override int Format(Span<byte> destination, ReadOnlyMemory<byte>? value)
        {
            if (value == null)
            {
                return MsgPackSpec.WriteNil(destination);
            }

            var span = value.Value.Span;
            return MsgPackSpec.WriteBinary(destination, span);
        }

        public override IMemoryOwner<byte> Parse(ReadOnlySpan<byte> source, out int readSize)
        {
            if (source[0] == DataCodes.Nil)
            {
                readSize = DataLengths.Nil;
                return null;
            }

            return MsgPackSpec.ReadBinary(source, out readSize);
        }
    }
}