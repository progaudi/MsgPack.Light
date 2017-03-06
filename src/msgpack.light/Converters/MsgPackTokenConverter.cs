namespace ProGaudi.MsgPack.Light.Converters
{
    public class MsgPackTokenConverter :
        IMsgPackConverter<MsgPackToken>,
        IMsgPackConverter<bool>,
        IMsgPackConverter<string>,
        IMsgPackConverter<byte[]>,
        IMsgPackConverter<byte>,
        IMsgPackConverter<sbyte>,
        IMsgPackConverter<short>,
        IMsgPackConverter<ushort>,
        IMsgPackConverter<int>,
        IMsgPackConverter<uint>,
        IMsgPackConverter<long>,
        IMsgPackConverter<ulong>,
        IMsgPackConverter<float>,
        IMsgPackConverter<double>
    {
        private bool _strictParseOfFloat;

        public MsgPackTokenConverter(bool strictParseOfFloat)
        {
            _strictParseOfFloat = strictParseOfFloat;
        }

        public void Initialize(MsgPackContext context)
        {
        }

        public MsgPackToken Write(MsgPackToken value)
        {
            return value;
        }

        MsgPackToken IMsgPackConverter<MsgPackToken>.Read(MsgPackToken token)
        {
            return token;
        }

        public MsgPackToken Write(bool value)
        {
            return (MsgPackToken)value;
        }

        bool IMsgPackConverter<bool>.Read(MsgPackToken token)
        {
            return (bool)token;
        }

        public MsgPackToken Write(string value)
        {
            return (MsgPackToken)value;
        }

        string IMsgPackConverter<string>.Read(MsgPackToken token)
        {
            return (string)token;
        }

        public MsgPackToken Write(byte[] value)
        {
            return (MsgPackToken)value;
        }

        byte[] IMsgPackConverter<byte[]>.Read(MsgPackToken token)
        {
            return (byte[])token;
        }

        public MsgPackToken Write(byte value)
        {
            return (MsgPackToken)value;
        }

        byte IMsgPackConverter<byte>.Read(MsgPackToken token)
        {
            return (byte)token;
        }

        public MsgPackToken Write(sbyte value)
        {
            return (MsgPackToken)value;
        }

        sbyte IMsgPackConverter<sbyte>.Read(MsgPackToken token)
        {
            return (sbyte)token;
        }

        public MsgPackToken Write(short value)
        {
            return (MsgPackToken)value;
        }

        short IMsgPackConverter<short>.Read(MsgPackToken token)
        {
            return (short)token;
        }

        public MsgPackToken Write(ushort value)
        {
            return (MsgPackToken)value;
        }

        ushort IMsgPackConverter<ushort>.Read(MsgPackToken token)
        {
            return (ushort)token;
        }

        public MsgPackToken Write(int value)
        {
            return (MsgPackToken)value;
        }

        int IMsgPackConverter<int>.Read(MsgPackToken token)
        {
            return (int)token;
        }

        public MsgPackToken Write(uint value)
        {
            return (MsgPackToken)value;
        }

        uint IMsgPackConverter<uint>.Read(MsgPackToken token)
        {
            return (uint)token;
        }

        public MsgPackToken Write(long value)
        {
            return (MsgPackToken)value;
        }

        long IMsgPackConverter<long>.Read(MsgPackToken token)
        {
            return (long)token;
        }

        public MsgPackToken Write(ulong value)
        {
            return (MsgPackToken)value;
        }

        ulong IMsgPackConverter<ulong>.Read(MsgPackToken token)
        {
            return (ulong)token;
        }

        public MsgPackToken Write(float value)
        {
            return (MsgPackToken)value;
        }

        float IMsgPackConverter<float>.Read(MsgPackToken token)
        {
            if (_strictParseOfFloat && token.DataType != DataTypes.Single)
            {
                throw ExceptionUtils.BadTypeException(token.DataType, DataTypes.Single);
            }

            return (float)token;
        }

        public MsgPackToken Write(double value)
        {
            return (MsgPackToken)value;
        }

        double IMsgPackConverter<double>.Read(MsgPackToken token)
        {
            if (_strictParseOfFloat && token.DataType != DataTypes.Single && token.DataType != DataTypes.Double)
            {
                throw ExceptionUtils.BadTypeException(token.DataType, DataTypes.Single, DataTypes.Double);
            }

            return (double)token;
        }
    }
}