using System;
using System.Collections.Generic;

using MsgPack.Light.Converters;

namespace MsgPack.Light
{
    internal class MsgPackByteArrayWriter : IMsgPackWriter
    {
        private readonly List<byte[]> buffers = new List<byte[]>();

        private int bufferLength = 0;

        public void Write(DataTypes dataType)
        {
            Write((byte) dataType);
        }

        public void Write(byte value)
        {
            Write(new[] { value });
        }

        public void Write(byte[] array)
        {
            buffers.Add(array);
            bufferLength += array.Length;
        }

        public void WriteArrayHeader(uint length)
        {
            if (length <= 15)
            {
                IntConverter.WriteValue((byte) ((byte) DataTypes.FixArray + length), this);
                return;
            }

            if (length <= ushort.MaxValue)
            {
                Write(DataTypes.Array16);
                IntConverter.WriteValue((ushort) length, this);
            }
            else
            {
                Write(DataTypes.Array32);
                IntConverter.WriteValue((uint) length, this);
            }
        }

        public void WriteMapHeaderAndLength(uint length)
        {
            if (length <= 15)
            {
                IntConverter.WriteValue((byte) ((byte) DataTypes.FixMap + length), this);
                return;
            }

            if (length <= ushort.MaxValue)
            {
                Write(DataTypes.Map16);
                IntConverter.WriteValue((ushort) length, this);
            }
            else
            {
                Write(DataTypes.Map32);
                IntConverter.WriteValue((uint) length, this);
            }
        }

        public byte[] ToArray()
        {
            if (bufferLength == 0)
            {
                return new byte[0];
            }

            if (buffers.Count == 1)
            {
                return buffers[0];
            }

            var result = new byte[bufferLength];

            var offset = 0;
            foreach (var buffer in buffers)
            {
                Array.Copy(buffer, 0, result, offset, buffer.Length);
                offset += buffer.Length;
            }

            return result;
        }
    }
}
