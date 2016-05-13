using System;
using System.IO;

using MsgPack.Light.Converters;

namespace MsgPack.Light
{
    internal class MsgPackMemoryStreamWriter : IMsgPackWriter, IDisposable
    {
        private readonly MemoryStream _stream;

        private readonly bool _disposeStream;

        public MsgPackMemoryStreamWriter(MemoryStream stream, bool disposeStream = true)
        {
            _stream = stream;
            _disposeStream = disposeStream;
        }

        public void Write(DataTypes dataType)
        {
            _stream.WriteByte((byte) dataType);
        }

        public void Write(byte value)
        {
            _stream.WriteByte(value);
        }

        public void Write(byte[] array)
        {
            _stream.Write(array, 0, array.Length);
        }

        public void Dispose()
        {
            if (_disposeStream)
                _stream.Dispose();
        }

        public void WriteArrayHeader(uint length)
        {
            if (length <= 15)
            {
                IntConverter.TryWriteUInt8((byte) ((byte) DataTypes.FixArray + length), this);
                return;
            }

            if (length <= ushort.MaxValue)
            {
                Write(DataTypes.Array16);
                IntConverter.TryWriteUInt16((ushort) length, this);
            }
            else
            {
                Write(DataTypes.Array32);
                IntConverter.TryWriteUInt32(length, this);
            }
        }

        public void WriteMapHeader(uint length)
        {
            if (length <= 15)
            {
                IntConverter.TryWriteUInt8((byte) ((byte) DataTypes.FixMap + length), this);
                return;
            }

            if (length <= ushort.MaxValue)
            {
                Write(DataTypes.Map16);
                IntConverter.TryWriteUInt16((ushort) length, this);
            }
            else
            {
                Write(DataTypes.Map32);
                IntConverter.TryWriteUInt32((uint) length, this);
            }
        }
    }
}
