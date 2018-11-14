using System;
using System.Buffers;

namespace ProGaudi.MsgPack.Converters.Binary
{
    public sealed class Constrained : IMsgPackFormatter<ReadOnlyMemory<byte>?>, IMsgPackParser<IMemoryOwner<byte>>
    {
        private readonly Converter _serializer;

        public bool CompatibilityMode { get; }

        public int? MinSize { get; }

        public int? MaxSize { get; }

        public bool Nullable { get; }

        public byte? DataCodeRestriction { get; }

        public Constrained(
            bool compatibilityMode,
            int? minSize = null,
            int? maxSize = null,
            bool nullable = true,
            byte? dataCodeRestriction = null)
        {
            CompatibilityMode = compatibilityMode;
            MinSize = minSize;
            MaxSize = maxSize;
            Nullable = nullable;
            DataCodeRestriction = dataCodeRestriction;

            if (minSize.HasValue && maxSize.HasValue && minSize > maxSize)
                throw ExceptionUtils.MinimumShouldBeLessThanOrEqualToMaximum(minSize.Value, maxSize.Value);

            _serializer = compatibilityMode ? Converter.Compatibility : Converter.Current;

            if (dataCodeRestriction == null) return;
            var dataCode = dataCodeRestriction.Value;
            _serializer = new DataCodeRestricted(dataCode, compatibilityMode);
            (MinSize, MaxSize) = Extensions.ValidateMinMaxCode(dataCode, minSize, maxSize);
        }

        // We will have problem with binary blobs greater than int.MaxValue bytes.
        public int GetBufferSize(ReadOnlyMemory<byte>? value)
        {
            if (value == null)
                return DataLengths.Nil;

            return HasConstantSize
                // ReSharper disable once PossibleInvalidOperationException because HasConstantSize will check for it
                ? DataLengths.GetBinaryHeaderLength(MinSize.Value) + MinSize.Value
                : _serializer.GetBufferSize(value);
        }

        public bool HasConstantSize => !Nullable && MinSize.HasValue && MinSize == MaxSize;

        public int Format(Span<byte> destination, ReadOnlyMemory<byte>? value)
        {
            if (value == null)
            {
                return Extensions.WriteNil(destination, Nullable);
            }

            Extensions.CheckMinMax(value.Value.Length, MinSize, MaxSize);

            return _serializer.Format(destination, value);
        }

        public IMemoryOwner<byte> Parse(ReadOnlySpan<byte> source, out int readSize) => _serializer.Parse(source, out readSize);
    }
}