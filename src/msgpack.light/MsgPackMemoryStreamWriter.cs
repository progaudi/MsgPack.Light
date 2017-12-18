using System;
using System.IO;

namespace ProGaudi.MsgPack.Light
{
    internal class MsgPackMemoryStreamWriter : MsgPackWriterBase, IDisposable
    {
        private readonly MemoryStream _stream;

        private readonly bool _disposeStream;

        public MsgPackMemoryStreamWriter(MemoryStream stream, bool disposeStream = true)
        {
            _stream = stream;
            _disposeStream = disposeStream;
        }

        public override void Write(DataTypes dataType)
        {
            _stream.WriteByte((byte) dataType);
        }

        public override void Write(byte value)
        {
            _stream.WriteByte(value);
        }

        public override void Write(byte[] array)
        {
            _stream.Write(array, 0, array.Length);
        }

        public override byte[] ToArray()
        {
            return _stream.ToArray();
        }

        public void Dispose()
        {
            if (_disposeStream)
                _stream.Dispose();
        }
    }
}
