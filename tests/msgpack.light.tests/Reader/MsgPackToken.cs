﻿using System.Linq;

using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests.Reader
{
    public class MsgPackTokenTest
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

            var token = MsgPackSerializer.Deserialize<MsgPackToken>(bytes);
            token.RawBytes.ShouldBe(bytes);
            ((MsgPackToken[])token).Select(t => (string)t).ToArray().ShouldBe(tests);
        }

        [Fact]
        public void TestNonFixedArray()
        {
            var array = new[]
               {
                    1, 2, 3, 4, 5,
                    1, 2, 3, 4, 5,
                    1, 2, 3, 4, 5,
                    1, 2, 3, 4, 5,
                };

            var bytes = new byte[]
            {
                0xdc,
                0x00,
                0x14,

                0x01, 0x02, 0x03, 0x04, 0x05,
                0x01, 0x02, 0x03, 0x04, 0x05,
                0x01, 0x02, 0x03, 0x04, 0x05,
                0x01, 0x02, 0x03, 0x04, 0x05,
            };

            var token = MsgPackSerializer.Deserialize<MsgPackToken>(bytes);
            token.RawBytes.ShouldBe(bytes);
            ((MsgPackToken[])token).Select(t => (int)t).ToArray().ShouldBe(array);
        }
    }
}