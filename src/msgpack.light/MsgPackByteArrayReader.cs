using System;
using System.IO;

namespace MsgPack.Light
{
    internal class MsgPackByteArrayReader : BaseMsgPackReader
    {
        private readonly byte[] _data;

        private uint _offset;

        public MsgPackByteArrayReader(byte[] data)
        {
            _data = data;
            _offset = 0;
        }

        public override byte ReadByte()
        {
            return _data[_offset++];
        }

        public override ArraySegment<byte> ReadBytes(uint length)
        {
            _offset += length;
            return new ArraySegment<byte>(_data, (int) (_offset - length), (int) length);
        }

        public override void Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    _offset = (uint)offset;
                    break;
                case SeekOrigin.Current:
                    _offset = (uint)(_offset + offset);
                    break;
                case SeekOrigin.End:
                    _offset = (uint) (_data.Length + offset);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(origin), origin, null);
            }
        }
    }
}
