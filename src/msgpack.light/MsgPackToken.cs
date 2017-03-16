namespace ProGaudi.MsgPack.Light
{
    public class MsgPackToken
    {
        public MsgPackToken(byte[] rawBytes)
        {
            RawBytes = rawBytes;
        }

        internal byte[] RawBytes { get; set; }
    }
}