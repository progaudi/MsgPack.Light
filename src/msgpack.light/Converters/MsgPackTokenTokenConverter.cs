namespace ProGaudi.MsgPack.Light.Converters
{
    public class MsgPackTokenTokenConverter :
        IMsgPackTokenConverter<MsgPackToken>,
        IMsgPackTokenConverter<bool>,
        IMsgPackTokenConverter<string>,
        IMsgPackTokenConverter<byte[]>,
        IMsgPackTokenConverter<byte>,
        IMsgPackTokenConverter<sbyte>,
        IMsgPackTokenConverter<short>,
        IMsgPackTokenConverter<ushort>,
        IMsgPackTokenConverter<int>,
        IMsgPackTokenConverter<uint>,
        IMsgPackTokenConverter<long>,
        IMsgPackTokenConverter<ulong>,
        IMsgPackTokenConverter<float>,
        IMsgPackTokenConverter<double>
    {
        private bool _strictParseOfFloat;

        public MsgPackTokenTokenConverter(bool strictParseOfFloat)
        {
            _strictParseOfFloat = strictParseOfFloat;
        }

        public void Initialize(MsgPackContext context)
        {
        }

        public MsgPackToken ConvertFrom(MsgPackToken value)
        {
            return value;
        }

        MsgPackToken IMsgPackTokenConverter<MsgPackToken>.ConvertTo(MsgPackToken token)
        {
            return token;
        }

        public MsgPackToken ConvertFrom(bool value)
        {
            return (MsgPackToken)value;
        }

        bool IMsgPackTokenConverter<bool>.ConvertTo(MsgPackToken token)
        {
            return (bool)token;
        }

        public MsgPackToken ConvertFrom(string value)
        {
            return (MsgPackToken)value;
        }

        string IMsgPackTokenConverter<string>.ConvertTo(MsgPackToken token)
        {
            return (string)token;
        }

        public MsgPackToken ConvertFrom(byte[] value)
        {
            return (MsgPackToken)value;
        }

        byte[] IMsgPackTokenConverter<byte[]>.ConvertTo(MsgPackToken token)
        {
            return (byte[])token;
        }

        public MsgPackToken ConvertFrom(byte value)
        {
            return (MsgPackToken)value;
        }

        byte IMsgPackTokenConverter<byte>.ConvertTo(MsgPackToken token)
        {
            return (byte)token;
        }

        public MsgPackToken ConvertFrom(sbyte value)
        {
            return (MsgPackToken)value;
        }

        sbyte IMsgPackTokenConverter<sbyte>.ConvertTo(MsgPackToken token)
        {
            return (sbyte)token;
        }

        public MsgPackToken ConvertFrom(short value)
        {
            return (MsgPackToken)value;
        }

        short IMsgPackTokenConverter<short>.ConvertTo(MsgPackToken token)
        {
            return (short)token;
        }

        public MsgPackToken ConvertFrom(ushort value)
        {
            return (MsgPackToken)value;
        }

        ushort IMsgPackTokenConverter<ushort>.ConvertTo(MsgPackToken token)
        {
            return (ushort)token;
        }

        public MsgPackToken ConvertFrom(int value)
        {
            return (MsgPackToken)value;
        }

        int IMsgPackTokenConverter<int>.ConvertTo(MsgPackToken token)
        {
            return (int)token;
        }

        public MsgPackToken ConvertFrom(uint value)
        {
            return (MsgPackToken)value;
        }

        uint IMsgPackTokenConverter<uint>.ConvertTo(MsgPackToken token)
        {
            return (uint)token;
        }

        public MsgPackToken ConvertFrom(long value)
        {
            return (MsgPackToken)value;
        }

        long IMsgPackTokenConverter<long>.ConvertTo(MsgPackToken token)
        {
            return (long)token;
        }

        public MsgPackToken ConvertFrom(ulong value)
        {
            return (MsgPackToken)value;
        }

        ulong IMsgPackTokenConverter<ulong>.ConvertTo(MsgPackToken token)
        {
            return (ulong)token;
        }

        public MsgPackToken ConvertFrom(float value)
        {
            return (MsgPackToken)value;
        }

        float IMsgPackTokenConverter<float>.ConvertTo(MsgPackToken token)
        {
            if (_strictParseOfFloat && token.DataTypeInternal != DataTypeInternal.Single)
            {
                throw ExceptionUtils.BadTypeException(token.DataTypeInternal, DataTypeInternal.Single);
            }

            return (float)token;
        }

        public MsgPackToken ConvertFrom(double value)
        {
            return (MsgPackToken)value;
        }

        double IMsgPackTokenConverter<double>.ConvertTo(MsgPackToken token)
        {
            if (_strictParseOfFloat && token.DataTypeInternal != DataTypeInternal.Single && token.DataTypeInternal != DataTypeInternal.Double)
            {
                throw ExceptionUtils.BadTypeException(token.DataTypeInternal, DataTypeInternal.Single, DataTypeInternal.Double);
            }

            return (double)token;
        }
    }
}