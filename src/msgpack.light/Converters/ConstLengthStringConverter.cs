using System.Text;

namespace ProGaudi.MsgPack.Light.Converters
{
    public class ConstLengthStringConverter : PrecisionEncodingStringConverter
    {
        private readonly int _charLength;

        public ConstLengthStringConverter(Encoding encoding, int charLength)
            : base(encoding)
        {
            _charLength = charLength;
        }

        public override int GuessByteArrayLength(string value)
        {
            if (value == null) return 1;

            return value.Length * _charLength;
        }
    }
}
