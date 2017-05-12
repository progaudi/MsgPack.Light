using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests.Writer
{
    public class Enums
    {
        [Theory, ClassData(typeof(EnumValuesProvider<DefaultEnum>))]
        public void WriteEnum(DefaultEnum enumValue)
        {
            var intEnumContext = new MsgPackContext(convertEnumsAsStrings: false);
            var enumResult = MsgPackSerializer.Serialize(enumValue, intEnumContext);
            var valueResult = MsgPackSerializer.Serialize((int)enumValue, intEnumContext);

            enumResult.ShouldBe(valueResult);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<DefaultEnum>))]
        public void WriteEnumAsString(DefaultEnum enumValue)
        {
            var enumAsStringResult = MsgPackSerializer.Serialize(enumValue);
            var enumAsStringExpected = MsgPackSerializer.Serialize(enumValue.ToString());

            enumAsStringResult.ShouldBe(enumAsStringExpected);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<FlagEnum>))]
        public void WriteFlagEnum(FlagEnum enumValue)
        {
            var intEnumContext = new MsgPackContext(convertEnumsAsStrings: false);
            var enumResult = MsgPackSerializer.Serialize(enumValue, intEnumContext);
            var valueResult = MsgPackSerializer.Serialize((int)enumValue, intEnumContext);

            enumResult.ShouldBe(valueResult);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<FlagEnum>))]
        public void WriteFlagEnumAsString(FlagEnum enumValue)
        {
            var enumAsStringResult = MsgPackSerializer.Serialize(enumValue);
            var enumAsStringExpected = MsgPackSerializer.Serialize(enumValue.ToString());

            enumAsStringResult.ShouldBe(enumAsStringExpected);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<SbyteEnum>))]
        public void WriteSbyteEnum(SbyteEnum enumValue)
        {
            var intEnumContext = new MsgPackContext(convertEnumsAsStrings: false);

            var enumResult = MsgPackSerializer.Serialize(enumValue, intEnumContext);
            var valueResult = MsgPackSerializer.Serialize((sbyte)enumValue, intEnumContext);

            enumResult.ShouldBe(valueResult);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<ByteEnum>))]
        public void WriteByteEnum(ByteEnum enumValue)
        {
            var intEnumContext = new MsgPackContext(convertEnumsAsStrings: false);
            var enumResult = MsgPackSerializer.Serialize(enumValue, intEnumContext);
            var valueResult = MsgPackSerializer.Serialize((byte)enumValue, intEnumContext);

            enumResult.ShouldBe(valueResult);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<ShortEnum>))]
        public void WriteShortEnum(ShortEnum enumValue)
        {
            var intEnumContext = new MsgPackContext(convertEnumsAsStrings: false);
            var enumResult = MsgPackSerializer.Serialize(enumValue, intEnumContext);
            var valueResult = MsgPackSerializer.Serialize((short)enumValue, intEnumContext);

            enumResult.ShouldBe(valueResult);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<UshortEnum>))]
        public void WriteUshortEnum(UshortEnum enumValue)
        {
            var intEnumContext = new MsgPackContext(convertEnumsAsStrings: false);
            var enumResult = MsgPackSerializer.Serialize(enumValue, intEnumContext);
            var valueResult = MsgPackSerializer.Serialize((ushort)enumValue, intEnumContext);

            enumResult.ShouldBe(valueResult);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<IntEnum>))]
        public void WriteIntEnum(IntEnum enumValue)
        {
            var intEnumContext = new MsgPackContext(convertEnumsAsStrings: false);
            var enumResult = MsgPackSerializer.Serialize(enumValue, intEnumContext);
            var valueResult = MsgPackSerializer.Serialize((int)enumValue, intEnumContext);

            enumResult.ShouldBe(valueResult);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<UintEnum>))]
        public void WriteUintEnum(UintEnum enumValue)
        {
            var intEnumContext = new MsgPackContext(convertEnumsAsStrings: false);
            var enumResult = MsgPackSerializer.Serialize(enumValue, intEnumContext);
            var valueResult = MsgPackSerializer.Serialize((uint)enumValue, intEnumContext);

            enumResult.ShouldBe(valueResult);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<LongEnum>))]
        public void WriteLongEnum(LongEnum enumValue)
        {
            var intEnumContext = new MsgPackContext(convertEnumsAsStrings: false);
            var enumResult = MsgPackSerializer.Serialize(enumValue, intEnumContext);
            var valueResult = MsgPackSerializer.Serialize((long)enumValue, intEnumContext);

            enumResult.ShouldBe(valueResult);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<UlongEnum>))]
        public void WriteUlongEnum(UlongEnum enumValue)
        {
            var intEnumContext = new MsgPackContext(convertEnumsAsStrings: false);
            var enumResult = MsgPackSerializer.Serialize(enumValue, intEnumContext);
            var valueResult = MsgPackSerializer.Serialize((ulong)enumValue, intEnumContext);

            enumResult.ShouldBe(valueResult);
        }
    }
}