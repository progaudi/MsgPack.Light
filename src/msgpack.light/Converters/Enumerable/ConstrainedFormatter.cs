using System;
using System.Collections.Generic;

namespace ProGaudi.MsgPack.Converters.Enumerable
{
    public sealed class ConstrainedFormatter<TEnumerable, TElement> : IMsgPackFormatter<TEnumerable>
        where TEnumerable : IEnumerable<TElement>
    {
        public byte Code { get; }

        public int? MinSize { get; }

        public int? MaxSize { get; }

        public bool Nullable { get; }

        private readonly IMsgPackFormatter<TElement> _elementFormatter;

        public ConstrainedFormatter(
            MsgPackContext context,
            byte? code = null,
            int? minSize = null,
            int? maxSize = null,
            bool nullable = true)
        {
            Code = code ?? DataCodes.Array32;
            MinSize = minSize;
            MaxSize = maxSize;
            Nullable = nullable;

            _elementFormatter = default;

            if (minSize.HasValue && maxSize.HasValue)
            {
                throw ExceptionUtils.MinimumShouldBeLessThanOrEqualToMaximum(minSize.Value, maxSize.Value);
            }

            if (MsgPackSpec.GetDataFamily(Code) != DataFamily.Array)
                throw ExceptionUtils.BadCodeConstraint(Code, DataFamily.Array);

            (MinSize, MaxSize) = MsgPack.Extensions.ValidateMinMaxCode(Code, minSize, maxSize);
        }

        int IMsgPackFormatter<TEnumerable>.GetBufferSize(TEnumerable value) => value.GetBufferSize(_elementFormatter);

        public bool HasConstantSize => !Nullable && _elementFormatter.HasConstantSize && MinSize.HasValue && MinSize == MaxSize;

        int IMsgPackFormatter<TEnumerable>.Format(Span<byte> destination, TEnumerable value)
        {
            if (value == null) return MsgPackSpec.WriteNil(destination);

            var span = destination.Slice(DataLengths.GetHeaderLength(Code));
            var length = 0;
            var result = 0;
            foreach (var element in value)
            {
                result += _elementFormatter.Format(span.Slice(result), element);
                length += 1;
            }
            MsgPack.Extensions.CheckMinMax(length, MinSize, MaxSize);
            result += WriteHeader(destination, length);

            return result;
        }

        private int WriteHeader(Span<byte> buffer, int length)
        {
            switch (Code)
            {
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