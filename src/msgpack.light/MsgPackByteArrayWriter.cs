using System;
using System.Buffers;

namespace ProGaudi.MsgPack.Light
{
    internal class MsgPackByteArrayWriter : MsgPackWriterBase, IDisposable
    {
        public MsgPackByteArrayWriter()
        {
            //_buffer = ArrayPool<byte>.Shared.Rent()
        }

        public override void Write(DataTypes dataType)
        {
            Write((byte) dataType);
        }

        public override void Write(byte value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] array)
        {
            throw new NotImplementedException();
        }

        public override byte[] ToArray()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
