namespace ProGaudi.MsgPack.Light
{
    internal interface IMsgPackWriter
    {
        void Write(DataTypeInternal dataTypeInternal);

        void Write(byte value);

        void Write(byte[] array);
    }
}
