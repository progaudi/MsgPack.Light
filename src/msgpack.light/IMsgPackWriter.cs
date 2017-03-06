namespace ProGaudi.MsgPack.Light
{
    public interface IMsgPackWriter
    {
        void Write(DataTypes dataType);

        void Write(byte value);

        void Write(byte[] array);
    }
}
