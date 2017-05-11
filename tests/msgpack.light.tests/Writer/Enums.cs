// <copyright file="Enums.cs" company="eVote">
//   Copyright © eVote
// </copyright>



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
    }
}