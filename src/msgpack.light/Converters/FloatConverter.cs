using System;
using System.Runtime.InteropServices;

namespace MsgPack.Light.Converters
{
    internal class FloatConverter : IMsgPackConverter<float>, IMsgPackConverter<double>
    {
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