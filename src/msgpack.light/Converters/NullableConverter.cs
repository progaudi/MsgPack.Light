using System.IO;

namespace ProGaudi.MsgPack.Light.Converters
{
    public class NullableConverter<T> : IMsgPackConverter<T?>
        where T : struct
    {
        private MsgPackContext _context;

        private IMsgPackConverter<T> _converter;

        public void Initialize(MsgPackContext context)
        {
            _context = context;
            _converter = context.GetConverter<T>();
        }
        
        public MsgPackToken Write(T? value)
        {
            return value.HasValue ? _converter.Write(value.Value) : _context.NullConverter.Write(null);
        }

        public T? Read(MsgPackToken token)
        {
            if (token.DataType == DataTypes.Null)
                return null;

            return _converter.Read(token);
        }
    }
}
