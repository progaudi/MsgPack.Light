namespace ProGaudi.MsgPack.Light.Converters
{
    public class MsgPackTokenConverter : IMsgPackConverter<MsgPackToken>
    {
        public void Initialize(MsgPackContext context)
        {
        }

        public void Write(MsgPackToken value, IMsgPackWriter writer)
        {
            writer.Write(value.RawBytes);
        }

        public MsgPackToken Read(IMsgPackReader reader)
        {
            var rawBytes = reader.ReadToken();
            return new MsgPackToken(rawBytes);
        }
    }
}
