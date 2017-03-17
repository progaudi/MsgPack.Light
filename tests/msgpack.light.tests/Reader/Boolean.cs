using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests.Reader
{
    public class Boolean
    {
        [Theory]
        [InlineData(true, new byte[] { (byte)DataTypes.True })]
        [InlineData(false, new byte[] { (byte)DataTypes.False })]
        public void Test(bool value, byte[] data)
        {
            MsgPackSerializer.Deserialize<bool>(data).ShouldBe(value);

            var token = Helpers.CheckTokenDeserialization(data);
            ((bool)token).ShouldBe(value);
        }
    }
}
