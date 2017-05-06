using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests.Reader
{
    public class Enums
    {
        [Theory, ClassData(typeof(EnumValuesProvider<DefaultEnum>))]
        public void ReadEnum(DefaultEnum enumValue)
        {
            var bytes = MsgPackSerializer.Serialize((int)enumValue);
            var enumResult = MsgPackSerializer.Deserialize<DefaultEnum>(bytes);

            enumResult.ShouldBe(enumValue);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<DefaultEnum>))]
        public void ReadEnumAsString(DefaultEnum enumValue)
        {
            var stringConverterContext = new MsgPackContext(convertEnumsAsStrings: true);

            var bytes = MsgPackSerializer.Serialize(enumValue.ToString(), stringConverterContext);
            var enumResult = MsgPackSerializer.Deserialize<DefaultEnum>(bytes, stringConverterContext);

            enumResult.ShouldBe(enumValue);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<FlagEnum>))]
        public void ReadFlagEnum(FlagEnum enumValue)
        {
            var bytes = MsgPackSerializer.Serialize((int)enumValue);
            var enumResult = MsgPackSerializer.Deserialize<FlagEnum>(bytes);

            enumResult.ShouldBe(enumValue);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<FlagEnum>))]
        public void ReadFlagEnumAsString(FlagEnum enumValue)
        {
            var stringConverterContext = new MsgPackContext(convertEnumsAsStrings: true);

            var bytes = MsgPackSerializer.Serialize(enumValue.ToString(), stringConverterContext);
            var enumResult = MsgPackSerializer.Deserialize<FlagEnum>(bytes, stringConverterContext);

            enumResult.ShouldBe(enumValue);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<SbyteEnum>))]
        public void ReadSbyteEnum(SbyteEnum enumValue)
        {
            var bytes = MsgPackSerializer.Serialize((sbyte)enumValue);
            var enumResult = MsgPackSerializer.Deserialize<SbyteEnum>(bytes);
            enumResult.ShouldBe(enumValue);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<ByteEnum>))]
        public void ReadByteEnum(ByteEnum enumValue)
        {
            var bytes = MsgPackSerializer.Serialize((byte)enumValue);
            var enumResult = MsgPackSerializer.Deserialize<ByteEnum>(bytes);
            enumResult.ShouldBe(enumValue);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<ShortEnum>))]
        public void ReadShortEnum(ShortEnum enumValue)
        {
            var bytes = MsgPackSerializer.Serialize((short)enumValue);
            var enumResult = MsgPackSerializer.Deserialize<ShortEnum>(bytes);
            enumResult.ShouldBe(enumValue);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<UshortEnum>))]
        public void ReadUshortEnum(UshortEnum enumValue)
        {
            var bytes = MsgPackSerializer.Serialize((ushort)enumValue);
            var enumResult = MsgPackSerializer.Deserialize<UshortEnum>(bytes);
            enumResult.ShouldBe(enumValue);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<IntEnum>))]
        public void ReadIntEnum(IntEnum enumValue)
        {
            var bytes = MsgPackSerializer.Serialize((int)enumValue);
            var enumResult = MsgPackSerializer.Deserialize<IntEnum>(bytes);
            enumResult.ShouldBe(enumValue);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<UintEnum>))]
        public void ReadUintEnum(UintEnum enumValue)
        {
            var bytes = MsgPackSerializer.Serialize((uint)enumValue);
            var enumResult = MsgPackSerializer.Deserialize<UintEnum>(bytes);
            enumResult.ShouldBe(enumValue);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<LongEnum>))]
        public void ReadLongEnum(LongEnum enumValue)
        {
            var bytes = MsgPackSerializer.Serialize((long)enumValue);
            var enumResult = MsgPackSerializer.Deserialize<LongEnum>(bytes);
            enumResult.ShouldBe(enumValue);
        }

        [Theory, ClassData(typeof(EnumValuesProvider<UlongEnum>))]
        public void ReadUlongEnum(UlongEnum enumValue)
        {
            var bytes = MsgPackSerializer.Serialize((ulong)enumValue);
            var enumResult = MsgPackSerializer.Deserialize<UlongEnum>(bytes);
            enumResult.ShouldBe(enumValue);
        }
    }
}