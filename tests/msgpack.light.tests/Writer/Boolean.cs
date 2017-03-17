using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests.Writer
{
    public class Boolean
    {
        [Theory]
        [InlineData(true, new byte[] { (byte)DataTypes.True })]
        [InlineData(false, new byte[] { (byte)DataTypes.False })]
        public void Test(bool value, byte[] data)
        {
            MsgPackSerializer.Serialize(value).ShouldBe(data);

            ((MsgPackToken)value).RawBytes.ShouldBe(data);
        }
    }
}
