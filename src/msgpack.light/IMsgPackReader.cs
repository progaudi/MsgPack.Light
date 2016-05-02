using System;
using System.IO;

namespace MsgPack.Light
{
    public interface IMsgPackReader
    {
        DataTypes ReadDataType();

        byte ReadByte();

        ArraySegment<byte> ReadBytes(uint length);

        void Seek(int offset, SeekOrigin origin);

        uint? ReadArrayLength();

        uint? ReadMapLength();
    }
}