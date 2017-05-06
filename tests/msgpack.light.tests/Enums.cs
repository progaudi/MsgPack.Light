using System;

namespace ProGaudi.MsgPack.Light.Tests
{
    public enum DefaultEnum
    {
        TestValue1,

        TestValue2,

        TestValue3 = 3
    }

    [Flags]
    public enum FlagEnum
    {
        Value1,

        Value2,

        Value3 = Value1 | Value2,

        Value4 = Value1 & Value2
    }

    public enum SbyteEnum : sbyte
    {
        Value1,
        Value2,
        Value3 = 3,
        Value4 = sbyte.MaxValue,
        Value5 = sbyte.MinValue,
    }

    public enum ByteEnum : byte
    {
        Value1,
        Value2,
        Value3 = 3,
        Value4 = byte.MaxValue,
        Value5 = byte.MinValue,
    }

    public enum ShortEnum : short
    {
        Value1,
        Value2,
        Value3 = 3,
        Value4 = short.MaxValue,
        Value5 = short.MinValue,
    }

    public enum UshortEnum : ushort
    {
        Value1,
        Value2,
        Value3 = 3,
        Value4 = ushort.MaxValue,
        Value5 = ushort.MinValue,
    }

    public enum IntEnum : int
    {
        Value1,
        Value2,
        Value3 = 3,
        Value4 = int.MaxValue,
        Value5 = int.MinValue,
    }

    public enum UintEnum : uint
    {
        Value1,
        Value2,
        Value3 = 3,
        Value4 = uint.MaxValue,
        Value5 = uint.MinValue,
    }

    public enum LongEnum : long
    {
        Value1,
        Value2,
        Value3 = 3,
        Value4 = long.MaxValue,
        Value5 = long.MinValue,
    }

    public enum UlongEnum : ulong
    {
        Value1,
        Value2,
        Value3 = 3,
        Value4 = ulong.MaxValue,
        Value5 = ulong.MinValue,
    }
}