using System;

namespace ProGaudi.MsgPack.Converters.String
{
    public sealed class ConstrainedFormatter : IMsgPackFormatter<string>
    {
        public byte? Code { get; }

        public int? MinSize { get; }

        public int? MaxSize { get; }

        public bool Nullable { get; }

        public ConstrainedFormatter(byte? code = null, int? minSize = null, int? maxSize = null, bool nullable = true)
        {
            Code = code;
            MinSize = minSize;
            MaxSize = maxSize;
            Nullable = nullable;

            if (minSize.HasValue && maxSize.HasValue && minSize > maxSize)
            {
                throw ExceptionUtils.MinimumShouldBeLessThanOrEqualToMaximum(minSize.Value, maxSize.Value);
            }

            if (code == null) return;

            var codeValue = code.Value;

            if (MsgPackSpec.GetDataFamily(codeValue) != DataFamily.String)
                throw ExceptionUtils.BadCodeConstraint(codeValue, DataFamily.String);

            (MinSize, MaxSize) = Extensions.ValidateMinMaxCode(code.Value, minSize, maxSize);
        }

        public int GetBufferSize(string value) => value == null
            ? DataLengths.Nil
            : DataLengths.GetStringHeaderLengthByBytesCount(value.Length * 4) + value.Length * 4;

        public bool HasConstantSize => !Nullable && MinSize.HasValue && MinSize == MaxSize;

        public int Format(Span<byte> destination, string value)
        {
            if (value == null) return MsgPackSpec.WriteNil(destination);

            var length = value.Length;
            Extensions.CheckMinMax(length, MinSize, MaxSize);

            switch (Code)
            {
                case null:
                    return MsgPackSpec.WriteString(destination, value.AsSpan());
                case DataCodes.String8:
                    return MsgPackSpec.WriteString8(destination, value.AsSpan());
                case DataCodes.String16:
                    return MsgPackSpec.WriteString16(destination, value.AsSpan());
                case DataCodes.String32:
                    return MsgPackSpec.WriteString32(destination, value.AsSpan());
                default:
                    return MsgPackSpec.WriteFixString(destination, value.AsSpan());
            }
        }
    }
}