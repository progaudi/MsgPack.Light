using System;

namespace ProGaudi.MsgPack.Light
{
    public interface IMsgPackReader
    {
        DataTypes ReadDataType();

        byte ReadByte();

        ArraySegment<byte> ReadBytes(uint length);
    }
}