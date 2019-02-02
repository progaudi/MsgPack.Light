using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests.SequenceReader
{
    public class Boolean
    {
        [Theory]
        [InlineData(true, new[] { DataCodes.True })]
        [InlineData(false, new[] { DataCodes.False })]
        public void Test(bool value, byte[] data)
        {
            MsgPackSerializer.Deserialize<bool>(data.ToMultipleSegments(), out var readSize).ShouldBe(value);
            readSize.ShouldBe(data.Length);
        }
    }
}
