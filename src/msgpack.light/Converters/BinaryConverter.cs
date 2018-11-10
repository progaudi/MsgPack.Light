using System;
using System.Buffers;
using System.Collections.Generic;

using JetBrains.Annotations;

using ProGaudi.Buffers;

namespace ProGaudi.MsgPack.Converters
{
    [PublicAPI]
    public abstract class BinaryConverter : IMsgPackFormatter<ReadOnlyMemory<byte>?>, IMsgPackParser<IMemoryOwner<byte>>
    {
        public static readonly BinaryConverter Current = new CurrentSpec();

        public static readonly BinaryConverter Compatibility = new CompatibilitySpec();

        public abstract int GetBufferSize(ReadOnlyMemory<byte>? value);

        public abstract bool HasConstantSize { get; }

        public abstract int Format(Span<byte> destination, ReadOnlyMemory<byte>? value);

        public abstract IMemoryOwner<byte> Parse(ReadOnlySpan<byte> source, out int readSize);

        public sealed class CurrentSpec : BinaryConverter
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

        public sealed class CompatibilitySpec : BinaryConverter
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
                DataCodes.String32,
            };

            public override IMemoryOwner<byte> Parse(ReadOnlySpan<byte> source, out int readSize)
            {
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

        public sealed class DataCodeRestrictedSpec : BinaryConverter
        {
            private readonly byte _code;

            private IMsgPackParser<IMemoryOwner<byte>> _reader;

            public DataCodeRestrictedSpec(byte code, bool compatibilityMode)
            {
                _code = code;
                var (min, max) = DataLengths.GetMinAndMaxLength(code);
                HasConstantSize = min == max;
                if (compatibilityMode)
                {
                    if (!CompatibilitySpec.AllowedCodes.Contains(code) && !(DataCodes.FixStringMin <= code && code <= DataCodes.FixStringMax))
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

            public override int GetBufferSize(ReadOnlyMemory<byte>? value)
            {
                if (value == null) return DataLengths.Nil;

                var memory = value.Value;
                switch (_code)
                {
                    case DataCodes.Binary8:
                        return DataLengths.Binary8Header + memory.Length;
                    case DataCodes.Binary16:
                        return DataLengths.Binary16Header + memory.Length;
                    case DataCodes.Binary32:
                        return DataLengths.Binary32Header + memory.Length;

                    case DataCodes.String8:
                        return DataLengths.String8Header + memory.Length;
                    case DataCodes.String16:
                        return DataLengths.String16Header + memory.Length;
                    case DataCodes.String32:
                        return DataLengths.String32Header + memory.Length;
                    
                    default:
                        if (DataCodes.FixStringMin <= _code && _code <= DataCodes.FixStringMax)
                            return DataLengths.FixStringHeader + (_code - DataCodes.FixStringMin);
                        throw ExceptionUtils.UnexpectedCode(_code);
                }
            }

            public override bool HasConstantSize { get; }

            public override int Format(Span<byte> destination, ReadOnlyMemory<byte>? value)
            {
                if (value == null) return MsgPackSpec.WriteNil(destination);

                var memory = value.Value;
                int wroteSize;

                switch (_code)
                {
                    case DataCodes.Binary8:
                        wroteSize = MsgPackSpec.WriteBinary8Header(destination, (byte) memory.Length);
                        break;
                    case DataCodes.Binary16:
                        wroteSize = MsgPackSpec.WriteBinary16Header(destination, (ushort) memory.Length);
                        break;
                    case DataCodes.Binary32:
                        wroteSize = MsgPackSpec.WriteBinary32Header(destination, (uint) memory.Length);
                        break;

                    case DataCodes.String8:
                        wroteSize = MsgPackSpec.WriteString8Header(destination, (byte) memory.Length);
                        break;
                    case DataCodes.String16:
                        wroteSize = MsgPackSpec.WriteString16Header(destination, (ushort) memory.Length);
                        break;
                    case DataCodes.String32:
                        wroteSize = MsgPackSpec.WriteString32Header(destination, (uint) memory.Length);
                        break;

                    default:
                        if (DataCodes.FixStringMin <= _code && _code <= DataCodes.FixStringMax)
                        {
                            wroteSize = MsgPackSpec.WriteFixStringHeader(destination, (byte) memory.Length);
                            break;
                        }

                        throw ExceptionUtils.UnexpectedCode(_code);
                }
                
                memory.Span.CopyTo(destination.Slice(wroteSize));
                return wroteSize + memory.Length;
            }

            public override IMemoryOwner<byte> Parse(ReadOnlySpan<byte> source, out int readSize) => _reader.Parse(source, out readSize);
        }
    }
}
