using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests.Writer
{
    public class Boolean
    {
        [Fact]
        public void False()
        {
            MsgPackSerializer.Serialize(false).ShouldBe(new[] {(byte) DataTypeInternal.False});
        }

        [Fact]
        public void True()
        {
            MsgPackSerializer.Serialize(true).ShouldBe(new[] {(byte) DataTypeInternal.True});
        }
    }
}
