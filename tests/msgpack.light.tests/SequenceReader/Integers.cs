using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests.SequenceReader
{
    public class Integers
    {
        [Theory]
        [InlineData(0, new byte[] {0x00})]
        [InlineData(1, new byte[] {1})]
        [InlineData(-1, new byte[] {0xff})]
        [InlineData(sbyte.MinValue, new byte[] {208, 128})]
        [InlineData(sbyte.MaxValue, new byte[] {127})]
        [InlineData(short.MinValue, new byte[] {209, 128, 0})]
        [InlineData(short.MaxValue, new byte[] {209, 127, 0xff})]
        [InlineData(int.MinValue, new byte[] {210, 128, 0, 0, 0})]
        [InlineData(int.MaxValue, new byte[] {210, 127, 0xff, 0xff, 0xff})]
        [InlineData(long.MaxValue, new byte[] {211, 127, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff})]
        [InlineData(long.MinValue, new byte[] {211, 128, 0, 0, 0, 0, 0, 0, 0})]
        [InlineData(long.MaxValue, new byte[] { 207, 127, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff })]
        public void TestSignedLong(long number, byte[] data)
        {
            MsgPackSerializer.Deserialize<long>(data.ToMultipleSegments(), out var readSize).ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(0, new byte[] {0x00})]
        [InlineData(1, new byte[] {1})]
        [InlineData(-1, new byte[] {0xff})]
        [InlineData(sbyte.MinValue, new byte[] {208, 128})]
        [InlineData(sbyte.MaxValue, new byte[] {127})]
        [InlineData(short.MinValue, new byte[] {209, 128, 0})]
        [InlineData(short.MaxValue, new byte[] {209, 127, 0xff})]
        [InlineData(int.MinValue, new byte[] {210, 128, 0, 0, 0})]
        [InlineData(int.MaxValue, new byte[] {210, 127, 0xff, 0xff, 0xff})]
        [InlineData(50505, new byte[] {205, 197, 73})]
        public void TestSignedInt(int number, byte[] data)
        {
            MsgPackSerializer.Deserialize<int>(data.ToMultipleSegments(), out var readSize).ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(0, new byte[] {0x00})]
        [InlineData(1, new byte[] {1})]
        [InlineData(-1, new byte[] {0xff})]
        [InlineData(sbyte.MinValue, new byte[] {208, 128})]
        [InlineData(sbyte.MaxValue, new byte[] {127})]
        [InlineData(short.MinValue, new byte[] {209, 128, 0})]
        [InlineData(short.MaxValue, new byte[] {209, 127, 0xff})]
        public void TestSignedShort(short number, byte[] data)
        {
            MsgPackSerializer.Deserialize<short>(data.ToMultipleSegments(), out var readSize).ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(0, new byte[] {0x00})]
        [InlineData(1, new byte[] {1})]
        [InlineData(-1, new byte[] {0xff})]
        [InlineData(sbyte.MinValue, new byte[] {208, 128})]
        [InlineData(sbyte.MaxValue, new byte[] {127})]
        public void TestSignedByte(sbyte number, byte[] data)
        {
            MsgPackSerializer.Deserialize<sbyte>(data.ToMultipleSegments(), out var readSize).ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(0, new byte[] {0x00})]
        [InlineData(1, new byte[] {1})]
        [InlineData(byte.MaxValue, new byte[] { 0xcc, 0xff })]
        [InlineData(ushort.MaxValue, new byte[] { 0xcd, 0xff, 0xff })]
        [InlineData(uint.MaxValue, new byte[] { 0xce, 0xff, 0xff, 0xff, 0xff })]
        [InlineData(ulong.MaxValue, new byte[] { 0xcf, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff })]
        public void TestUnsignedLong(ulong number, byte[] data)
        {
            MsgPackSerializer.Deserialize<ulong>(data.ToMultipleSegments(), out var readSize).ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(0, new byte[] {0x00})]
        [InlineData(1, new byte[] {1})]
        [InlineData(byte.MaxValue, new byte[] {0xcc, 0xff})]
        [InlineData(ushort.MaxValue, new byte[] {0xcd, 0xff, 0xff})]
        [InlineData(uint.MaxValue, new byte[] {0xce, 0xff, 0xff, 0xff, 0xff})]
        [InlineData(0x10000000, new byte[] {0xce, 0x10, 0x00, 0x00, 0x00})]
        public void TestUnsignedInt(uint number, byte[] data)
        {
            MsgPackSerializer.Deserialize<uint>(data.ToMultipleSegments(), out var readSize).ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(0, new byte[] {0x00})]
        [InlineData(1, new byte[] {1})]
        [InlineData(byte.MaxValue, new byte[] {0xcc, 0xff})]
        [InlineData(ushort.MaxValue, new byte[] {0xcd, 0xff, 0xff})]
        public void TestUnsignedShort(ushort number, byte[] data)
        {
            MsgPackSerializer.Deserialize<ushort>(data.ToMultipleSegments(), out var readSize).ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }

        [Theory]
        [InlineData(0, new byte[] {0x00})]
        [InlineData(1, new byte[] {1})]
        [InlineData(byte.MaxValue, new byte[] {0xcc, 0xff})]
        public void TestUnsignedByte(byte number, byte[] data)
        {
            MsgPackSerializer.Deserialize<byte>(data.ToMultipleSegments(), out var readSize).ShouldBe(number);
            readSize.ShouldBe(data.Length);
        }
    }
}
