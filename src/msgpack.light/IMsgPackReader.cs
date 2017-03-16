using System;

namespace ProGaudi.MsgPack.Light
{
    internal interface IMsgPackReader
    {
        DataTypeInternal ReadDataType();

        byte ReadByte();

        ArraySegment<byte> ReadBytes(uint length);
    }
}