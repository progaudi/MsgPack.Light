using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests.Writer
{
    public class Boolean
    {
        [Theory]
        [InlineData(true, new[] { DataCodes.True })]
        [InlineData(false, new[] { DataCodes.False })]
        public void Test(bool value, byte[] data)
        {
            using (var blob = MsgPackSerializer.Serialize(value, out var wroteSize))
                blob.Memory.Slice(0, wroteSize).ShouldBe(data);
        }
    }
}
