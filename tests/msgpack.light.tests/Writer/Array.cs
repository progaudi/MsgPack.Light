using System.Collections.Generic;

using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests.Writer
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

            MsgPackSerializer.Serialize(tests).ShouldBe(bytes);
        }

        [Fact]
        public void TestArrayPack()
        {
            var tests = new object[]
            {
                0,
                50505,
                float.NaN,
                float.MaxValue,
                new[] {true, false, true},
                null,
                new Dictionary<string, string> {{"Ball", "Soccer"}}
            };

            var data = new byte[]
            {
                151,
                0,
                205, 197, 73,
                202, 255, 192, 0, 0,
                202, 127, 127, 255, 255,
                147,
                    195,
                    194,
                    195,
                192,
                129,
                    164, 66, 97, 108, 108, 166, 83, 111, 99, 99, 101, 114
            };

            var settings = new MsgPackContext();
            settings.RegisterConverter(new TestReflectionTokenConverter());
            MsgPackSerializer.Serialize(tests, settings).ShouldBe(data);
        }
    }
}
