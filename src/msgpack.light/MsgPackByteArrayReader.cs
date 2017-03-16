using System;
using System.IO;

namespace ProGaudi.MsgPack.Light
{
    internal class MsgPackByteArrayReader : IMsgPackReader
    {
        private readonly byte[] _data;

        private uint _offset;

        public MsgPackByteArrayReader(byte[] data)
        {
            _data = data;
            _offset = 0;
        }

        public DataTypeInternal ReadDataType()
        {
            return (DataTypeInternal)ReadByte();
        }

        public byte ReadByte()
        {
            return _data[_offset++];
        }

        public ArraySegment<byte> ReadBytes(uint length)
        {
            _offset += length;
            return new ArraySegment<byte>(_data, (int)(_offset - length), (int)length);
        }
    }
}