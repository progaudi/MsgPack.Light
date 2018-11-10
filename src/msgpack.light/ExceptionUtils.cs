using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

using ProGaudi.MsgPack.Converters.Generation.Exceptions;

namespace ProGaudi.MsgPack
{
    public static class ExceptionUtils
    {
        public static Exception BadTypeException(DataTypes actual, params DataTypes[] expectedCodes)
        {
            return new SerializationException($"Got {actual:G} (0x{actual:X}), while expecting one of these: {String.Join(", ", expectedCodes)}");
        }

        public static Exception CantReadStringAsBinary()
        {
            return new SerializationException("Reading a string as a byte array is disabled. Set \'binaryCompatibilityMode\' parameter in MsgPackContext constructor to true to enable it");
        }

        public static Exception NotEnoughBytes(uint actual, uint expected)
        {
            return new SerializationException($"Expected {expected} bytes, got {actual} bytes.");
        }

        public static Exception NotEnoughBytes(int actual, int expected)
        {
            return new SerializationException($"Expected {expected} bytes, got {actual} bytes.");
        }

        public static Exception NotEnoughBytes(long actual, uint expected)
        {
            return new SerializationException($"Expected {expected} bytes, got {actual} bytes.");
        }

        public static Exception NotEnoughBytes(uint actual, long expected)
        {
            return new SerializationException($"Expected {expected} bytes, got {actual} bytes.");
        }

        public static Exception CantReadReadOnlyCollection(Type type)
        {
            return new SerializationException($"Can't deserialize into read-only collection {type.Name}. Create a specialized converter for that.");
        }

        public static Exception NoConverterForCollectionElement(Type type, string elementName)
        {
            return new SerializationException($"Provide converter for {elementName}: {type.Name}");
        }

        public static Exception IntDeserializationFailure(DataTypes type)
        {
            return new SerializationException($"Waited for an int, got {type:G} (0x{type:X})");
        }

        public static Exception IntSerializationFailure(long value)
        {
            return new SerializationException($"Can't serialize {value}");
        }

        public static Exception IntSerializationFailure(ulong value)
        {
            return new SerializationException($"Can't serialize {value}");
        }

        public static Exception ConverterNotFound(Type type)
        {
            return new ConverterNotFoundException(type);
        }

        public static InvalidOperationException EnumExpected(TypeInfo type)
        {
            return new InvalidOperationException($"Enum expected, but got {type}.");
        }

        public static InvalidOperationException UnexpectedEnumUnderlyingType(Type enumUnderlyingType)
        {
            return new InvalidOperationException($"Unexpected underlying enum type: {enumUnderlyingType}.");
        }

        public static Exception DuplicateArrayElement(Type typeToWrap, KeyValuePair<int, PropertyInfo[]> pair)
        {
            return new DuplicateArrayElementException(typeToWrap, pair.Key, pair.Value);
        }

        public static Exception DuplicateMapElement(Type typeToWrap, KeyValuePair<string, PropertyInfo[]> pair)
        {
            return new DuplicateMapElementException(typeToWrap, pair.Key, pair.Value);
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
