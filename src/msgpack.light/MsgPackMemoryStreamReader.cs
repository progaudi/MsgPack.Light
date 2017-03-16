using System;
using System.Collections.Generic;
using System.IO;

namespace ProGaudi.MsgPack.Light
{
    internal class MsgPackMemoryStreamReader : BaseMsgPackReader, IDisposable
    {
        private readonly MemoryStream _stream;

        private readonly bool _disposeStream;

        private List<byte> _bytesGatheringBuffer = null;

        public MsgPackMemoryStreamReader(MemoryStream stream, bool disposeStream = true)
        {
            _stream = stream;
            _disposeStream = disposeStream;
        }

        public override byte ReadByte()
        {
            var temp = _stream.ReadByte();
            if (temp == -1)
            {
                throw ExceptionUtils.NotEnoughBytes(0, 1);
            }

            var result = (byte) temp;
            _bytesGatheringBuffer?.Add(result);

            return result;
        }

        public override ArraySegment<byte> ReadBytes(uint length)
        {
            var buffer = ReadBytesInternal(length);
            _bytesGatheringBuffer?.AddRange(buffer);
            return new ArraySegment<byte>(buffer, 0, buffer.Length);
        }

        public override void Seek(long offset, SeekOrigin origin)
        {
            if (_bytesGatheringBuffer != null)
            {
                var buffer = ReadBytesInternal((uint) offset);
                _bytesGatheringBuffer.AddRange(buffer);
            }
            else
            {
                _stream.Seek(offset, origin);
            }
        }

        public void Dispose()
        {
            if (_disposeStream)
                _stream.Dispose();
        }

        protected override IList<byte> StopTokenGathering()
        {
            var result = _bytesGatheringBuffer;
            _bytesGatheringBuffer = null;
            return result;
        }

        protected override void StartTokenGathering()
        {
            _bytesGatheringBuffer = new List<byte>();
        }

        private byte[] ReadBytesInternal(uint length)
        {
            var buffer = new byte[length];
            var read = _stream.Read(buffer, 0, buffer.Length);
            if (read < buffer.Length)
                throw ExceptionUtils.NotEnoughBytes(read, buffer.Length);
            return buffer;
        }
    }
}
