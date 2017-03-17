using System;
using System.Collections.Generic;
using System.IO;

namespace ProGaudi.MsgPack.Light
{
    internal class MsgPackMemoryStreamReader : BaseMsgPackReader, IDisposable
    {
        private readonly MemoryStream _stream;

        private readonly bool _disposeStream;

        private readonly List<(byte, byte[])> _bytesGatheringBuffer = new List<(byte, byte[])>();

        private bool _bytesGatheringInProgress;

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
            if (_bytesGatheringInProgress)
            {
                _bytesGatheringBuffer.Add((result, (byte[]) null));
            }

            return result;
        }

        public override ArraySegment<byte> ReadBytes(uint length)
        {
            var buffer = ReadBytesInternal(length);
            if (_bytesGatheringInProgress)
            {
                _bytesGatheringBuffer.Add((0, buffer));
            }
            
            return new ArraySegment<byte>(buffer, 0, buffer.Length);
        }

        public override void Seek(long offset, SeekOrigin origin)
        {
            if (_bytesGatheringInProgress)
            {
                var buffer = ReadBytesInternal((uint)offset);
                _bytesGatheringBuffer.Add((0, buffer));
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
            var result = new List<byte>();
            foreach (var tuple in _bytesGatheringBuffer)
            {
                if (tuple.Item2 != null)
                {
                    result.AddRange(tuple.Item2);
                }
                else
                {
                    result.Add(tuple.Item1);
                }
            }

            _bytesGatheringInProgress = false;
            return result;
        }

        protected override void StartTokenGathering()
        {
            _bytesGatheringInProgress = true;
            _bytesGatheringBuffer.Clear();
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
