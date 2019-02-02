using System.Collections.Generic;

using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests.Writer
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

            using (var blob = MsgPackSerializer.Serialize(test, out var wroteSize))
                blob.Memory.Slice(0, wroteSize).ShouldBe(bytes);
        }
    }
}
