using System.IO;

namespace ProGaudi.MsgPack.Light.Converters
{
    public class NullableTokenConverter<T> : IMsgPackTokenConverter<T?>
        where T : struct
    {
        private MsgPackContext _context;

        private IMsgPackTokenConverter<T> _tokenConverter;

        public void Initialize(MsgPackContext context)
        {
            _context = context;
            _tokenConverter = context.GetConverter<T>();
        }
        
        public MsgPackToken ConvertFrom(T? value)
        {
            return value.HasValue ? _tokenConverter.ConvertFrom(value.Value) : _context.NullTokenConverter.ConvertFrom(null);
        }

        public T? ConvertTo(MsgPackToken token)
        {
            if (token.DataTypeInternal == DataTypeInternal.Null)
                return null;

            return _tokenConverter.ConvertTo(token);
        }
    }
}
