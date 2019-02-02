using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests.Reader
{
    public class Array
    {
        [Fact]
        public void SimpleArray()
        {
            var tests = new[]
            {
                "a",
                "b",
                "c",
                "d",
                "e"
            };

            var bytes = new byte[]
            {
                149,
                161, 97,
                161, 98,
                161, 99,
                161, 100,
                161, 101
            };

            MsgPackSerializer.Deserialize<string[]>(bytes, out var readSize).ShouldBe(tests);
            readSize.ShouldBe(bytes.Length);
        }

        [Fact]
        public void TestNonFixedArray()
        {
            var array = new[]
            {
                1, 2, 3, 4, 5,
                1, 2, 3, 4, 5,
                1, 2, 3, 4, 5,
                1, 2, 3, 4, 5
            };

            var bytes = new byte[]
            {
                0xdc,
                0x00,
                0x14,

                0x01, 0x02, 0x03, 0x04, 0x05,
                0x01, 0x02, 0x03, 0x04, 0x05,
                0x01, 0x02, 0x03, 0x04, 0x05,
                0x01, 0x02, 0x03, 0x04, 0x05
            };

            MsgPackSerializer.Deserialize<int[]>(bytes, out int readSize).ShouldBe(array);
            readSize.ShouldBe(bytes.Length);
        }
    }
}
