namespace ProGaudi.MsgPack.Light.Converters
{
    internal class NullConverter : IMsgPackConverter<object>
    {
        public void Initialize(MsgPackContext context)
        {
        }

        public MsgPackToken Write(object value)
        {
            return null;
        }

        public object Read(MsgPackToken token)
        {
            if (token.DataType == DataTypes.Null)
            {
                return null;
            }

            throw ExceptionUtils.BadTypeException(token.DataType, DataTypes.Null);
        }
    }
}