using System;
using System.Runtime.Serialization;

using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests.Reader
{
    public class FloatingPoint
    {
        [Theory]
        [InlineData(0, new byte[] {203, 0, 0, 0, 0, 0, 0, 0, 0})]
        [InlineData(1, new byte[] {203, 63, 240, 0, 0, 0, 0, 0, 0})]
        [InlineData(-1, new byte[] {203, 191, 240, 0, 0, 0, 0, 0, 0})]
        [InlineData(Math.E, new byte[] {203, 64, 5, 191, 10, 139, 20, 87, 105})]
        [InlineData(Math.PI, new byte[] {203, 64, 9, 33, 251, 84, 68, 45, 24})]
        [InlineData(224, new byte[] {203, 64, 108, 0, 0, 0, 0, 0, 0})]
        [InlineData(256, new byte[] {203, 64, 112, 0, 0, 0, 0, 0, 0})]
        [InlineData(65530, new byte[] {203, 64, 239, 255, 64, 0, 0, 0, 0})]
        [InlineData(65540, new byte[] {203, 64, 240, 0, 64, 0, 0, 0, 0})]
        [InlineData(double.NaN, new byte[] {203, 255, 248, 0, 0, 0, 0, 0, 0})]
        [InlineData(double.MaxValue, new byte[] {203, 127, 239, 255, 255, 255, 255, 255, 255})]
        [InlineData(double.MinValue, new byte[] {203, 255, 239, 255, 255, 255, 255, 255, 255})]
        [InlineData(double.PositiveInfinity, new byte[] {203, 127, 240, 0, 0, 0, 0, 0, 0})]
        [InlineData(double.NegativeInfinity, new byte[] {203, 255, 240, 0, 0, 0, 0, 0, 0})]
        // integers
        [InlineData(sbyte.MinValue, new byte[] { 208, 128 })]
        [InlineData(sbyte.MaxValue, new byte[] { 127 })]
        [InlineData(short.MinValue, new byte[] { 209, 128, 0 })]
        [InlineData(short.MaxValue, new byte[] { 209, 127, 0xff })]
        [InlineData(int.MinValue, new byte[] { 210, 128, 0, 0, 0 })]
        [InlineData(int.MaxValue, new byte[] { 210, 127, 0xff, 0xff, 0xff })]
        [InlineData(long.MinValue / 128, new byte[] { 211, 0xff, 0, 0, 0, 0, 0, 0, 0 })]
        [InlineData(long.MaxValue / 128, new byte[] { 207, 0, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff })]
        public void TestDouble(double value, byte[] bytes)
        {
            var d = MsgPackSerializer.Deserialize<double>(bytes);
            d.ShouldBe(value);

            var token = Helpers.CheckTokenDeserialization(bytes);
            ((double)token).ShouldBe(value);
        }

        [Theory]
        [InlineData(0, new byte[] {202, 0, 0, 0, 0})]
        [InlineData(1, new byte[] {202, 63, 128, 0, 0})]
        [InlineData(-1, new byte[] {202, 191, 128, 0, 0})]
        [InlineData(2.71828, new byte[] {202, 64, 45, 248, 77})]
        [InlineData(3.14159, new byte[] {202, 64, 73, 15, 208})]
        [InlineData(224, new byte[] {202, 67, 96, 0, 0})]
        [InlineData(256, new byte[] {202, 67, 128, 0, 0})]
        [InlineData(65530, new byte[] {202, 71, 127, 250, 0})]
        [InlineData(65540, new byte[] {202, 71, 128, 2, 0})]
        [InlineData(float.NaN, new byte[] {202, 255, 192, 0, 0})]
        [InlineData(float.MaxValue, new byte[] {202, 127, 127, 255, 255})]
        [InlineData(float.MinValue, new byte[] {202, 255, 127, 255, 255})]
        [InlineData(float.PositiveInfinity, new byte[] {202, 127, 128, 0, 0})]
        [InlineData(float.NegativeInfinity, new byte[] {202, 255, 128, 0, 0})]
        // integers
        [InlineData(sbyte.MinValue, new byte[] { 208, 128 })]
        [InlineData(sbyte.MaxValue, new byte[] { 127 })]
        [InlineData(short.MinValue, new byte[] { 209, 128, 0 })]
        [InlineData(short.MaxValue, new byte[] { 209, 127, 0xff })]
        [InlineData(int.MinValue / 128, new byte[] { 210, 0xff, 0, 0, 0 })]
        [InlineData(int.MaxValue / 128, new byte[] { 206, 0, 0xff, 0xff, 0xff })]
        public void TestFloat(float value, byte[] bytes)
        {
            MsgPackSerializer.Deserialize<float>(bytes).ShouldBe(value);

            var token = Helpers.CheckTokenDeserialization(bytes);
            ((float)token).ShouldBe(value);
        }

        [Theory]
        [InlineData(new byte[] { 208, 128 })]
        [InlineData(new byte[] { 127 })]
        [InlineData(new byte[] { 209, 128, 0 })]
        [InlineData(new byte[] { 209, 127, 0xff })]
        [InlineData(new byte[] { 210, 128, 0, 0, 0 })]
        [InlineData(new byte[] { 210, 127, 0xff, 0xff, 0xff })]
        [InlineData(new byte[] { 211, 0xff, 0, 0, 0, 0, 0, 0, 0 })]
        [InlineData(new byte[] { 207, 0, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff })]
        public void TestDoubleStritctParsing(byte[] bytes)
        {
            var e = Should.Throw<SerializationException>(() => MsgPackSerializer.Deserialize<double>(bytes, new MsgPackContext(true)));
            e.Message.ShouldBe(ExceptionUtils.BadTypeException((DataTypes)bytes[0], DataTypes.Single, DataTypes.Double).Message);
        }

        [Theory]
        [InlineData(new byte[] { 208, 128 })]
        [InlineData(new byte[] { 127 })]
        [InlineData(new byte[] { 209, 128, 0 })]
        [InlineData(new byte[] { 209, 127, 0xff })]
        [InlineData(new byte[] { 210, 0xff, 0, 0, 0 })]
        [InlineData(new byte[] { 206, 0, 0xff, 0xff, 0xff })]
        public void TestFloatStritctParsing(byte[] bytes)
        {
            var e = Should.Throw<SerializationException>(() => MsgPackSerializer.Deserialize<float>(bytes, new MsgPackContext(true)));
            e.Message.ShouldBe(ExceptionUtils.BadTypeException((DataTypes)bytes[0], DataTypes.Single).Message);
        }
    }
}
