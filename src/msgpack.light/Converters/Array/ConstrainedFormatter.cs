using System;

namespace ProGaudi.MsgPack.Converters.Array
{
    public sealed class ConstrainedFormatter<TElement> : IMsgPackFormatter<TElement[]>
    {
        public byte? Code { get; }

        public int? MinSize { get; }

        public int? MaxSize { get; }

        public bool Nullable { get; }

        private readonly IMsgPackFormatter<TElement> _elementFormatter;

        public ConstrainedFormatter(MsgPackContext context, byte? code = null, int? minSize = null, int? maxSize = null, bool nullable = true)
        {
            Code = code;
            MinSize = minSize;
            MaxSize = maxSize;
            Nullable = nullable;

            _elementFormatter = context.GetRequiredFormatter<TElement>();

            if (minSize.HasValue && maxSize.HasValue && minSize > maxSize)
            {
                throw ExceptionUtils.MinimumShouldBeLessThanOrEqualToMaximum(minSize.Value, maxSize.Value);
            }

            if (code == null) return;

            var codeValue = code.Value;

            if (MsgPackSpec.GetDataFamily(codeValue) != DataFamily.Array)
                throw ExceptionUtils.BadCodeConstraint(codeValue, DataFamily.Array);

            (MinSize, MaxSize) = Converters.Extensions.ValidateMinMaxCode(code.Value, minSize, maxSize);
        }

        public int GetBufferSize(TElement[] value) => value == null
            ? DataLengths.Nil
            : ((ReadOnlyMemory<TElement>) value).GetBufferSize(_elementFormatter);

        public bool HasConstantSize => !Nullable && _elementFormatter.HasConstantSize && MinSize.HasValue && MinSize == MaxSize;

        public int Format(Span<byte> destination, TElement[] value)
        {
            if (value == null) return MsgPackSpec.WriteNil(destination);

            var span = value.AsSpan();
            var length = span.Length;
            Converters.Extensions.CheckMinMax(length, MinSize, MaxSize);

            var result = WriteHeader(destination);

            for (var i = 0; i < length; i++)
            {
                result += _elementFormatter.Format(destination.Slice(result), span[i]);
            }

            return result;

            int WriteHeader(Span<byte> buffer)
            {
                switch (Code)
                {
                    case null:
                        return MsgPackSpec.WriteArrayHeader(buffer, length);
                    case DataCodes.Array16:
                        return MsgPackSpec.WriteArray16Header(buffer, (ushort) length);
                    case DataCodes.Array32:
                        return MsgPackSpec.WriteArray32Header(buffer, (uint) length);
                    default:
                        return MsgPackSpec.WriteFixArrayHeader(buffer, (byte) length);
                }
            }
        }
    }
}
