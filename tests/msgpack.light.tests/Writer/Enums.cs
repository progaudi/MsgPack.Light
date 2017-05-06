// <copyright file="Enums.cs" company="eVote">
//   Copyright © eVote
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests.Writer
{
    public class Enums
    {
        [Theory, ClassData(typeof(EnumValuesProvider<DefaultEnum>))]
        public void WriteEnum(DefaultEnum enumValue)
        {
            var enumResult = MsgPackSerializer.Serialize(enumValue);
            var valueResult = MsgPackSerializer.Serialize((int)enumValue);

            enumResult.ShouldBe(valueResult);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<DefaultEnum>))]
        public void WriteEnumAsString(DefaultEnum enumValue)
        {
            var stringConverterContext = new MsgPackContext(convertEnumsAsStrings: true);
            var enumAsStringResult = MsgPackSerializer.Serialize(enumValue, stringConverterContext);
            var enumAsStringExpected = MsgPackSerializer.Serialize(enumValue.ToString(), stringConverterContext);

            enumAsStringResult.ShouldBe(enumAsStringExpected);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<FlagEnum>))]
        public void WriteFlagEnum(FlagEnum enumValue)
        {
            var enumResult = MsgPackSerializer.Serialize(enumValue);
            var valueResult = MsgPackSerializer.Serialize((int)enumValue);

            enumResult.ShouldBe(valueResult);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<FlagEnum>))]
        public void WriteFlagEnumAsString(FlagEnum enumValue)
        {
            var stringConverterContext = new MsgPackContext(convertEnumsAsStrings: true);
            var enumAsStringResult = MsgPackSerializer.Serialize(enumValue, stringConverterContext);
            var enumAsStringExpected = MsgPackSerializer.Serialize(enumValue.ToString(), stringConverterContext);

            enumAsStringResult.ShouldBe(enumAsStringExpected);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<SbyteEnum>))]
        public void WriteSbyteEnum(SbyteEnum enumValue)
        {
            var enumResult = MsgPackSerializer.Serialize(enumValue);
            var valueResult = MsgPackSerializer.Serialize((sbyte)enumValue);

            enumResult.ShouldBe(valueResult);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<ByteEnum>))]
        public void WriteByteEnum(ByteEnum enumValue)
        {
            var enumResult = MsgPackSerializer.Serialize(enumValue);
            var valueResult = MsgPackSerializer.Serialize((byte)enumValue);

            enumResult.ShouldBe(valueResult);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<ShortEnum>))]
        public void WriteShortEnum(ShortEnum enumValue)
        {
            var enumResult = MsgPackSerializer.Serialize(enumValue);
            var valueResult = MsgPackSerializer.Serialize((short)enumValue);

            enumResult.ShouldBe(valueResult);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<UshortEnum>))]
        public void WriteUshortEnum(UshortEnum enumValue)
        {
            var enumResult = MsgPackSerializer.Serialize(enumValue);
            var valueResult = MsgPackSerializer.Serialize((ushort)enumValue);

            enumResult.ShouldBe(valueResult);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<IntEnum>))]
        public void WriteIntEnum(IntEnum enumValue)
        {
            var enumResult = MsgPackSerializer.Serialize(enumValue);
            var valueResult = MsgPackSerializer.Serialize((int)enumValue);

            enumResult.ShouldBe(valueResult);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<UintEnum>))]
        public void WriteUintEnum(UintEnum enumValue)
        {
            var enumResult = MsgPackSerializer.Serialize(enumValue);
            var valueResult = MsgPackSerializer.Serialize((uint)enumValue);

            enumResult.ShouldBe(valueResult);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<LongEnum>))]
        public void WriteLongEnum(LongEnum enumValue)
        {
            var enumResult = MsgPackSerializer.Serialize(enumValue);
            var valueResult = MsgPackSerializer.Serialize((long)enumValue);

            enumResult.ShouldBe(valueResult);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<UlongEnum>))]
        public void WriteUlongEnum(UlongEnum enumValue)
        {
            var enumResult = MsgPackSerializer.Serialize(enumValue);
            var valueResult = MsgPackSerializer.Serialize((ulong)enumValue);

            enumResult.ShouldBe(valueResult);
        }

        public class EnumValuesProvider<T> : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                return Enum
                    .GetValues(typeof(T))
                    .Cast<T>()
                    .Select(v => new object[] {v})
                    .GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }

    public enum DefaultEnum
    {
        TestValue1,

        TestValue2,

        TestValue3 = 3
    }

    [Flags]
    public enum FlagEnum
    {
        Value1,

        Value2,

        Value3 = Value1 | Value2,

        Value4 = Value1 & Value2
    }

    public enum SbyteEnum : sbyte
    {
        Value1,
        Value2,
        Value3 = 3,
        Value4 = sbyte.MaxValue,
        Value5 = sbyte.MinValue,
    }

    public enum ByteEnum : byte
    {
        Value1,
        Value2,
        Value3 = 3,
        Value4 = byte.MaxValue,
        Value5 = byte.MinValue,
    }

    public enum ShortEnum : short
    {
        Value1,
        Value2,
        Value3 = 3,
        Value4 = short.MaxValue,
        Value5 = short.MinValue,
    }

    public enum UshortEnum : ushort
    {
        Value1,
        Value2,
        Value3 = 3,
        Value4 = ushort.MaxValue,
        Value5 = ushort.MinValue,
    }

    public enum IntEnum : int
    {
        Value1,
        Value2,
        Value3 = 3,
        Value4 = int.MaxValue,
        Value5 = int.MinValue,
    }

    public enum UintEnum : uint
    {
        Value1,
        Value2,
        Value3 = 3,
        Value4 = uint.MaxValue,
        Value5 = uint.MinValue,
    }

    public enum LongEnum : long
    {
        Value1,
        Value2,
        Value3 = 3,
        Value4 = long.MaxValue,
        Value5 = long.MinValue,
    }

    public enum UlongEnum : ulong
    {
        Value1,
        Value2,
        Value3 = 3,
        Value4 = ulong.MaxValue,
        Value5 = ulong.MinValue,
    }
}