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
            MsgPackSerializer.Serialize((int[]) null).ShouldBe(new[] {(byte) DataTypes.Null});
        }

        [Fact]
        public void WriteNullByteArray()
        {
            MsgPackSerializer.Serialize((byte[]) null).ShouldBe(new[] {(byte) DataTypes.Null});
            ((MsgPackToken)(byte[])null).RawBytes.ShouldBe(new[] { (byte)DataTypes.Null });
        }

        [Fact]
        public void WriteNullMap()
        {
            MsgPackSerializer.Serialize((IDictionary<int, int>) null).ShouldBe(new[] {(byte) DataTypes.Null});
        }

        [Fact]
        public void WriteNullString()
        {
            MsgPackSerializer.Serialize((string) null).ShouldBe(new[] {(byte) DataTypes.Null});
            ((MsgPackToken)(string)null).RawBytes.ShouldBe(new[] { (byte)DataTypes.Null });
        }
    }
}
