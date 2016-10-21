using System;
using System.IO;

namespace ProGaudi.MsgPack.Light
{
    internal class MsgPackMemoryStreamReader : BaseMsgPackReader, IDisposable
    {
        private readonly MemoryStream _stream;

        private readonly bool _disposeStream;

        public MsgPackMemoryStreamReader(MemoryStream stream, bool disposeStream = true)
        {
            _stream = stream;
            _disposeStream = disposeStream;
        }

        public override byte ReadByte()
        {
            var temp = _stream.ReadByte();
            if (temp == -1)
                throw ExceptionUtils.NotEnoughBytes(0, 1);

            return (byte)temp;
        }

        public override ArraySegment<byte> ReadBytes(uint length)
        {
            var buffer = new byte[length];
            var read = _stream.Read(buffer, 0, buffer.Length);
            if (read < buffer.Length)
                throw ExceptionUtils.NotEnoughBytes(read, buffer.Length);
            return new ArraySegment<byte>(buffer, 0, buffer.Length);
        }

        public override void Seek(long offset, SeekOrigin origin)
        {
            _stream.Seek(offset, origin);
        }

        public void Dispose()
        {
            if (_disposeStream)
                _stream.Dispose();
        }
    }
}
