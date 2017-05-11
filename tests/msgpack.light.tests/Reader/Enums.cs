using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests.Reader
{
    public class Enums
    {
        [Theory, ClassData(typeof(EnumValuesProvider<DefaultEnum>))]
        public void ReadEnum(DefaultEnum enumValue)
        {
            var intEnumContext = new MsgPackContext(convertEnumsAsStrings: false);
            var bytes = MsgPackSerializer.Serialize((int)enumValue, intEnumContext);
            var enumResult = MsgPackSerializer.Deserialize<DefaultEnum>(bytes, intEnumContext);

            enumResult.ShouldBe(enumValue);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<DefaultEnum>))]
        public void ReadEnumAsString(DefaultEnum enumValue)
        {
            var bytes = MsgPackSerializer.Serialize(enumValue.ToString());
            var enumResult = MsgPackSerializer.Deserialize<DefaultEnum>(bytes);

            enumResult.ShouldBe(enumValue);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<FlagEnum>))]
        public void ReadFlagEnum(FlagEnum enumValue)
        {
            var intEnumContext = new MsgPackContext(convertEnumsAsStrings: false);
            var bytes = MsgPackSerializer.Serialize((int)enumValue, intEnumContext);
            var enumResult = MsgPackSerializer.Deserialize<FlagEnum>(bytes, intEnumContext);

            enumResult.ShouldBe(enumValue);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<FlagEnum>))]
        public void ReadFlagEnumAsString(FlagEnum enumValue)
        {
            var bytes = MsgPackSerializer.Serialize(enumValue.ToString());
            var enumResult = MsgPackSerializer.Deserialize<FlagEnum>(bytes);

            enumResult.ShouldBe(enumValue);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<SbyteEnum>))]
        public void ReadSbyteEnum(SbyteEnum enumValue)
        {
            var intEnumContext = new MsgPackContext(convertEnumsAsStrings: false);
            var bytes = MsgPackSerializer.Serialize((sbyte)enumValue, intEnumContext);
            var enumResult = MsgPackSerializer.Deserialize<SbyteEnum>(bytes, intEnumContext);
            enumResult.ShouldBe(enumValue);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<ByteEnum>))]
        public void ReadByteEnum(ByteEnum enumValue)
        {
            var intEnumContext = new MsgPackContext(convertEnumsAsStrings: false);
            var bytes = MsgPackSerializer.Serialize((byte)enumValue, intEnumContext);
            var enumResult = MsgPackSerializer.Deserialize<ByteEnum>(bytes, intEnumContext);
            enumResult.ShouldBe(enumValue);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<ShortEnum>))]
        public void ReadShortEnum(ShortEnum enumValue)
        {
            var intEnumContext = new MsgPackContext(convertEnumsAsStrings: false);
            var bytes = MsgPackSerializer.Serialize((short)enumValue, intEnumContext);
            var enumResult = MsgPackSerializer.Deserialize<ShortEnum>(bytes, intEnumContext);
            enumResult.ShouldBe(enumValue);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<UshortEnum>))]
        public void ReadUshortEnum(UshortEnum enumValue)
        {
            var intEnumContext = new MsgPackContext(convertEnumsAsStrings: false);
            var bytes = MsgPackSerializer.Serialize((ushort)enumValue, intEnumContext);
            var enumResult = MsgPackSerializer.Deserialize<UshortEnum>(bytes, intEnumContext);
            enumResult.ShouldBe(enumValue);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<IntEnum>))]
        public void ReadIntEnum(IntEnum enumValue)
        {
            var intEnumContext = new MsgPackContext(convertEnumsAsStrings: false);
            var bytes = MsgPackSerializer.Serialize((int)enumValue, intEnumContext);
            var enumResult = MsgPackSerializer.Deserialize<IntEnum>(bytes, intEnumContext);
            enumResult.ShouldBe(enumValue);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<UintEnum>))]
        public void ReadUintEnum(UintEnum enumValue)
        {
            var intEnumContext = new MsgPackContext(convertEnumsAsStrings: false);
            var bytes = MsgPackSerializer.Serialize((uint)enumValue, intEnumContext);
            var enumResult = MsgPackSerializer.Deserialize<UintEnum>(bytes, intEnumContext);
            enumResult.ShouldBe(enumValue);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<LongEnum>))]
        public void ReadLongEnum(LongEnum enumValue)
        {
            var intEnumContext = new MsgPackContext(convertEnumsAsStrings: false);
            var bytes = MsgPackSerializer.Serialize((long)enumValue, intEnumContext);
            var enumResult = MsgPackSerializer.Deserialize<LongEnum>(bytes, intEnumContext);
            enumResult.ShouldBe(enumValue);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<UlongEnum>))]
        public void ReadUlongEnum(UlongEnum enumValue)
        {
            var intEnumContext = new MsgPackContext(convertEnumsAsStrings: false);
            var bytes = MsgPackSerializer.Serialize((ulong)enumValue, intEnumContext);
            var enumResult = MsgPackSerializer.Deserialize<UlongEnum>(bytes, intEnumContext);
            enumResult.ShouldBe(enumValue);
        }
    }
}