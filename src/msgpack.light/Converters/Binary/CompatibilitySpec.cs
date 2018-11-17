using System;
using System.Buffers;
using System.Collections.Generic;

using ProGaudi.Buffers;

namespace ProGaudi.MsgPack.Converters.Binary
{
    internal sealed class CompatibilitySpec : Converter
    {
        public override int GetBufferSize(ReadOnlyMemory<byte>? value)
        {
            if (value == null)
                return DataLengths.Nil;

            var length = value.Value.Length;
            return length + DataLengths.GetCompatibilityBinaryHeaderLength(length);
        }

        public override bool HasConstantSize => false;

        public override int Format(Span<byte> destination, ReadOnlyMemory<byte>? value)
        {
            if (value == null)
            {
                return MsgPackSpec.WriteNil(destination);
            }

            var span = value.Value.Span;
            var wroteSize = WriteStringHeaderAndLength(destination, span.Length);
            span.CopyTo(destination.Slice(wroteSize));
            return wroteSize + span.Length;

            int WriteStringHeaderAndLength(Span<byte> buffer, int length)
            {
                if (length <= 31)
                {
                    return MsgPackSpec.WriteFixStringHeader(buffer, (byte) length);
                }

                return length <= ushort.MaxValue
                    ? MsgPackSpec.WriteString16Header(buffer, (ushort) length)
                    : MsgPackSpec.WriteString32Header(buffer, (uint) length);
            }
        }

        public static readonly HashSet<byte> AllowedCodes = new HashSet<byte>
        {
            DataCodes.Binary8,
            DataCodes.Binary16,
            DataCodes.Binary32,
            DataCodes.String8,
            DataCodes.String16,
            DataCodes.String32
        };

        public override IMemoryOwner<byte> Parse(ReadOnlySpan<byte> source, out int readSize)
        {
            if (MsgPackSpec.TryReadNil(source, out readSize)) return null;
            var code = source[0];
            switch (code)
            {
                case DataCodes.Binary8:
                case DataCodes.Binary16:
                case DataCodes.Binary32:
                    return MsgPackSpec.ReadBinary(source, out readSize);

                case DataCodes.String8:
                case DataCodes.String16:
                case DataCodes.String32:
                    return ReadBinaryFromString(source, out readSize);
            }

            if (DataCodes.FixStringMin <= code && code <= DataCodes.FixStringMax)
            {
                return ReadBinaryFromString(source, out readSize);
            }

            throw ExceptionUtils.BadBinaryCompatibilityCode(code, AllowedCodes);

            IMemoryOwner<byte> ReadBinaryFromString(ReadOnlySpan<byte> buffer, out int r)
            {
                var length = MsgPackSpec.ReadStringHeader(buffer, out r);
                var result = FixedLengthMemoryPool<byte>.Shared.Rent(length);
                buffer.Slice(r).CopyTo(result.Memory.Span);
                r += length;
                return result;
            }
        }
    }
}
