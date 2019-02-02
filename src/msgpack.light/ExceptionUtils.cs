using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ProGaudi.MsgPack
{
    public static class ExceptionUtils
    {
        public static Exception NotEnoughBytes(int actual, int expected)
        {
            return new SerializationException($"Expected {expected} bytes, got {actual} bytes.");
        }

        public static Exception IntSerializationFailure(long value)
        {
            return new SerializationException($"Can't serialize {value}");
        }

        public static Exception IntSerializationFailure(ulong value)
        {
            return new SerializationException($"Can't serialize {value}");
        }

        public static Exception NonNullableConstraintIsViolated() => new NonNullableConstraintViolationException();

        public static Exception MinimumLengthConstraintIsViolated(int minSize, int actualLength) => new MinimumConstraintViolationException(minSize, actualLength);

        public static Exception MaximumLengthConstraintIsViolated(int maxSize, int actualLength) => new MaximumConstraintViolationException(maxSize, actualLength);

        public static Exception BadCodeConstraint(byte dataCode, DataFamily binary) => new CodeDoesntBelongFamilyException(dataCode, binary);

        public static Exception MinSizeIsTooBigForDataCode(DataFamily dataFamily, byte dataCode, int minSize)
        {
            return new BadSizeConstraintException(dataFamily, dataCode, minSize);
        }

        public static Exception MinSizeIsTooSmallForDataCode(DataFamily dataFamily, byte dataCode, int minSize)
        {
            return new BadSizeConstraintException(dataFamily, dataCode, minSize);
        }

        public static Exception MaxSizeIsTooBigForDataCode(DataFamily dataFamily, byte dataCode, int maxSize)
        {
            return new BadSizeConstraintException(dataFamily, dataCode, maxSize);
        }

        public static Exception MaxSizeIsTooSmallForDataCode(DataFamily dataFamily, byte dataCode, int maxSize)
        {
            return new BadSizeConstraintException(dataFamily, dataCode, maxSize);
        }

        public static Exception MinimumShouldBeLessThanOrEqualToMaximum(int minSize, int maxSize)
        {
            return new MinimumShouldBeLessThanMaximumException(minSize, maxSize);
        }

        public static Exception BadBinaryCompatibilityCode(byte code, HashSet<byte> allowedCodes)
        {
            return new BadCodeConstraintException(code, allowedCodes);
        }

        public static Exception UnexpectedCode(byte code) => new UnexpectedCodeException(code);
    }
}
