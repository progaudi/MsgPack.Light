using System.IO;

using ProGaudi.MsgPack.Light.Converters;

namespace ProGaudi.MsgPack.Light
{
    public class MsgPackToken
    {
        private static readonly IMsgPackConverter<bool> BoolConverter = new BoolConverter();
        private static readonly IMsgPackConverter<string> StringConverter = new StringConverter();
        private static readonly NumberConverter NumberConverter = new NumberConverter(false);

        public MsgPackToken(byte[] rawBytes)
        {
            RawBytes = rawBytes;
        }

        internal byte[] RawBytes { get; }

        #region Bool typeInternal conversion

        public static explicit operator MsgPackToken(bool b)
        {
            return CastValueToToken(b, BoolConverter);
        }

        public static explicit operator bool(MsgPackToken token)
        {
            return CastTokenToValue(token, BoolConverter);
        }

        #endregion

        #region String typeInternal conversion

        public static explicit operator MsgPackToken(string str)
        {
            return CastValueToToken(str, StringConverter);
        }

        public static explicit operator string(MsgPackToken token)
        {
            return CastTokenToValue(token, StringConverter);
        }

        #endregion

        #region byte[] typeInternal conversion

        public static explicit operator MsgPackToken(byte[] data)
        {
            return new MsgPackToken(data);
        }

        public static explicit operator byte[] (MsgPackToken token)
        {
            return token.RawBytes;
        }

        #endregion

        #region ulong typeInternal conversion

        public static explicit operator MsgPackToken(ulong value)
        {
            return CastValueToToken(value, NumberConverter);
        }

        public static explicit operator ulong(MsgPackToken token)
        {
            return CastTokenToValue<ulong>(token, NumberConverter);
        }

        #endregion

        #region uint typeInternal conversion

        public static explicit operator MsgPackToken(uint value)
        {
            return CastValueToToken(value, NumberConverter);
        }

        public static explicit operator uint(MsgPackToken token)
        {
            return CastTokenToValue<uint>(token, NumberConverter);
        }

        #endregion

        #region ushort typeInternal conversion

        public static explicit operator MsgPackToken(ushort value)
        {
            return CastValueToToken(value, NumberConverter);
        }

        public static explicit operator ushort(MsgPackToken token)
        {
            return CastTokenToValue<ushort>(token, NumberConverter);
        }

        #endregion

        #region byte typeInternal conversion

        public static explicit operator MsgPackToken(byte value)
        {
            return CastValueToToken(value, NumberConverter);
        }

        public static explicit operator byte(MsgPackToken token)
        {
            return CastTokenToValue<byte>(token, NumberConverter);
        }

        #endregion

        #region long typeInternal conversion

        public static explicit operator MsgPackToken(long value)
        {
            return CastValueToToken(value, NumberConverter);
        }

        public static explicit operator long(MsgPackToken token)
        {
            return CastTokenToValue<long>(token, NumberConverter);
        }

        #endregion

        #region int typeInternal conversion

        public static explicit operator MsgPackToken(int value)
        {
            return CastValueToToken(value, NumberConverter);
        }

        public static explicit operator int(MsgPackToken token)
        {
            return CastTokenToValue<int>(token, NumberConverter);
        }

        #endregion

        #region short typeInternal conversion

        public static explicit operator MsgPackToken(short value)
        {
            return CastValueToToken(value, NumberConverter);
        }

        public static explicit operator short(MsgPackToken token)
        {
            return CastTokenToValue<short>(token, NumberConverter);
        }

        #endregion

        #region sbyte typeInternal conversion

        public static explicit operator MsgPackToken(sbyte value)
        {
            return CastValueToToken(value, NumberConverter);
        }

        public static explicit operator sbyte(MsgPackToken token)
        {
            return CastTokenToValue<sbyte>(token, NumberConverter);
        }

        #endregion

        #region float typeInternal conversion

        public static explicit operator MsgPackToken(float value)
        {
            return CastValueToToken(value, NumberConverter);
        }

        public static explicit operator float(MsgPackToken token)
        {
            return CastTokenToValue<float>(token, NumberConverter);
        }

        #endregion

        #region double typeInternal conversion

        public static explicit operator MsgPackToken(double value)
        {
            return CastValueToToken(value, NumberConverter);
        }

        public static explicit operator double(MsgPackToken token)
        {
            return CastTokenToValue<double>(token, NumberConverter);
        }

        #endregion

        private static MsgPackToken CastValueToToken<T>(T value, IMsgPackConverter<T> converter)
        {
            var memoryStream = new MemoryStream();
            var writer = new MsgPackMemoryStreamWriter(memoryStream);
            converter.Write(value, writer);

            return new MsgPackToken(memoryStream.ToArray());
        }

        private static T CastTokenToValue<T>(MsgPackToken token, IMsgPackConverter<T> converter)
        {
            var reader = new MsgPackByteArrayReader(token.RawBytes);
            return converter.Read(reader);
        }
    }
}