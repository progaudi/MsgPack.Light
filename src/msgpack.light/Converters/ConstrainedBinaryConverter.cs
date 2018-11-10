using System;
using System.Buffers;

using JetBrains.Annotations;

namespace ProGaudi.MsgPack.Converters
{
    [PublicAPI]
    public sealed class ConstrainedBinaryConverter : IMsgPackFormatter<ReadOnlyMemory<byte>?>, IMsgPackParser<IMemoryOwner<byte>>
    {
        private BinaryConverter _serializer;

        public bool CompatibilityMode { get; }

        public int? MinSize { get; }

        public int? MaxSize { get; }

        public bool Nullable { get; }

        public byte? DataCodeRestriction { get; }

        public ConstrainedBinaryConverter(bool compatibilityMode, int? minSize = null, int? maxSize = null, bool nullable = true, byte? dataCodeRestriction = null)
        {
            CompatibilityMode = compatibilityMode;
            MinSize = minSize;
            MaxSize = maxSize;
            Nullable = nullable;
            DataCodeRestriction = dataCodeRestriction;

            if (minSize.HasValue && maxSize.HasValue && minSize > maxSize)
                throw ExceptionUtils.MinimumShouldBeLessThanOrEqualToMaximum(minSize.Value, maxSize.Value);
            
            _serializer = compatibilityMode ? BinaryConverter.Compatibility : BinaryConverter.Current;

            if (dataCodeRestriction == null) return;
            var dataCode = dataCodeRestriction.Value;
            _serializer = new BinaryConverter.DataCodeRestrictedSpec(dataCode, compatibilityMode);
            
            var (minLengthByCode, maxLengthByCode) = DataLengths.GetMinAndMaxLength(dataCode);
            if (minSize.HasValue)
            {
                if (maxLengthByCode < minSize) throw ExceptionUtils.MinSizeIsTooBigForDataCode(DataFamily.Binary, dataCode, minSize.Value);
                if (minSize < minLengthByCode) throw ExceptionUtils.MinSizeIsTooSmallForDataCode(DataFamily.Binary, dataCode, minSize.Value);
            }
            else
            {
                MinSize = minLengthByCode;
            }

            if (maxSize.HasValue)
            {
                if (maxLengthByCode < maxSize) throw ExceptionUtils.MaxSizeIsTooBigForDataCode(DataFamily.Binary, dataCode, maxSize.Value);
                if (minSize < maxLengthByCode) throw ExceptionUtils.MaxSizeIsTooSmallForDataCode(DataFamily.Binary, dataCode, maxSize.Value);
            }
            else
            {
                MaxSize = maxLengthByCode;
            }
        }

        // We will have problem with binary blobs greater than int.MaxValue bytes.
        public int GetBufferSize(ReadOnlyMemory<byte>? value) => value == null
            ? DataLengths.Nil
            : HasConstantSize
                // ReSharper disable once PossibleInvalidOperationException because HasConstantSize will check for it
                ? DataLengths.GetBinaryHeaderLength(MinSize.Value) + MinSize.Value
                : _serializer.GetBufferSize(value);

        public bool HasConstantSize => MinSize.HasValue && MinSize == MaxSize;

        public int Format(Span<byte> destination, ReadOnlyMemory<byte>? value)
        {
            if (value == null)
            {
                if (Nullable)
                    return MsgPackSpec.WriteNil(destination);

                throw ExceptionUtils.NonNullableConstraintIsViolated();
            }

            var memory = value.Value;
            if (MinSize.HasValue)
            {
                if (memory.Length < MinSize.Value)
                    throw ExceptionUtils.MinimumLengthConstraintIsViolated(MinSize.Value, memory.Length);
            }

            if (MaxSize.HasValue)
            {
                if (memory.Length > MaxSize.Value)
                    throw ExceptionUtils.MaximumLengthConstraintIsViolated(MaxSize.Value, memory.Length);
            }

            return _serializer.Format(destination, value);
        }

        public IMemoryOwner<byte> Parse(ReadOnlySpan<byte> source, out int readSize) => _serializer.Parse(source, out readSize);
    }
}