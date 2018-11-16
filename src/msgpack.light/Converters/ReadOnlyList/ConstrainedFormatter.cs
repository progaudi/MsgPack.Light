using System;
using System.Collections.Generic;

namespace ProGaudi.MsgPack.Converters.ReadOnlyList
{
    public sealed class ConstrainedFormatter<TList, TElement> : IMsgPackFormatter<TList>
        where TList : IReadOnlyList<TElement>
    {
        public byte? Code { get; }

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
            Code = code;
            MinSize = minSize;
            MaxSize = maxSize;
            Nullable = nullable;

            _elementFormatter = default;

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

        int IMsgPackFormatter<TList>.GetBufferSize(TList value) => value.GetBufferSize(_elementFormatter);

        public bool HasConstantSize => !Nullable && _elementFormatter.HasConstantSize && MinSize.HasValue && MinSize == MaxSize;

        int IMsgPackFormatter<TList>.Format(Span<byte> destination, TList value)
        {
            if (value == null) return MsgPackSpec.WriteNil(destination);

            var span = value;
            var length = span.Count;
            Converters.Extensions.CheckMinMax(length, MinSize, MaxSize);

            var result = WriteHeader(destination, length);

            for (var i = 0; i < length; i++)
            {
                result += _elementFormatter.Format(destination.Slice(result), span[i]);
            }

            return result;
        }

        private int WriteHeader(Span<byte> buffer, int length)
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
