using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace ProGaudi.MsgPack.Converters
{
    internal static class Extensions
    {
        internal static (int? minSize, int? maxSize) ValidateMinMaxCode(byte code, int? minSize, int? maxSize)
        {
            var (minLengthByCode, maxLengthByCode) = DataLengths.GetMinAndMaxLength(code);
            if (minSize.HasValue)
            {
                if (maxLengthByCode < minSize)
                    throw ExceptionUtils.MinSizeIsTooBigForDataCode(DataFamily.Binary, code, minSize.Value);
                if (minSize < minLengthByCode)
                    throw ExceptionUtils.MinSizeIsTooSmallForDataCode(DataFamily.Binary, code, minSize.Value);
            }
            else
            {
                minSize = minLengthByCode;
            }

            if (maxSize.HasValue)
            {
                if (maxLengthByCode < maxSize)
                    throw ExceptionUtils.MaxSizeIsTooBigForDataCode(DataFamily.Binary, code, maxSize.Value);
                if (maxSize < maxLengthByCode)
                    throw ExceptionUtils.MaxSizeIsTooSmallForDataCode(DataFamily.Binary, code, maxSize.Value);
            }
            else
            {
                maxSize = maxLengthByCode;
            }

            return (minSize, maxSize);
        }

        internal static int WriteNil(Span<byte> destination, bool nullable)
        {
            if (nullable)
                return MsgPackSpec.WriteNil(destination);

            throw ExceptionUtils.NonNullableConstraintIsViolated();
        }

        internal static void CheckMinMax(int length, int? minSize, int? maxSize)
        {
            if (minSize.HasValue)
            {
                if (length < minSize.Value)
                    throw ExceptionUtils.MinimumLengthConstraintIsViolated(minSize.Value, length);
            }

            if (maxSize.HasValue)
            {
                if (length > maxSize.Value)
                    throw ExceptionUtils.MaximumLengthConstraintIsViolated(maxSize.Value, length);
            }
        }

        internal static int GetMapBufferSize<TKey, TValue>([NotNull]this IEnumerable<KeyValuePair<TKey, TValue>> value, int count, IMsgPackFormatter<TKey> keyFormatter, IMsgPackFormatter<TValue> valueFormatter)
        {
            var sum = DataLengths.GetMapHeaderLength(count);
            foreach (var pair in value)
            {
                sum += keyFormatter.GetBufferSize(pair.Key);
                sum += valueFormatter.GetBufferSize(pair.Value);
            }

            return sum;
        }

        internal static int FormatTo<TKey, TValue>([NotNull]this IEnumerable<KeyValuePair<TKey, TValue>> value, Span<byte> destination, int count, IMsgPackFormatter<TKey> keyFormatter, IMsgPackFormatter<TValue> valueFormatter)
        {
            var sum = MsgPackSpec.WriteMapHeader(destination, count);
            foreach (var pair in value)
            {
                sum += keyFormatter.Format(destination.Slice(sum), pair.Key);
                sum += valueFormatter.Format(destination.Slice(sum), pair.Value);
            }

            return sum;
        }
    }
}
