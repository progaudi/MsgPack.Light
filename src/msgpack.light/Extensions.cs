using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace ProGaudi.MsgPack
{
    internal static class Extensions
    {
        public static TValue GetOrAdd<TKey, TValue>([NotNull]this Dictionary<TKey, TValue> dictionary, [NotNull]TKey key, [NotNull]Func<TKey, TValue> creator)
        {
            if (!dictionary.TryGetValue(key, out var temp))
                dictionary[key] = temp = creator(key);
            return temp;
        }

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

        public static void CheckMinMax(int length, int? minSize, int? maxSize)
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
    }
}
