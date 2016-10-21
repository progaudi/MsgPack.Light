using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests.Reader
{
    public class Boolean
    {
        [Fact]
        public void False()
        {
            MsgPackSerializer.Deserialize<bool>(new[] {(byte) DataTypes.False}).ShouldBeFalse();
        }

        [Fact]
        public void True()
        {
            MsgPackSerializer.Deserialize<bool>(new[] {(byte) DataTypes.True}).ShouldBeTrue();
        }
    }
}
