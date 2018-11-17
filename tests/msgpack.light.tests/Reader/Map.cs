using System.Collections.Generic;

using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests.Reader
{
    public class Map
    {
        [Fact]
        public void SimpleDictionary()
        {
            var test = new Dictionary<int, string>
            {
                {1, "a"},
                {2, "b"},
                {3, "c"},
                {4, "d"},
                {5, "e"}
            };

            var bytes = new byte[]
            {
                133,
                1, 161, 97,
                2, 161, 98,
                3, 161, 99,
                4, 161, 100,
                5, 161, 101
            };

            MsgPackSerializer.Deserialize<Dictionary<int, string>>(bytes, out var readSize).ShouldBe(test);
            readSize.ShouldBe(bytes.Length);
        }

        [Fact]
        public void NonFixedDictionary()
        {
            var bytes = new byte[]
            {
                0xde,
                0x00,
                0x14,

                0x01, 0xa1, 0x61,
                0x02, 0xa1, 0x62,
                0x03, 0xa1, 0x63,
                0x04, 0xa1, 0x64,
                0x05, 0xa1, 0x65,

                0x0b, 0xa1, 0x61,
                0x0c, 0xa1, 0x62,
                0x0d, 0xa1, 0x63,
                0x0e, 0xa1, 0x64,
                0x0f, 0xa1, 0x65,

                0x15, 0xa1, 0x61,
                0x16, 0xa1, 0x62,
                0x17, 0xa1, 0x63,
                0x18, 0xa1, 0x64,
                0x19, 0xa1, 0x65,

                0x1f, 0xa1, 0x61,
                0x20, 0xa1, 0x62,
                0x21, 0xa1, 0x63,
                0x22, 0xa1, 0x64,
                0x23, 0xa1, 0x65
            };

            var test = new Dictionary<int, string>
            {
                {1, "a"},
                {2, "b"},
                {3, "c"},
                {4, "d"},
                {5, "e"},

                {11, "a"},
                {12, "b"},
                {13, "c"},
                {14, "d"},
                {15, "e"},

                {21, "a"},
                {22, "b"},
                {23, "c"},
                {24, "d"},
                {25, "e"},

                {31, "a"},
                {32, "b"},
                {33, "c"},
                {34, "d"},
                {35, "e"}
            };

            MsgPackSerializer.Deserialize<Dictionary<int, string>>(bytes, out var readSize).ShouldBe(test);
            readSize.ShouldBe(bytes.Length);
        }
    }
}
