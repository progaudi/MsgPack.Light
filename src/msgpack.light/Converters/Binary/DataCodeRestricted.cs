using System;
using System.Buffers;

namespace ProGaudi.MsgPack.Converters.Binary
{
    public sealed class DataCodeRestricted : Converter
    {
        private readonly byte _code;

        private readonly Converter _reader;

        public DataCodeRestricted(byte code, bool compatibilityMode)
        {
            _code = code;
            var (min, max) = DataLengths.GetMinAndMaxLength(code);
            HasConstantSize = min == max;
            if (compatibilityMode)
            {
                if (!CompatibilitySpec.AllowedCodes.Contains(code)
                    && !(DataCodes.FixStringMin <= code && code <= DataCodes.FixStringMax))
                    throw ExceptionUtils.BadBinaryCompatibilityCode(code, CompatibilitySpec.AllowedCodes);
                _reader = Compatibility;
            }
            else
            {
                if (MsgPackSpec.GetDataFamily(code) != DataFamily.Binary)
                    throw ExceptionUtils.BadCodeConstraint(code, DataFamily.Binary);
                _reader = Current;
            }
        }

        public override int GetBufferSize(ReadOnlyMemory<byte> value)
        {
            switch (_code)
            {
                case DataCodes.Binary8:
                    return DataLengths.Binary8Header + value.Length;
                case DataCodes.Binary16:
                    return DataLengths.Binary16Header + value.Length;
                case DataCodes.Binary32:
                    return DataLengths.Binary32Header + value.Length;

                case DataCodes.String8:
                    return DataLengths.String8Header + value.Length;
                case DataCodes.String16:
                    return DataLengths.String16Header + value.Length;
                case DataCodes.String32:
                    return DataLengths.String32Header + value.Length;

                default:
                    if (DataCodes.FixStringMin <= _code && _code <= DataCodes.FixStringMax)
                        return DataLengths.FixStringHeader + (_code - DataCodes.FixStringMin);
                    throw ExceptionUtils.UnexpectedCode(_code);
            }
        }

        public override bool HasConstantSize { get; }

        public override int Format(Span<byte> destination, ReadOnlyMemory<byte> value)
        {
            int wroteSize;

            switch (_code)
            {
                case DataCodes.Binary8:
                    wroteSize = MsgPackSpec.WriteBinary8Header(destination, (byte) value.Length);
                    break;
                case DataCodes.Binary16:
                    wroteSize = MsgPackSpec.WriteBinary16Header(destination, (ushort) value.Length);
                    break;
                case DataCodes.Binary32:
                    wroteSize = MsgPackSpec.WriteBinary32Header(destination, (uint) value.Length);
                    break;

                case DataCodes.String8:
                    wroteSize = MsgPackSpec.WriteString8Header(destination, (byte) value.Length);
                    break;
                case DataCodes.String16:
                    wroteSize = MsgPackSpec.WriteString16Header(destination, (ushort) value.Length);
                    break;
                case DataCodes.String32:
                    wroteSize = MsgPackSpec.WriteString32Header(destination, (uint) value.Length);
                    break;

                default:
                    if (DataCodes.FixStringMin <= _code && _code <= DataCodes.FixStringMax)
                    {
                        wroteSize = MsgPackSpec.WriteFixStringHeader(destination, (byte) value.Length);
                        break;
                    }

                    throw ExceptionUtils.UnexpectedCode(_code);
            }

            value.Span.CopyTo(destination.Slice(wroteSize));
            return wroteSize + value.Length;
        }

        public override IMemoryOwner<byte> Parse(ReadOnlySpan<byte> source, out int readSize) => _reader.Parse(source, out readSize);

        public override IMemoryOwner<byte> Parse(ReadOnlySequence<byte> source, out int readSize) => _reader.Parse(source, out readSize);
    }
}
