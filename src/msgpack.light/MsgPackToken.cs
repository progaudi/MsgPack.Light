using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ProGaudi.MsgPack.Light
{
    public class MsgPackToken
    {
        internal DataTypeInternal DataTypeInternal { get; }

        internal byte[] ValueBytes { get; }

        internal MsgPackToken[] ArrayElements { get; }

        internal KeyValuePair<MsgPackToken, MsgPackToken>[] MapElements{ get; }

        internal MsgPackToken(DataTypeInternal dataTypeInternal)
        {
            this.DataTypeInternal = dataTypeInternal;
        }

        internal MsgPackToken(DataTypeInternal dataTypeInternal, byte[] valueBytes) : this(dataTypeInternal)
        {
            this.ValueBytes = valueBytes;
        }

        internal MsgPackToken(DataTypeInternal dataTypeInternal, KeyValuePair<MsgPackToken, MsgPackToken>[] mapElements) : this(dataTypeInternal)
        {
            this.MapElements = mapElements;
        }

        internal MsgPackToken(DataTypeInternal dataTypeInternal, MsgPackToken[] arrayElements) : this(dataTypeInternal)
        {
            this.ArrayElements = arrayElements;
        }

        public DataType DataType => DataTypeInternal.GetDataType();

        #region Bool typeInternal conversion

        public static explicit operator MsgPackToken(bool b)
        {
            return new MsgPackToken(b ? DataTypeInternal.True : DataTypeInternal.False);
        }

        public static explicit operator bool(MsgPackToken token)
        {
            if (token == null)
            {
                throw ExceptionUtils.NullTokenExpection("bool");
            }

            switch (token.DataTypeInternal)
            {
                case DataTypeInternal.True:
                    return true;
                case DataTypeInternal.False:
                    return false;
                default:
                    throw ExceptionUtils.BadTypeException(token.DataTypeInternal, DataTypeInternal.True, DataTypeInternal.False);
            }
        }

        #endregion

        #region String typeInternal conversion

        public static explicit operator MsgPackToken(string str)
        {
            if (str == null)
            {
                return CreateNullToken();
            }

            var data = Encoding.UTF8.GetBytes(str);

            var length = (uint)data.Length;

            DataTypeInternal dataTypeInternal;
            if (length <= 31)
            {
                dataTypeInternal = (DataTypeInternal)(((byte)DataTypeInternal.FixStr + length) % 256);
            }
            else if (length <= byte.MaxValue)
            {
                dataTypeInternal = DataTypeInternal.Str8;
            }
            else if (length <= ushort.MaxValue)
            {
                dataTypeInternal = DataTypeInternal.Str16;
            }
            else
            {
                dataTypeInternal = DataTypeInternal.Str32;
            }

            return new MsgPackToken(dataTypeInternal, data);
        }

        public static explicit operator string(MsgPackToken token)
        {
            if (token == null)
            {
                return null;
            }

            switch (token.DataTypeInternal)
            {
                case DataTypeInternal.Null:
                    return null;
                case DataTypeInternal.Str8:
                case DataTypeInternal.Str16:
                case DataTypeInternal.Str32:
                    return Encoding.UTF8.GetString(token.ValueBytes, 0, token.ValueBytes.Length);
            }

            if (TryGetFixstrLength(token.DataTypeInternal, out uint length))
            {
                return Encoding.UTF8.GetString(token.ValueBytes, 0, token.ValueBytes.Length);
            }

            throw ExceptionUtils.BadTypeException(token.DataTypeInternal, DataTypeInternal.FixStr, DataTypeInternal.Str8, DataTypeInternal.Str16, DataTypeInternal.Str32);
        }

        #endregion

        #region byte[] typeInternal conversion

        public static explicit operator MsgPackToken(byte[] data)
        {
            if (data == null)
            {
                return CreateNullToken();
            }

            var length = (uint)data.Length;

            DataTypeInternal dataTypeInternal;
            if (length <= byte.MaxValue)
            {
                dataTypeInternal = DataTypeInternal.Bin8;
            }
            else if (length <= ushort.MaxValue)
            {
                dataTypeInternal = DataTypeInternal.Bin16;
            }
            else
            {
                dataTypeInternal = DataTypeInternal.Bin32;
            }

            return new MsgPackToken(dataTypeInternal, data);
        }

        public static explicit operator byte[] (MsgPackToken token)
        {
            if (token == null)
            {
                return null;
            }

            switch (token.DataTypeInternal)
            {
                case DataTypeInternal.Null:
                    return null;
                case DataTypeInternal.Bin8:
                case DataTypeInternal.Bin16:
                case DataTypeInternal.Bin32:
                    var array = new byte[token.ValueBytes.Length];
                    Array.Copy(token.ValueBytes, 0, array, 0, token.ValueBytes.Length);
                    return array;
            }

            throw ExceptionUtils.BadTypeException(token.DataTypeInternal, DataTypeInternal.Bin8, DataTypeInternal.Bin16, DataTypeInternal.Bin32, DataTypeInternal.Null);
        }

        #endregion

        #region ulong typeInternal conversion

        public static explicit operator MsgPackToken(ulong value)
        {
            return WriteNonNegativeInteger(value);
        }

        public static explicit operator ulong(MsgPackToken token)
        {
            if (TryGetFixPositiveNumber(token.DataTypeInternal, out byte temp))
            {
                return temp;
            }

            if (TryGetNegativeNumber(token.DataTypeInternal, out sbyte tempInt8))
            {
                return (ulong)tempInt8;
            }

            switch (token.DataTypeInternal)
            {
                case DataTypeInternal.UInt8:
                    return ReadUInt8(token.ValueBytes);

                case DataTypeInternal.UInt16:
                    return ReadUInt16(token.ValueBytes);

                case DataTypeInternal.UInt32:
                    return ReadUInt32(token.ValueBytes);

                case DataTypeInternal.UInt64:
                    return ReadUInt64(token.ValueBytes);

                case DataTypeInternal.Int8:
                    return (ulong)ReadInt8(token.ValueBytes);

                case DataTypeInternal.Int16:
                    return (ulong)ReadInt16(token.ValueBytes);

                case DataTypeInternal.Int32:
                    return (ulong)ReadInt32(token.ValueBytes);

                case DataTypeInternal.Int64:
                    return (ulong)ReadInt64(token.ValueBytes);

                default:
                    throw ExceptionUtils.IntDeserializationFailure(token.DataTypeInternal);
            }
        }

        #endregion

        #region uint typeInternal conversion

        public static explicit operator MsgPackToken(uint value)
        {
            return WriteNonNegativeInteger(value);
        }

        public static explicit operator uint(MsgPackToken token)
        {
            if (TryGetFixPositiveNumber(token.DataTypeInternal, out byte temp))
            {
                return temp;
            }

            if (TryGetNegativeNumber(token.DataTypeInternal, out sbyte tempInt8))
            {
                return (uint)tempInt8;
            }

            switch (token.DataTypeInternal)
            {
                case DataTypeInternal.UInt8:
                    return ReadUInt8(token.ValueBytes);

                case DataTypeInternal.UInt16:
                    return ReadUInt16(token.ValueBytes);

                case DataTypeInternal.UInt32:
                    return ReadUInt32(token.ValueBytes);

                case DataTypeInternal.Int8:
                    return (uint)ReadInt8(token.ValueBytes);

                case DataTypeInternal.Int16:
                    return (uint)ReadInt16(token.ValueBytes);

                case DataTypeInternal.Int32:
                    return (uint)ReadInt32(token.ValueBytes);

                default:
                    throw ExceptionUtils.IntDeserializationFailure(token.DataTypeInternal);
            }
        }

        #endregion

        #region ushort typeInternal conversion

        public static explicit operator MsgPackToken(ushort value)
        {
            return WriteNonNegativeInteger(value);
        }

        public static explicit operator ushort(MsgPackToken token)
        {
            if (TryGetFixPositiveNumber(token.DataTypeInternal, out byte temp))
            {
                return temp;
            }

            if (TryGetNegativeNumber(token.DataTypeInternal, out sbyte tempInt8))
            {
                return (ushort)tempInt8;
            }

            switch (token.DataTypeInternal)
            {
                case DataTypeInternal.UInt8:
                    return ReadUInt8(token.ValueBytes);

                case DataTypeInternal.UInt16:
                    return ReadUInt16(token.ValueBytes);

                case DataTypeInternal.Int8:
                    return (ushort)ReadInt8(token.ValueBytes);

                case DataTypeInternal.Int16:
                    return (ushort)ReadInt16(token.ValueBytes);

                default:
                    throw ExceptionUtils.IntDeserializationFailure(token.DataTypeInternal);
            }
        }

        #endregion

        #region byte typeInternal conversion

        public static explicit operator MsgPackToken(byte value)
        {
            return WriteNonNegativeInteger(value);
        }

        public static explicit operator byte(MsgPackToken token)
        {
            if (TryGetFixPositiveNumber(token.DataTypeInternal, out byte temp))
            {
                return temp;
            }

            if (TryGetNegativeNumber(token.DataTypeInternal, out sbyte tempInt8))
            {
                return (byte)tempInt8;
            }

            switch (token.DataTypeInternal)
            {
                case DataTypeInternal.UInt8:
                    return ReadUInt8(token.ValueBytes);

                case DataTypeInternal.Int8:
                    return (byte)ReadInt8(token.ValueBytes);

                default:
                    throw ExceptionUtils.IntDeserializationFailure(token.DataTypeInternal);
            }
        }

        #endregion

        #region long typeInternal conversion

        public static explicit operator MsgPackToken(long value)
        {
            return WriteInteger(value);
        }

        public static explicit operator long(MsgPackToken token)
        {
            return TryGetInt64(token) ?? throw ExceptionUtils.IntDeserializationFailure(token.DataTypeInternal);
        }

        #endregion

        #region int typeInternal conversion

        public static explicit operator MsgPackToken(int value)
        {
            return WriteInteger(value);
        }

        public static explicit operator int(MsgPackToken token)
        {
            return TryGetInt32(token) ?? throw ExceptionUtils.IntDeserializationFailure(token.DataTypeInternal);
        }

        #endregion

        #region short typeInternal conversion

        public static explicit operator MsgPackToken(short value)
        {
            return WriteInteger(value);
        }

        public static explicit operator short(MsgPackToken token)
        {
            if (TryGetFixPositiveNumber(token.DataTypeInternal, out byte temp))
            {
                return temp;
            }

            if (TryGetNegativeNumber(token.DataTypeInternal, out sbyte tempInt8))
            {
                return tempInt8;
            }

            switch (token.DataTypeInternal)
            {
                case DataTypeInternal.UInt8:
                    return ReadUInt8(token.ValueBytes);

                case DataTypeInternal.UInt16:
                    var ushortValue = ReadUInt16(token.ValueBytes);
                    if (ushortValue <= short.MaxValue)
                    {
                        return (short)ushortValue;
                    }

                    throw ExceptionUtils.IntDeserializationFailure(token.DataTypeInternal);

                case DataTypeInternal.Int8:
                    return ReadInt8(token.ValueBytes);

                case DataTypeInternal.Int16:
                    return ReadInt16(token.ValueBytes);

                default:
                    throw ExceptionUtils.IntDeserializationFailure(token.DataTypeInternal);
            }
        }

        #endregion

        #region sbyte typeInternal conversion

        public static explicit operator MsgPackToken(sbyte value)
        {
            return WriteInteger(value);
        }

        public static explicit operator sbyte(MsgPackToken token)
        {
            if (TryGetFixPositiveNumber(token.DataTypeInternal, out byte temp))
            {
                return (sbyte)temp;
            }

            if (TryGetNegativeNumber(token.DataTypeInternal, out sbyte tempInt8))
            {
                return tempInt8;
            }

            if (token.DataTypeInternal == DataTypeInternal.Int8)
            {
                return ReadInt8(token.ValueBytes);
            }

            throw ExceptionUtils.IntDeserializationFailure(token.DataTypeInternal);
        }

        #endregion

        #region float typeInternal conversion

        public static explicit operator MsgPackToken(float value)
        {
            var binary = new FloatBinary(value);
            byte[] bytes;
            if (BitConverter.IsLittleEndian)
            {
                bytes = new[]
                {
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
                    binary.byte0,
                    binary.byte1,
                    binary.byte2,
                    binary.byte3
                };
            }

            return new MsgPackToken(DataTypeInternal.Single, bytes);
        }

        public static explicit operator float(MsgPackToken token)
        {
            if (token.DataTypeInternal == DataTypeInternal.Single)
            {
                return new FloatBinary(token.ValueBytes).value;
            }

            return TryGetInt32(token) ?? throw ExceptionUtils.BadTypeException(token.DataTypeInternal, DataTypeInternal.Single);
        }

        #endregion

        #region double typeInternal conversion

        public static explicit operator MsgPackToken(double value)
        {
            var binary = new DoubleBinary(value);
            byte[] bytes;
            if (BitConverter.IsLittleEndian)
            {
                bytes = new[]
                {
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

            return new MsgPackToken(DataTypeInternal.Double, bytes);
        }

        public static explicit operator double(MsgPackToken token)
        {
            if (token.DataTypeInternal != DataTypeInternal.Single && token.DataTypeInternal != DataTypeInternal.Double)
            {
                return TryGetInt64(token) ?? throw ExceptionUtils.BadTypeException(token.DataTypeInternal, DataTypeInternal.Single, DataTypeInternal.Double);
            }

            return token.DataTypeInternal == DataTypeInternal.Single
                ? new FloatBinary(token.ValueBytes).value
                : new DoubleBinary(token.ValueBytes).value;
        }

        #endregion

        #region MsgPackToken[] typeInternal conversion

        public static explicit operator MsgPackToken(MsgPackToken[] elements)
        {
            var length = elements.Length;
            DataTypeInternal dataTypeInternal;
            if (length <= 15)
            {
                dataTypeInternal = (DataTypeInternal) ((byte) DataTypeInternal.FixArray + length);
            }
            else if (length <= ushort.MaxValue)
            {
                dataTypeInternal = DataTypeInternal.Array16;
            }
            else
            {
                dataTypeInternal = DataTypeInternal.Array32;
            }

            return new MsgPackToken(dataTypeInternal, elements);
        }

        public static explicit operator MsgPackToken[] (MsgPackToken token)
        {
            return token.ArrayElements;
        }

        #endregion

        #region KeyValuePair<MsgPackToken, MsgPackToken>[] typeInternal conversion

        public static explicit operator MsgPackToken(KeyValuePair<MsgPackToken, MsgPackToken>[] elements)
        {
            var length = elements.Length;
            DataTypeInternal dataTypeInternal;
            if (length <= 15)
            {
                dataTypeInternal = (DataTypeInternal)((byte)DataTypeInternal.FixMap + length);
            }
            else if (length <= ushort.MaxValue)
            {
                dataTypeInternal = DataTypeInternal.Map16;
            }
            else
            {
                dataTypeInternal = DataTypeInternal.Map16;
            }

            return new MsgPackToken(dataTypeInternal, elements);
        }

        public static explicit operator KeyValuePair<MsgPackToken, MsgPackToken>[] (MsgPackToken token)
        {
            return token.MapElements;
        }

        #endregion

        private static MsgPackToken WriteInteger(long value)
        {
            if (value >= 0)
            {
                return WriteNonNegativeInteger((ulong)value);
            }

            return TryWriteSignedFixNum(value) ??
                TryWriteInt8(value) ??
                TryWriteInt16(value) ??
                TryWriteInt32(value) ??
                TryWriteInt64(value);
        }

        private static MsgPackToken TryWriteSignedFixNum(long value)
        {

            if ((value >= 0 && value < 128L) // positive fixnum
                || (value >= -32L && value <= -1L)) // negative fixnum
            {
                return new MsgPackToken((DataTypeInternal)unchecked((byte)value));
            }

            return null;
        }

        private static MsgPackToken TryWriteInt8(long value)
        {
            if (value > sbyte.MaxValue || value < sbyte.MinValue)
            {
                return null;
            }

            return new MsgPackToken(
                DataTypeInternal.Int8,
                new[]
                {
                    (byte) value
                });
        }

        private static MsgPackToken TryWriteInt16(long value)
        {
            if (value > short.MaxValue || value < short.MinValue)
            {
                return null;
            }

            return new MsgPackToken(
                DataTypeInternal.Int16,
                new[]
                {
                    (byte) ((value >> 8) & 0xff),
                    (byte) (value & 0xff)
                });
        }

        private static MsgPackToken TryWriteInt32(long value)
        {
            if (value > int.MaxValue || value < int.MinValue)
            {
                return null;
            }

            return new MsgPackToken(
                DataTypeInternal.Int32,
                new[]
                {
                    (byte) ((value >> 24) & 0xff),
                    (byte) ((value >> 16) & 0xff),
                    (byte) ((value >> 8) & 0xff),
                    (byte) (value & 0xff)
                });
        }

        private static MsgPackToken TryWriteInt64(long value)
        {
            return new MsgPackToken(
                DataTypeInternal.Int64,
                new[]
                {
                    (byte) ((value >> 56) & 0xff),
                    (byte) ((value >> 48) & 0xff),
                    (byte) ((value >> 40) & 0xff),
                    (byte) ((value >> 32) & 0xff),
                    (byte) ((value >> 24) & 0xff),
                    (byte) ((value >> 16) & 0xff),
                    (byte) ((value >> 8) & 0xff),
                    (byte) (value & 0xff)
                });
        }

        private static MsgPackToken WriteNonNegativeInteger(ulong value)
        {
            return TryWriteUnsignedFixNum(value) ??
                TryWriteUInt8(value) ??
                TryWriteUInt16(value) ??
                TryWriteUInt32(value) ??
                TryWriteUInt64(value);
        }

        private static MsgPackToken TryWriteUnsignedFixNum(ulong value)
        {
            // positive fixnum
            return value < 128L ? new MsgPackToken((DataTypeInternal)unchecked((byte)value)) : null;
        }

        private static MsgPackToken TryWriteUInt8(ulong value)
        {
            return value <= byte.MaxValue ? new MsgPackToken(DataTypeInternal.UInt8, new[] { (byte)value }) : null;
        }

        private static MsgPackToken TryWriteUInt16(ulong value)
        {
            return value <= ushort.MaxValue
                ? new MsgPackToken(
                    DataTypeInternal.UInt16,
                    new[]
                    {
                        (byte) ((value >> 8) & 0xff),
                        (byte) (value & 0xff)
                    })
                : null;
        }

        private static MsgPackToken TryWriteUInt32(ulong value)
        {
            return value <= uint.MaxValue
                ? new MsgPackToken(
                    DataTypeInternal.UInt32,
                    new[]
                    {
                        (byte) ((value >> 24) & 0xff),
                        (byte) ((value >> 16) & 0xff),
                        (byte) ((value >> 8) & 0xff),
                        (byte) (value & 0xff)
                    })
                : null;
        }

        private static MsgPackToken TryWriteUInt64(ulong value)
        {
            return new MsgPackToken(
                DataTypeInternal.UInt64,
                new[]
                {
                    (byte) ((value >> 56) & 0xff),
                    (byte) ((value >> 48) & 0xff),
                    (byte) ((value >> 40) & 0xff),
                    (byte) ((value >> 32) & 0xff),
                    (byte) ((value >> 24) & 0xff),
                    (byte) ((value >> 16) & 0xff),
                    (byte) ((value >> 8) & 0xff),
                    (byte) (value & 0xff),
                });
        }

        private static MsgPackToken CreateNullToken()
        {
            return new MsgPackToken(DataTypeInternal.Null);
        }

        private static bool TryGetFixstrLength(DataTypeInternal typeInternal, out uint length)
        {
            length = typeInternal - DataTypeInternal.FixStr;
            return typeInternal.GetHighBits(3) == DataTypeInternal.FixStr.GetHighBits(3);
        }

        private static int? TryGetInt32(MsgPackToken token)
        {
            if (TryGetFixPositiveNumber(token.DataTypeInternal, out byte temp))
            {
                return temp;
            }

            if (TryGetNegativeNumber(token.DataTypeInternal, out sbyte tempInt8))
            {
                return tempInt8;
            }

            switch (token.DataTypeInternal)
            {
                case DataTypeInternal.UInt8:
                    return ReadUInt8(token.ValueBytes);

                case DataTypeInternal.UInt16:
                    return ReadUInt16(token.ValueBytes);

                case DataTypeInternal.UInt32:
                    var uintValue = ReadUInt32(token.ValueBytes);
                    if (uintValue <= int.MaxValue)
                    {
                        return (int)uintValue;
                    }
                    return null;

                case DataTypeInternal.Int8:
                    return ReadInt8(token.ValueBytes);

                case DataTypeInternal.Int16:
                    return ReadInt16(token.ValueBytes);

                case DataTypeInternal.Int32:
                    return ReadInt32(token.ValueBytes);

                default:
                    return null;
            }
        }

        private static long? TryGetInt64(MsgPackToken token)
        {
            if (TryGetFixPositiveNumber(token.DataTypeInternal, out byte temp))
            {
                return temp;
            }

            if (TryGetNegativeNumber(token.DataTypeInternal, out sbyte tempInt8))
            {
                return tempInt8;
            }

            switch (token.DataTypeInternal)
            {
                case DataTypeInternal.UInt8:
                    return ReadUInt8(token.ValueBytes);

                case DataTypeInternal.UInt16:
                    return ReadUInt16(token.ValueBytes);

                case DataTypeInternal.UInt32:
                    return ReadUInt32(token.ValueBytes);

                case DataTypeInternal.UInt64:
                    var ulongValue = ReadUInt64(token.ValueBytes);
                    if (ulongValue <= long.MaxValue)
                    {
                        return (long)ulongValue;
                    }
                    return null;

                case DataTypeInternal.Int8:
                    return ReadInt8(token.ValueBytes);

                case DataTypeInternal.Int16:
                    return ReadInt16(token.ValueBytes);

                case DataTypeInternal.Int32:
                    return ReadInt32(token.ValueBytes);

                case DataTypeInternal.Int64:
                    return ReadInt64(token.ValueBytes);

                default:
                    return null;
            }
        }

        private static bool TryGetFixPositiveNumber(DataTypeInternal typeInternal, out byte temp)
        {
            temp = (byte)typeInternal;
            return typeInternal.GetHighBits(1) == DataTypeInternal.PositiveFixNum.GetHighBits(1);
        }

        private static bool TryGetNegativeNumber(DataTypeInternal typeInternal, out sbyte temp)
        {
            temp = (sbyte)((byte)typeInternal - 1 - byte.MaxValue);

            return typeInternal.GetHighBits(3) == DataTypeInternal.NegativeFixNum.GetHighBits(3);
        }


        internal static sbyte ReadInt8(byte[] bytes)
        {
            if (bytes[0] <= sbyte.MaxValue)
                return (sbyte)bytes[0];

            return (sbyte)(bytes[0] - byte.MaxValue - 1);
        }

        internal static byte ReadUInt8(byte[] bytes)
        {
            return bytes[0];
        }

        internal static ushort ReadUInt16(byte[] bytes)
        {
            return (ushort)((bytes[0] << 8) + bytes[1]);
        }

        internal static short ReadInt16(byte[] bytes)
        {
            var temp = ReadUInt16(bytes);
            if (temp <= short.MaxValue)
                return (short)temp;

            return (short)(temp - 1 - ushort.MaxValue);
        }

        internal static int ReadInt32(byte[] bytes)
        {
            var temp = ReadUInt32(bytes);
            if (temp <= int.MaxValue)
                return (int)temp;

            return (int)(temp - 1 - uint.MaxValue);
        }

        internal static uint ReadUInt32(byte[] bytes)
        {
            var temp = (uint)(bytes[0] << 24);
            temp += (uint)bytes[1] << 16;
            temp += (uint)bytes[2] << 8;
            temp += bytes[3];

            return temp;
        }

        internal static ulong ReadUInt64(byte[] bytes)
        {
            var temp = (ulong)bytes[0] << 56;
            temp += (ulong)bytes[1] << 48;
            temp += (ulong)bytes[2] << 40;
            temp += (ulong)bytes[3] << 32;
            temp += (ulong)bytes[4] << 24;
            temp += (ulong)bytes[5] << 16;
            temp += (ulong)bytes[6] << 8;
            temp += bytes[7];

            return temp;
        }

        internal static long ReadInt64(byte[] bytes)
        {
            var temp = ReadUInt64(bytes);
            if (temp <= long.MaxValue)
                return (long)temp;

            return (long)(temp - 1 - ulong.MaxValue);
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

            public FloatBinary(byte[] bytes)
            {
                value = 0;
                if (BitConverter.IsLittleEndian)
                {
                    byte0 = bytes[3];
                    byte1 = bytes[2];
                    byte2 = bytes[1];
                    byte3 = bytes[0];
                }
                else
                {
                    byte0 = bytes[0];
                    byte1 = bytes[1];
                    byte2 = bytes[2];
                    byte3 = bytes[3];
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

            public DoubleBinary(byte[] bytes)
            {
                value = 0;
                if (BitConverter.IsLittleEndian)
                {
                    byte0 = bytes[7];
                    byte1 = bytes[6];
                    byte2 = bytes[5];
                    byte3 = bytes[4];
                    byte4 = bytes[3];
                    byte5 = bytes[2];
                    byte6 = bytes[1];
                    byte7 = bytes[0];
                }
                else
                {
                    byte0 = bytes[0];
                    byte1 = bytes[1];
                    byte2 = bytes[2];
                    byte3 = bytes[3];
                    byte4 = bytes[4];
                    byte5 = bytes[5];
                    byte6 = bytes[6];
                    byte7 = bytes[7];
                }
            }
        }
    }
}
