using System.Collections.Generic;

using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests.Reader
{
    public class Null
    {
        [Fact]
        public void ReadNullArray()
        {
            MsgPackSerializer.Deserialize<int[]>(new[] { DataCodes.Nil }, out var readSize).ShouldBe(null);
            readSize.ShouldBe(1);
        }

        [Fact]
        public void ReadNullByteArray()
        {
            MsgPackSerializer.Deserialize<byte[]>(new[] { DataCodes.Nil }, out var readSize).ShouldBe(null);
            readSize.ShouldBe(1);
        }

        [Fact]
        public void ReadNullMap()
        {
            MsgPackSerializer.Deserialize<Dictionary<int, int>>(new[] { DataCodes.Nil }, out var readSize).ShouldBe(null);
            readSize.ShouldBe(1);
        }

        [Fact]
        public void ReadNullString()
        {
            MsgPackSerializer.Deserialize<string>(new[] { DataCodes.Nil }, out var readSize).ShouldBe(null);
            readSize.ShouldBe(1);
        }
    }
}
