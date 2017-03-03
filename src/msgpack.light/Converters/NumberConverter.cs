using System;
using System.Runtime.InteropServices;

namespace ProGaudi.MsgPack.Light.Converters
{
    internal class NumberConverter :
        IMsgPackConverter<byte>,
        IMsgPackConverter<sbyte>,
        IMsgPackConverter<short>,
        IMsgPackConverter<ushort>,
        IMsgPackConverter<int>,
        IMsgPackConverter<uint>,
        IMsgPackConverter<long>,
        IMsgPackConverter<ulong>,
        IMsgPackConverter<float>,
        IMsgPackConverter<double>
    {
        private static readonly Func<ulong, IMsgPackWriter, bool>[] PositiveSerializationMethods =
        {
            TryWriteUnsignedFixNum,
            TryWriteUInt8,
            TryWriteUInt16,
            TryWriteUInt32,
            TryWriteUInt64
        };

        private static readonly Func<long, IMsgPackWriter, bool>[] NegativeSerializationMethods =
        {
            TryWriteSignedFixNum,
            TryWriteInt8,
            TryWriteInt16,
            TryWriteInt32,
            TryWriteInt64
        };

        public void Initialize(MsgPackContext context)
        {
        }

        public void Write(double value, IMsgPackWriter writer)
        {
            var binary = new DoubleBinary(value);
            byte[] bytes;
            if (BitConverter.IsLittleEndian)
            {
                bytes = new[]
                {
                    (byte) DataTypes.Double,
                    binary.byte7,
                    binary.byte6,
                    binary.byte5,
                    binary.byte4,
                    binary.byte3,
                    binary.byte2,
                    binary.byte1,
                    binary.byte0
                };
            }
            else
            {
                bytes = new[]
                {
                    (byte) DataTypes.Double,
                    binary.byte0,
                    binary.byte1,
                    binary.byte2,
                    binary.byte3,
                    binary.byte4,
                    binary.byte5,
                    binary.byte6,
                    binary.byte7
                };
            }

            writer.Write(bytes);
        }

        double IMsgPackConverter<double>.Read(IMsgPackReader reader)
        {
            var type = reader.ReadDataType();

            if (type != DataTypes.Single && type != DataTypes.Double)
                throw ExceptionUtils.BadTypeException(type, DataTypes.Single, DataTypes.Double);

            if (type == DataTypes.Single)
            {
                return ReadFloat(reader);
            }

            var bytes = ReadBytes(reader, 8);

            return new DoubleBinary(bytes).value;
        }

        public void Write(float value, IMsgPackWriter writer)
        {
            var binary = new FloatBinary(value);
            byte[] bytes;
            if (BitConverter.IsLittleEndian)
            {
                bytes = new[]
                {
                    (byte) DataTypes.Single,
                    binary.byte3,
                    binary.byte2,
                    binary.byte1,
                    binary.byte0
                };
            }
            else
            {
                bytes = new[]
                {
                    (byte) DataTypes.Single,
                    binary.byte0,
                    binary.byte1,
                    binary.byte2,
                    binary.byte3
                };
            }

            writer.Write(bytes);
        }

        float IMsgPackConverter<float>.Read(IMsgPackReader reader)
        {
            var type = reader.ReadDataType();

            if (type != DataTypes.Single)
                throw ExceptionUtils.BadTypeException(type, DataTypes.Single);

            return ReadFloat(reader);
        }

        public void Write(byte value, IMsgPackWriter writer)
        {
            // positive fixnum
            if (value < 128L)
            {
                writer.Write(value);
            }
            else
            {
                writer.Write(DataTypes.UInt8);
                WriteByteValue(value, writer);
            }
        }

        byte IMsgPackConverter<byte>.Read(IMsgPackReader reader)
        {
            var type = reader.ReadDataType();

            if (TryGetFixPositiveNumber(type, out byte temp))
            {
                return temp;
            }

            if (TryGetNegativeNumber(type, out sbyte tempInt8))
            {
                return (byte)tempInt8;
            }

            switch (type)
            {
                case DataTypes.UInt8:
                    return ReadUInt8(reader);

                case DataTypes.Int8:
                    return (byte)ReadInt8(reader);

                default:
                    throw ExceptionUtils.IntDeserializationFailure(type);
            }
        }

        public void Write(sbyte value, IMsgPackWriter writer)
        {
            WriteInteger(value, writer);
        }

        sbyte IMsgPackConverter<sbyte>.Read(IMsgPackReader reader)
        {
            var type = reader.ReadDataType();

            if (TryGetFixPositiveNumber(type, out byte temp))
            {
                return (sbyte)temp;
            }

            if (TryGetNegativeNumber(type, out sbyte tempInt8))
            {
                return tempInt8;
            }

            if (type == DataTypes.Int8)
            {
                return ReadInt8(reader);
            }

            throw ExceptionUtils.IntDeserializationFailure(type);
        }

        public void Write(short value, IMsgPackWriter writer)
        {
            WriteInteger(value, writer);
        }

        short IMsgPackConverter<short>.Read(IMsgPackReader reader)
        {
            var type = reader.ReadDataType();

            if (TryGetFixPositiveNumber(type, out byte temp))
            {
                return temp;
            }

            if (TryGetNegativeNumber(type, out sbyte tempInt8))
            {
                return tempInt8;
            }

            switch (type)
            {
                case DataTypes.UInt8:
                    return ReadUInt8(reader);

                case DataTypes.UInt16:
                    var ushortValue = ReadUInt16(reader);
                    if (ushortValue <= short.MaxValue)
                    {
                        return (short)ushortValue;
                    }

                    throw ExceptionUtils.IntDeserializationFailure(type);

                case DataTypes.Int8:
                    return ReadInt8(reader);

                case DataTypes.Int16:
                    return ReadInt16(reader);

                default:
                    throw ExceptionUtils.IntDeserializationFailure(type);
            }
        }

        public void Write(ushort value, IMsgPackWriter writer)
        {
            WriteNonNegativeInteger(value, writer);
        }

        public ushort Read(IMsgPackReader reader)
        {
            var type = reader.ReadDataType();

            if (TryGetFixPositiveNumber(type, out byte temp))
            {
                return temp;
            }

            if (TryGetNegativeNumber(type, out sbyte tempInt8))
            {
                return (ushort)tempInt8;
            }

            switch (type)
            {
                case DataTypes.UInt8:
                    return ReadUInt8(reader);

                case DataTypes.UInt16:
                    return ReadUInt16(reader);

                case DataTypes.Int8:
                    return (ushort)ReadInt8(reader);

                case DataTypes.Int16:
                    return (ushort)ReadInt16(reader);

                default:
                    throw ExceptionUtils.IntDeserializationFailure(type);
            }
        }

        public void Write(int value, IMsgPackWriter writer)
        {
            WriteInteger(value, writer);
        }

        int IMsgPackConverter<int>.Read(IMsgPackReader reader)
        {
            var type = reader.ReadDataType();

            if (TryGetFixPositiveNumber(type, out byte temp))
            {
                return temp;
            }

            if (TryGetNegativeNumber(type, out sbyte tempInt8))
            {
                return tempInt8;
            }

            switch (type)
            {
                case DataTypes.UInt8:
                    return ReadUInt8(reader);

                case DataTypes.UInt16:
                    return ReadUInt16(reader);

                case DataTypes.UInt32:
                    var uintValue = ReadUInt32(reader);
                    if (uintValue <= int.MaxValue)
                    {
                        return (int)uintValue;
                    }

                    throw ExceptionUtils.IntDeserializationFailure(type);

                case DataTypes.Int8:
                    return ReadInt8(reader);

                case DataTypes.Int16:
                    return ReadInt16(reader);

                case DataTypes.Int32:
                    return ReadInt32(reader);

                default:
                    throw ExceptionUtils.IntDeserializationFailure(type);
            }
        }

        public void Write(uint value, IMsgPackWriter writer)
        {
            WriteNonNegativeInteger(value, writer);
        }

        uint IMsgPackConverter<uint>.Read(IMsgPackReader reader)
        {
            var type = reader.ReadDataType();

            if (TryGetFixPositiveNumber(type, out byte temp))
            {
                return temp;
            }

            if (TryGetNegativeNumber(type, out sbyte tempInt8))
            {
                return (uint)tempInt8;
            }

            switch (type)
            {
                case DataTypes.UInt8:
                    return ReadUInt8(reader);

                case DataTypes.UInt16:
                    return ReadUInt16(reader);

                case DataTypes.UInt32:
                    return ReadUInt32(reader);

                case DataTypes.Int8:
                    return (uint)ReadInt8(reader);

                case DataTypes.Int16:
                    return (uint)ReadInt16(reader);

                case DataTypes.Int32:
                    return (uint)ReadInt32(reader);

                default:
                    throw ExceptionUtils.IntDeserializationFailure(type);
            }
        }

        public void Write(long value, IMsgPackWriter writer)
        {
            WriteInteger(value, writer);
        }

        long IMsgPackConverter<long>.Read(IMsgPackReader reader)
        {
            var type = reader.ReadDataType();

            if (TryGetFixPositiveNumber(type, out byte tempUInt8))
            {
                return tempUInt8;
            }

            if (TryGetNegativeNumber(type, out sbyte tempInt8))
            {
                return tempInt8;
            }

            switch (type)
            {
                case DataTypes.UInt8:
                    return ReadUInt8(reader);

                case DataTypes.UInt16:
                    return ReadUInt16(reader);

                case DataTypes.UInt32:
                    return ReadUInt32(reader);

                case DataTypes.UInt64:
                    var ulongValue = ReadUInt64(reader);
                    if (ulongValue <= long.MaxValue)
                    {
                        return (long)ulongValue;
                    }

                    throw ExceptionUtils.IntDeserializationFailure(type);

                case DataTypes.Int8:
                    return ReadInt8(reader);

                case DataTypes.Int16:
                    return ReadInt16(reader);

                case DataTypes.Int32:
                    return ReadInt32(reader);

                case DataTypes.Int64:
                    return ReadInt64(reader);

                default:
                    throw ExceptionUtils.IntDeserializationFailure(type);
            }
        }

        public void Write(ulong value, IMsgPackWriter writer)
        {
            WriteNonNegativeInteger(value, writer);
        }

        ulong IMsgPackConverter<ulong>.Read(IMsgPackReader reader)
        {
            var type = reader.ReadDataType();

            if (TryGetFixPositiveNumber(type, out byte temp))
            {
                return temp;
            }

            if (TryGetNegativeNumber(type, out sbyte tempInt8))
            {
                return (ulong)tempInt8;
            }

            switch (type)
            {
                case DataTypes.UInt8:
                    return ReadUInt8(reader);

                case DataTypes.UInt16:
                    return ReadUInt16(reader);

                case DataTypes.UInt32:
                    return ReadUInt32(reader);

                case DataTypes.UInt64:
                    return ReadUInt64(reader);

                case DataTypes.Int8:
                    return (ulong)ReadInt8(reader);

                case DataTypes.Int16:
                    return (ulong)ReadInt16(reader);

                case DataTypes.Int32:
                    return (ulong)ReadInt32(reader);

                case DataTypes.Int64:
                    return (ulong)ReadInt64(reader);

                default:
                    throw ExceptionUtils.IntDeserializationFailure(type);
            }
        }

        private static bool TryGetFixPositiveNumber(DataTypes type, out byte temp)
        {
            temp = (byte)type;
            return type.GetHighBits(1) == DataTypes.PositiveFixNum.GetHighBits(1);
        }

        private static bool TryGetNegativeNumber(DataTypes type, out sbyte temp)
        {
            temp = (sbyte)((byte)type - 1 - byte.MaxValue);

            return type.GetHighBits(3) == DataTypes.NegativeFixNum.GetHighBits(3);
        }

        internal static sbyte ReadInt8(IMsgPackReader reader)
        {
            var temp = reader.ReadByte();
            if (temp <= sbyte.MaxValue)
                return (sbyte)temp;

            return (sbyte)(temp - byte.MaxValue - 1);
        }

        internal static byte ReadUInt8(IMsgPackReader reader)
        {
            return reader.ReadByte();
        }

        internal static ushort ReadUInt16(IMsgPackReader reader)
        {
            return (ushort)((reader.ReadByte() << 8) + reader.ReadByte());
        }

        internal static short ReadInt16(IMsgPackReader reader)
        {
            var temp = ReadUInt16(reader);
            if (temp <= short.MaxValue)
                return (short)temp;

            return (short)(temp - 1 - ushort.MaxValue);
        }

        internal static int ReadInt32(IMsgPackReader reader)
        {
            var temp = ReadUInt32(reader);
            if (temp <= int.MaxValue)
                return (int)temp;

            return (int)(temp - 1 - uint.MaxValue);
        }

        internal static uint ReadUInt32(IMsgPackReader reader)
        {
            var temp = (uint)(reader.ReadByte() << 24);
            temp += (uint)reader.ReadByte() << 16;
            temp += (uint)reader.ReadByte() << 8;
            temp += reader.ReadByte();

            return temp;
        }

        internal static ulong ReadUInt64(IMsgPackReader reader)
        {
            var temp = (ulong)reader.ReadByte() << 56;
            temp += (ulong)reader.ReadByte() << 48;
            temp += (ulong)reader.ReadByte() << 40;
            temp += (ulong)reader.ReadByte() << 32;
            temp += (ulong)reader.ReadByte() << 24;
            temp += (ulong)reader.ReadByte() << 16;
            temp += (ulong)reader.ReadByte() << 8;
            temp += reader.ReadByte();

            return temp;
        }

        internal static long ReadInt64(IMsgPackReader reader)
        {
            var temp = ReadUInt64(reader);
            if (temp <= long.MaxValue)
                return (long)temp;

            return (long)(temp - 1 - ulong.MaxValue);
        }

        private static bool TryWriteSignedFixNum(long value, IMsgPackWriter writer)
        {
            // positive fixnum
            if (value >= 0 && value < 128L)
            {
                writer.Write(unchecked((byte)value));
                return true;
            }

            // negative fixnum
            if (value >= -32L && value <= -1L)
            {
                writer.Write(unchecked((byte)value));
                return true;
            }

            return false;
        }

        private static bool TryWriteUnsignedFixNum(ulong value, IMsgPackWriter writer)
        {
            // positive fixnum
            if (value < 128L)
            {
                writer.Write(unchecked((byte)value));
                return true;
            }

            return false;
        }

        private static bool TryWriteInt8(long value, IMsgPackWriter writer)
        {
            if (value > sbyte.MaxValue || value < sbyte.MinValue)
            {
                return false;
            }

            writer.Write(DataTypes.Int8);
            WriteSByteValue((sbyte)value, writer);
            return true;
        }

        public static void WriteSByteValue(sbyte value, IMsgPackWriter writer)
        {
            writer.Write((byte)value);
        }

        private static bool TryWriteUInt8(ulong value, IMsgPackWriter writer)
        {
            if (value > byte.MaxValue)
            {
                return false;
            }

            writer.Write(DataTypes.UInt8);
            WriteByteValue((byte)value, writer);
            return true;
        }

        public static void WriteByteValue(byte value, IMsgPackWriter writer)
        {
            writer.Write(value);
        }

        private static bool TryWriteInt16(long value, IMsgPackWriter writer)
        {
            if (value < short.MinValue || value > short.MaxValue)
            {
                return false;
            }

            writer.Write(DataTypes.Int16);
            WriteShortValue((short)value, writer);
            return true;
        }

        public static void WriteShortValue(short value, IMsgPackWriter writer)
        {
            unchecked
            {
                writer.Write((byte)((value >> 8) & 0xff));
                writer.Write((byte)(value & 0xff));
            }
        }

        private static bool TryWriteUInt16(ulong value, IMsgPackWriter writer)
        {
            if (value > ushort.MaxValue)
            {
                return false;
            }

            writer.Write(DataTypes.UInt16);
            WriteUShortValue((ushort)value, writer);
            return true;
        }

        public static void WriteUShortValue(ushort value, IMsgPackWriter writer)
        {
            unchecked
            {
                writer.Write((byte)((value >> 8) & 0xff));
                writer.Write((byte)(value & 0xff));
            }
        }

        private static bool TryWriteInt32(long value, IMsgPackWriter writer)
        {
            if (value > int.MaxValue || value < int.MinValue)
            {
                return false;
            }

            writer.Write(DataTypes.Int32);
            WriteIntValue((int)value, writer);
            return true;
        }

        public static void WriteIntValue(int value, IMsgPackWriter writer)
        {
            unchecked
            {
                writer.Write((byte)((value >> 24) & 0xff));
                writer.Write((byte)((value >> 16) & 0xff));
                writer.Write((byte)((value >> 8) & 0xff));
                writer.Write((byte)(value & 0xff));
            }
        }

        private static bool TryWriteUInt32(ulong value, IMsgPackWriter writer)
        {
            if (value > uint.MaxValue)
            {
                return false;
            }

            writer.Write(DataTypes.UInt32);
            WriteUIntValue((uint)value, writer);
            return true;
        }

        public static void WriteUIntValue(uint value, IMsgPackWriter writer)
        {
            unchecked
            {
                writer.Write((byte)((value >> 24) & 0xff));
                writer.Write((byte)((value >> 16) & 0xff));
                writer.Write((byte)((value >> 8) & 0xff));
                writer.Write((byte)(value & 0xff));
            }
        }

        private static bool TryWriteInt64(long value, IMsgPackWriter writer)
        {
            writer.Write(DataTypes.Int64);
            WriteLongValue(value, writer);
            return true;
        }

        public static void WriteLongValue(long value, IMsgPackWriter writer)
        {
            unchecked
            {
                writer.Write((byte)((value >> 56) & 0xff));
                writer.Write((byte)((value >> 48) & 0xff));
                writer.Write((byte)((value >> 40) & 0xff));
                writer.Write((byte)((value >> 32) & 0xff));
                writer.Write((byte)((value >> 24) & 0xff));
                writer.Write((byte)((value >> 16) & 0xff));
                writer.Write((byte)((value >> 8) & 0xff));
                writer.Write((byte)(value & 0xff));
            }
        }

        private static bool TryWriteUInt64(ulong value, IMsgPackWriter writer)
        {
            writer.Write(DataTypes.UInt64);
            WriteULongValue(value, writer);
            return true;
        }

        public static void WriteULongValue(ulong value, IMsgPackWriter writer)
        {
            unchecked
            {
                writer.Write((byte)((value >> 56) & 0xff));
                writer.Write((byte)((value >> 48) & 0xff));
                writer.Write((byte)((value >> 40) & 0xff));
                writer.Write((byte)((value >> 32) & 0xff));
                writer.Write((byte)((value >> 24) & 0xff));
                writer.Write((byte)((value >> 16) & 0xff));
                writer.Write((byte)((value >> 8) & 0xff));
                writer.Write((byte)(value & 0xff));
            }
        }

        private static void WriteInteger(long value, IMsgPackWriter writer)
        {
            if (value >= 0)
            {
                WriteNonNegativeInteger((ulong)value, writer);
                return;
            }

            for (var i = 0; i < NegativeSerializationMethods.Length; i++)
            {
                if (NegativeSerializationMethods[i](value, writer))
                {
                    return;
                }
            }

            throw ExceptionUtils.IntSerializationFailure(value);
        }

        private static void WriteNonNegativeInteger(ulong value, IMsgPackWriter writer)
        {
            for (var i = 0; i < PositiveSerializationMethods.Length; i++)
            {
                if (PositiveSerializationMethods[i](value, writer))
                {
                    return;
                }
            }

            throw ExceptionUtils.IntSerializationFailure(value);
        }

        private static float ReadFloat(IMsgPackReader reader)
        {
            var bytes = ReadBytes(reader, 4);

            return new FloatBinary(bytes).value;
        }

        private static ArraySegment<byte> ReadBytes(IMsgPackReader reader, uint length)
        {
            return reader.ReadBytes(length);
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct FloatBinary
        {
            [FieldOffset(0)]
            public readonly float value;

            [FieldOffset(0)]
            public readonly byte byte0;

            [FieldOffset(1)]
            public readonly byte byte1;

            [FieldOffset(2)]
            public readonly byte byte2;

            [FieldOffset(3)]
            public readonly byte byte3;

            public FloatBinary(float f)
            {
                this = default(FloatBinary);
                value = f;
            }

            public FloatBinary(ArraySegment<byte> bytes)
            {
                value = 0;
                if (BitConverter.IsLittleEndian)
                {
                    byte0 = bytes.Array[bytes.Offset + 3];
                    byte1 = bytes.Array[bytes.Offset + 2];
                    byte2 = bytes.Array[bytes.Offset + 1];
                    byte3 = bytes.Array[bytes.Offset + 0];
                }
                else
                {
                    byte0 = bytes.Array[bytes.Offset + 0];
                    byte1 = bytes.Array[bytes.Offset + 1];
                    byte2 = bytes.Array[bytes.Offset + 2];
                    byte3 = bytes.Array[bytes.Offset + 3];
                }
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct DoubleBinary
        {
            [FieldOffset(0)]
            public readonly double value;

            [FieldOffset(0)]
            public readonly byte byte0;

            [FieldOffset(1)]
            public readonly byte byte1;

            [FieldOffset(2)]
            public readonly byte byte2;

            [FieldOffset(3)]
            public readonly byte byte3;

            [FieldOffset(4)]
            public readonly byte byte4;

            [FieldOffset(5)]
            public readonly byte byte5;

            [FieldOffset(6)]
            public readonly byte byte6;

            [FieldOffset(7)]
            public readonly byte byte7;

            public DoubleBinary(double f)
            {
                this = default(DoubleBinary);
                value = f;
            }

            public DoubleBinary(ArraySegment<byte> bytes)
            {
                value = 0;
                if (BitConverter.IsLittleEndian)
                {
                    byte0 = bytes.Array[bytes.Offset + 7];
                    byte1 = bytes.Array[bytes.Offset + 6];
                    byte2 = bytes.Array[bytes.Offset + 5];
                    byte3 = bytes.Array[bytes.Offset + 4];
                    byte4 = bytes.Array[bytes.Offset + 3];
                    byte5 = bytes.Array[bytes.Offset + 2];
                    byte6 = bytes.Array[bytes.Offset + 1];
                    byte7 = bytes.Array[bytes.Offset + 0];
                }
                else
                {
                    byte0 = bytes.Array[bytes.Offset + 0];
                    byte1 = bytes.Array[bytes.Offset + 1];
                    byte2 = bytes.Array[bytes.Offset + 2];
                    byte3 = bytes.Array[bytes.Offset + 3];
                    byte4 = bytes.Array[bytes.Offset + 4];
                    byte5 = bytes.Array[bytes.Offset + 5];
                    byte6 = bytes.Array[bytes.Offset + 6];
                    byte7 = bytes.Array[bytes.Offset + 7];
                }
            }
        }
    }
}