using System.Collections.Generic;

using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests.Writer
{
    public class Null
    {
        [Fact]
        public void WriteNullArray()
        {
            using (var blob = MsgPackSerializer.Serialize((int[]) null, out var wroteSize))
                blob.Memory.Slice(0, wroteSize).ShouldBe(new[] { DataCodes.Nil });
        }

        [Fact]
        public void WriteNullByteArray()
        {
            using (var blob = MsgPackSerializer.Serialize((byte[]) null, out var wroteSize))
                blob.Memory.Slice(0, wroteSize).ShouldBe(new[] { DataCodes.Nil });
        }

        [Fact]
        public void WriteNullMap()
        {
            using (var blob = MsgPackSerializer.Serialize((IDictionary<int, int>) null, out var wroteSize))
                blob.Memory.Slice(0, wroteSize).ShouldBe(new[] { DataCodes.Nil });
        }

        [Fact]
        public void WriteNullString()
        {
            using (var blob = MsgPackSerializer.Serialize((string) null, out var wroteSize))
                blob.Memory.Slice(0, wroteSize).ShouldBe(new[] { DataCodes.Nil });
        }
    }
}
