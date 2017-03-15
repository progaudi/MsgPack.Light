namespace ProGaudi.MsgPack.Light.Converters
{
    internal class NullTokenConverter : IMsgPackTokenConverter<object>
    {
        public void Initialize(MsgPackContext context)
        {
        }

        public MsgPackToken ConvertFrom(object value)
        {
            return null;
        }

        public object ConvertTo(MsgPackToken token)
        {
            if (token.DataType == DataTypes.Null)
            {
                return null;
            }

            throw ExceptionUtils.BadTypeException(token.DataType, DataTypes.Null);
        }
    }
}