namespace ProGaudi.MsgPack.Light.Converters
{
    internal class NullConverter : IMsgPackConverter<object>
    {
        public void Initialize(MsgPackContext context)
        {
        }

        public void Write(object value, IMsgPackWriter writer)
        {
            writer.Write(DataTypes.Null);
        }

        public object Read(IMsgPackReader reader)
        {
            var type = reader.ReadDataType();
            if (type == DataTypes.Null)
                return null;

            throw ExceptionUtils.BadTypeException(type, DataTypes.Null);
        }

        public int GuessByteArrayLength(object value) => 1;

        public bool HasFixedLength => true;
    }
}