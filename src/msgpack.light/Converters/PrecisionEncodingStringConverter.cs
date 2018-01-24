using System.Text;

namespace ProGaudi.MsgPack.Light.Converters
{
    public class PrecisionEncodingStringConverter : StringConverter
    {
        public PrecisionEncodingStringConverter(Encoding encoding)
            : base(encoding)
        {
        }

        public override int GuessByteArrayLength(string value)
        {
            if (value == null) return 1;
            
            return Encoding.GetByteCount(value);
        }
    }
}
