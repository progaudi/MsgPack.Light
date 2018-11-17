using System;

using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests.Reader
{
    public class Nullable
    {
        [Fact]
        public void ReadNullAsNullableBool()
        {
            var data = new[] { DataCodes.Nil };
            var deserialize = MsgPackSerializer.Deserialize<bool?>(data, out var readSize);
            deserialize.ShouldBe(null);
            readSize.ShouldBe(1);
        }

        [Fact]
        public void ReadNullAsNullableFloat()
        {
            var data = new[] { DataCodes.Nil };
            MsgPackSerializer.Deserialize<float?>(data, out var readSize).ShouldBe(null);
            readSize.ShouldBe(1);
        }

        [Fact]
        public void ReadNullAsNullableDouble()
        {
            var data = new[] { DataCodes.Nil };
            MsgPackSerializer.Deserialize<double?>(data, out var readSize).ShouldBe(null);
            readSize.ShouldBe(1);
        }

        [Fact]
        public void ReadNullAsNullableByte()
        {
            var data = new[] { DataCodes.Nil };
            MsgPackSerializer.Deserialize<byte?>(data, out var readSize).ShouldBe(null);
            readSize.ShouldBe(1);
        }

        [Fact]
        public void ReadNullAsNullableSbyte()
        {
            var data = new[] { DataCodes.Nil };
            MsgPackSerializer.Deserialize<sbyte?>(data, out var readSize).ShouldBe(null);
            readSize.ShouldBe(1);
        }

        [Fact]
        public void ReadNullAsNullableShort()
        {
            var data = new[] { DataCodes.Nil };
            MsgPackSerializer.Deserialize<short?>(data, out var readSize).ShouldBe(null);
            readSize.ShouldBe(1);
        }

        [Fact]
        public void ReadNullAsNullableUshort()
        {
            var data = new[] { DataCodes.Nil };
            MsgPackSerializer.Deserialize<ushort?>(data, out var readSize).ShouldBe(null);
            readSize.ShouldBe(1);
        }

        [Fact]
        public void ReadNullAsNullableInt()
        {
            var data = new[] { DataCodes.Nil };
            MsgPackSerializer.Deserialize<int?>(data, out var readSize).ShouldBe(null);
            readSize.ShouldBe(1);
        }

        [Fact]
        public void ReadNullAsNullableUint()
        {
            var data = new[] { DataCodes.Nil };
            MsgPackSerializer.Deserialize<uint?>(data, out var readSize).ShouldBe(null);
            readSize.ShouldBe(1);
        }

        [Fact]
        public void ReadNullAsNullableLong()
        {
            var data = new[] { DataCodes.Nil };
            MsgPackSerializer.Deserialize<long?>(data, out var readSize).ShouldBe(null);
            readSize.ShouldBe(1);
        }

        [Fact]
        public void ReadNullAsNullableUlong()
        {
            var data = new[] { DataCodes.Nil };
            MsgPackSerializer.Deserialize<ulong?>(data, out var readSize).ShouldBe(null);
            readSize.ShouldBe(1);
        }

        [Fact]
        public void False()
        {
            var data = new[] { DataCodes.False };
            MsgPackSerializer.Deserialize<bool?>(data, out var readSize).ShouldBe(false);
            readSize.ShouldBe(1);
        }

        [Fact]
        public void True()
        {
            var data = new[] { DataCodes.True };
            MsgPackSerializer.Deserialize<bool?>(data, out var readSize).ShouldBe(true);
            readSize.ShouldBe(1);
        }

        [Theory]
        [InlineData(0, new byte[] { 203, 0, 0, 0, 0, 0, 0, 0, 0 })]
        [InlineData(1, new byte[] { 203, 63, 240, 0, 0, 0, 0, 0, 0 })]
        [InlineData(-1, new byte[] { 203, 191, 240, 0, 0, 0, 0, 0, 0 })]
        [InlineData(Math.E, new byte[] { 203, 64, 5, 191, 10, 139, 20, 87, 105 })]
        [InlineData(Math.PI, new byte[] { 203, 64, 9, 33, 251, 84, 68, 45, 24 })]
        [InlineData(224, new byte[] { 203, 64, 108, 0, 0, 0, 0, 0, 0 })]
        [InlineData(256, new byte[] { 203, 64, 112, 0, 0, 0, 0, 0, 0 })]
        [InlineData(65530, new byte[] { 203, 64, 239, 255, 64, 0, 0, 0, 0 })]
        [InlineData(65540, new byte[] { 203, 64, 240, 0, 64, 0, 0, 0, 0 })]
        [InlineData(double.NaN, new byte[] { 203, 255, 248, 0, 0, 0, 0, 0, 0 })]
        [InlineData(double.MaxValue, new byte[] { 203, 127, 239, 255, 255, 255, 255, 255, 255 })]
        [InlineData(double.MinValue, new byte[] { 203, 255, 239, 255, 255, 255, 255, 255, 255 })]
        [InlineData(double.PositiveInfinity, new byte[] { 203, 127, 240, 0, 0, 0, 0, 0, 0 })]
        [InlineData(double.NegativeInfinity, new byte[] { 203, 255, 240, 0, 0, 0, 0, 0, 0 })]
        public void TestDouble(double value, byte[] bytes)
        {
            MsgPackSerializer.Deserialize<double?>(bytes, out var readSize).ShouldBe(value);
            readSize.ShouldBe(bytes.Length);
        }

        [Theory]
        [InlineData(0, new byte[] { 202, 0, 0, 0, 0 })]
        [InlineData(1, new byte[] { 202, 63, 128, 0, 0 })]
        [InlineData(-1, new byte[] { 202, 191, 128, 0, 0 })]
        [InlineData(2.71828, new byte[] { 202, 64, 45, 248, 77 })]
        [InlineData(3.14159, new byte[] { 202, 64, 73, 15, 208 })]
        [InlineData(224, new byte[] { 202, 67, 96, 0, 0 })]
        [InlineData(256, new byte[] { 202, 67, 128, 0, 0 })]
        [InlineData(65530, new byte[] { 202, 71, 127, 250, 0 })]
        [InlineData(65540, new byte[] { 202, 71, 128, 2, 0 })]
        [InlineData(float.NaN, new byte[] { 202, 255, 192, 0, 0 })]
        [InlineData(float.MaxValue, new byte[] { 202, 127, 127, 255, 255 })]
        [InlineData(float.MinValue, new byte[] { 202, 255, 127, 255, 255 })]
        [InlineData(float.PositiveInfinity, new byte[] { 202, 127, 128, 0, 0 })]
        [InlineData(float.NegativeInfinity, new byte[] { 202, 255, 128, 0, 0 })]
        public void TestFloat(float value, byte[] bytes)
        {
            MsgPackSerializer.Deserialize<float?>(bytes, out var readSize).ShouldBe(value);
            readSize.ShouldBe(bytes.Length);
        }

        [Theory]
        [InlineData(0, new byte[] { 0x00 })]
        [InlineData(1, new byte[] { 1 })]
        [InlineData(-1, new byte[] { 0xff })]
        [InlineData(sbyte.MinValue, new byte[] { 208, 128 })]
        [InlineData(sbyte.MaxValue, new byte[] { 127 })]
        [InlineData(short.MinValue, new byte[] { 209, 128, 0 })]
        [InlineData(short.MaxValue, new byte[] { 209, 127, 0xff })]
        [InlineData(int.MinValue, new byte[] { 210, 128, 0, 0, 0 })]
        [InlineData(int.MaxValue, new byte[] { 210, 127, 0xff, 0xff, 0xff })]
        [InlineData(long.MaxValue, new byte[] { 211, 127, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff })]
        [InlineData(long.MinValue, new byte[] { 211, 128, 0, 0, 0, 0, 0, 0, 0 })]
        public void TestSignedLong(long number, byte[] data)
        {
            MsgPackSerializer.Deserialize<long?>(data, out var readSize).ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(0, new byte[] { 0x00 })]
        [InlineData(1, new byte[] { 1 })]
        [InlineData(-1, new byte[] { 0xff })]
        [InlineData(sbyte.MinValue, new byte[] { 208, 128 })]
        [InlineData(sbyte.MaxValue, new byte[] { 127 })]
        [InlineData(short.MinValue, new byte[] { 209, 128, 0 })]
        [InlineData(short.MaxValue, new byte[] { 209, 127, 0xff })]
        [InlineData(int.MinValue, new byte[] { 210, 128, 0, 0, 0 })]
        [InlineData(int.MaxValue, new byte[] { 210, 127, 0xff, 0xff, 0xff })]
        [InlineData(50505, new byte[] { 205, 197, 73 })]
        public void TestSignedInt(int number, byte[] data)
        {
            MsgPackSerializer.Deserialize<int?>(data, out var readSize).ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(0, new byte[] { 0x00 })]
        [InlineData(1, new byte[] { 1 })]
        [InlineData(-1, new byte[] { 0xff })]
        [InlineData(sbyte.MinValue, new byte[] { 208, 128 })]
        [InlineData(sbyte.MaxValue, new byte[] { 127 })]
        [InlineData(short.MinValue, new byte[] { 209, 128, 0 })]
        [InlineData(short.MaxValue, new byte[] { 209, 127, 0xff })]
        public void TestSignedShort(short number, byte[] data)
        {
            MsgPackSerializer.Deserialize<short?>(data, out var readSize).ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(0, new byte[] { 0x00 })]
        [InlineData(1, new byte[] { 1 })]
        [InlineData(-1, new byte[] { 0xff })]
        [InlineData(sbyte.MinValue, new byte[] { 208, 128 })]
        [InlineData(sbyte.MaxValue, new byte[] { 127 })]
        public void TestSignedByte(sbyte number, byte[] data)
        {
            MsgPackSerializer.Deserialize<sbyte?>(data, out var readSize).ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(0, new byte[] { 0x00 })]
        [InlineData(1, new byte[] { 1 })]
        [InlineData(byte.MaxValue, new byte[] { 0xcc, 0xff })]
        [InlineData(ushort.MaxValue, new byte[] { 0xcd, 0xff, 0xff })]
        [InlineData(uint.MaxValue, new byte[] { 0xce, 0xff, 0xff, 0xff, 0xff })]
        [InlineData(ulong.MaxValue, new byte[] { 0xcf, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff })]
        public void TestUnsignedLong(ulong number, byte[] data)
        {
            MsgPackSerializer.Deserialize<ulong?>(data, out var readSize).ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(0, new byte[] { 0x00 })]
        [InlineData(1, new byte[] { 1 })]
        [InlineData(byte.MaxValue, new byte[] { 0xcc, 0xff })]
        [InlineData(ushort.MaxValue, new byte[] { 0xcd, 0xff, 0xff })]
        [InlineData(uint.MaxValue, new byte[] { 0xce, 0xff, 0xff, 0xff, 0xff })]
        public void TestUnsignedInt(uint number, byte[] data)
        {
            MsgPackSerializer.Deserialize<uint?>(data, out var readSize).ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(0, new byte[] { 0x00 })]
        [InlineData(1, new byte[] { 1 })]
        [InlineData(byte.MaxValue, new byte[] { 0xcc, 0xff })]
        [InlineData(ushort.MaxValue, new byte[] { 0xcd, 0xff, 0xff })]
        public void TestUnsignedShort(ushort number, byte[] data)
        {
            MsgPackSerializer.Deserialize<ushort?>(data, out var readSize).ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(0, new byte[] { 0x00 })]
        [InlineData(1, new byte[] { 1 })]
        [InlineData(byte.MaxValue, new byte[] { 0xcc, 0xff })]
        public void TestUnsignedByte(byte number, byte[] data)
        {
            MsgPackSerializer.Deserialize<byte?>(data, out var readSize).ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }
    }
}
