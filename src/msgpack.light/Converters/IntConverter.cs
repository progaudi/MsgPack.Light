using System;

namespace MsgPack.Light.Converters
{
    internal class IntConverter :
        IMsgPackConverter<byte>,
        IMsgPackConverter<sbyte>,
        IMsgPackConverter<short>,
        IMsgPackConverter<ushort>,
        IMsgPackConverter<int>,
        IMsgPackConverter<uint>,
        IMsgPackConverter<long>,
        IMsgPackConverter<ulong>
    {
        public void Initialize(MsgPackContext context)
        {
        }

        public void Write(byte value, IMsgPackWriter writer)
        {
            if (TryWriteUnsignedFixNum(value, writer) ||
                TryWriteUInt8(value, writer))
            {
            }
        }

        byte IMsgPackConverter<byte>.Read(IMsgPackReader reader)
        {
            var type = reader.ReadDataType();

            byte temp;
            if (TryGetFixPositiveNumber(type, out temp))
            {
                return temp;
            }

            sbyte tempInt8;
            if (TryGetNegativeNumber(type, out tempInt8))
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
            if (TryWriteSignedFixNum(value, writer) ||
                TryWriteInt8(value, writer))
            {
            }
        }

        sbyte IMsgPackConverter<sbyte>.Read(IMsgPackReader reader)
        {
            var type = reader.ReadDataType();

            byte temp;
            if (TryGetFixPositiveNumber(type, out temp))
            {
                return (sbyte)temp;
            }

            sbyte tempInt8;
            if (TryGetNegativeNumber(type, out tempInt8))
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
            if (TryWriteSignedFixNum(value, writer) ||
                 TryWriteInt8(value, writer) ||
                 TryWriteInt16(value, writer))
            {
            }
        }

        short IMsgPackConverter<short>.Read(IMsgPackReader reader)
        {
            var type = reader.ReadDataType();

            byte temp;
            if (TryGetFixPositiveNumber(type, out temp))
            {
                return temp;
            }

            sbyte tempInt8;
            if (TryGetNegativeNumber(type, out tempInt8))
            {
                return tempInt8;
            }

            switch (type)
            {
                case DataTypes.UInt8:
                    return ReadUInt8(reader);

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
            if (TryWriteUnsignedFixNum(value, writer) ||
                TryWriteUInt8(value, writer) ||
                TryWriteUInt16(value, writer))
            {
            }
        }

        public ushort Read(IMsgPackReader reader)
        {
            var type = reader.ReadDataType();

            byte temp;
            if (TryGetFixPositiveNumber(type, out temp))
            {
                return temp;
            }

            sbyte tempInt8;
            if (TryGetNegativeNumber(type, out tempInt8))
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
            if (TryWriteSignedFixNum(value, writer))
                return;
            if (TryWriteInt8(value, writer))
                return;
            if (TryWriteInt16(value, writer))
                return;
            TryWriteInt32(value, writer);
        }

        int IMsgPackConverter<int>.Read(IMsgPackReader reader)
        {
            var type = reader.ReadDataType();

            byte temp;
            if (TryGetFixPositiveNumber(type, out temp))
            {
                return temp;
            }

            sbyte tempInt8;
            if (TryGetNegativeNumber(type, out tempInt8))
            {
                return tempInt8;
            }

            switch (type)
            {
                case DataTypes.UInt8:
                    return ReadUInt8(reader);

                case DataTypes.UInt16:
                    return ReadUInt16(reader);

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
            if (TryWriteUnsignedFixNum(value, writer) ||
                TryWriteUInt8(value, writer) ||
                TryWriteUInt16(value, writer) ||
                TryWriteUInt32(value, writer))
            {
            }
        }

        uint IMsgPackConverter<uint>.Read(IMsgPackReader reader)
        {
            var type = reader.ReadDataType();

            byte temp;
            if (TryGetFixPositiveNumber(type, out temp))
            {
                return temp;
            }

            sbyte tempInt8;
            if (TryGetNegativeNumber(type, out tempInt8))
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
            if (TryWriteSignedFixNum(value, writer) ||
                TryWriteInt8(value, writer) ||
                TryWriteInt16(value, writer) ||
                TryWriteInt32(value, writer) ||
                TryWriteInt64(value, writer))
            {
            }
        }

        long IMsgPackConverter<long>.Read(IMsgPackReader reader)
        {
            var type = reader.ReadDataType();

            byte tempUInt8;
            if (TryGetFixPositiveNumber(type, out tempUInt8))
            {
                return tempUInt8;
            }

            sbyte tempInt8;
            if (TryGetNegativeNumber(type, out tempInt8))
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
            if (TryWriteUnsignedFixNum(value, writer) ||
                TryWriteUInt8(value, writer) ||
                TryWriteUInt16(value, writer) ||
                TryWriteUInt32(value, writer) ||
                TryWriteUInt64(value, writer))
            {
            }
        }

        ulong IMsgPackConverter<ulong>.Read(IMsgPackReader reader)
        {
            var type = reader.ReadDataType();

            byte temp;
            if (TryGetFixPositiveNumber(type, out temp))
            {
                return temp;
            }

            sbyte tempInt8;
            if (TryGetNegativeNumber(type, out tempInt8))
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

        public static bool TryWriteSignedFixNum(long value, IMsgPackWriter writer)
        {
            // positive fixnum
            if ( value >= 0 && value < 128L )
            {
                writer.Write( unchecked( ( byte )value ) );
                return true;
            }

            // negative fixnum
            if ( value >= -32L && value <= -1L )
            {
                writer.Write( unchecked( ( byte )value ) );
                return true;
            }

            return false;
        }

        public static bool TryWriteUnsignedFixNum(ulong value, IMsgPackWriter writer)
        {
            // positive fixnum
            if (value < 128L)
            {
                writer.Write(unchecked((byte)value));
                return true;
            }

            return false;
        }

        public static bool TryWriteInt8(long value, IMsgPackWriter writer)
        {
            if (value > sbyte.MaxValue || value < sbyte.MinValue)
            {
                return false;
            }

            writer.Write(DataTypes.Int8);
            writer.Write((byte)value);
            return true;
        }

        public static bool TryWriteUInt8(ulong value, IMsgPackWriter writer)
        {
            if (value > byte.MaxValue)
            {
                return false;
            }

            writer.Write(DataTypes.UInt8);
            writer.Write((byte)value);
            return true;
        }

        public static bool TryWriteInt16(long value, IMsgPackWriter writer)
        {
            if (value < short.MinValue || value > short.MaxValue)
            {
                return false;
            }

            writer.Write(DataTypes.Int16);
            unchecked
            {
                writer.Write((byte)((value >> 8) & 0xff));
                writer.Write((byte)(value & 0xff));
            }
            return true;
        }

        public static bool TryWriteUInt16(ulong value, IMsgPackWriter writer)
        {
            if (value > ushort.MaxValue)
            {
                return false;
            }

            writer.Write(DataTypes.UInt16);
            unchecked
            {
                writer.Write((byte)((value >> 8) & 0xff));
                writer.Write((byte)(value & 0xff));
            }
            return true;
        }

        public static bool TryWriteInt32(long value, IMsgPackWriter writer)
        {
            if (value > int.MaxValue || value < int.MinValue)
            {
                return false;
            }

            writer.Write(DataTypes.Int32);
            unchecked
            {
                writer.Write((byte)((value >> 24) & 0xff));
                writer.Write((byte)((value >> 16) & 0xff));
                writer.Write((byte)((value >> 8) & 0xff));
                writer.Write((byte)(value & 0xff));
            }
            return true;
        }

        public static bool TryWriteUInt32(ulong value, IMsgPackWriter writer)
        {
            if (value > uint.MaxValue)
            {
                return false;
            }

            writer.Write(DataTypes.UInt32);
            unchecked
            {
                writer.Write((byte)((value >> 24) & 0xff));
                writer.Write((byte)((value >> 16) & 0xff));
                writer.Write((byte)((value >> 8) & 0xff));
                writer.Write((byte)(value & 0xff));
            }
            return true;
        }

        public static bool TryWriteInt64(long value, IMsgPackWriter writer)
        {
            writer.Write(DataTypes.Int64);
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
            return true;
        }

        public static bool TryWriteUInt64(ulong value, IMsgPackWriter writer)
        {
            writer.Write(DataTypes.UInt64);
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
            return true;
        }
    }
}