using System;
using System.Collections.Generic;
using System.IO;

using ProGaudi.MsgPack.Light.Converters;

namespace ProGaudi.MsgPack.Light
{
    public class MsgPackToken
    {
        private static readonly IMsgPackConverter<bool> BoolConverter = new BoolConverter();
        private static readonly IMsgPackConverter<string> StringConverter = new StringConverter();
        private static readonly NumberConverter NumberConverter = new NumberConverter(false);

        private static IMsgPackConverter<byte[]> _binaryConverter;
        private static IMsgPackConverter<MsgPackToken[]> _arrayConverter;
        private static IMsgPackConverter<Dictionary<MsgPackToken, MsgPackToken>> _dictionaryConverter;
        private static IMsgPackConverter<DateTime> _dateTimeConverter;
        private static IMsgPackConverter<DateTimeOffset> _dateTimeOffsetConverter;
        private static IMsgPackConverter<TimeSpan> _timeSpanConverter;

        public MsgPackToken(byte[] rawBytes, MsgPackContext context = null)
        {
            RawBytes = rawBytes;

            if (context == null)
            {
                return;
            }

            if (_arrayConverter == null)
            {
                _arrayConverter = context.GetConverter<MsgPackToken[]>();
            }

            if (_dictionaryConverter == null)
            {
                _dictionaryConverter = context.GetConverter<Dictionary<MsgPackToken, MsgPackToken>>();
            }

            if (_binaryConverter == null)
            {
                _binaryConverter = context.GetConverter<byte[]>();
            }

            if (_dateTimeConverter == null)
            {
                _dateTimeConverter = context.GetConverter<DateTime>();
            }

            if (_dateTimeOffsetConverter== null)
            {
                _dateTimeOffsetConverter = context.GetConverter<DateTimeOffset>();
            }

            if (_timeSpanConverter == null)
            {
                _timeSpanConverter = context.GetConverter<TimeSpan>();
            }
        }

        public byte[] RawBytes { get; }

        #region Bool type conversion

        public static explicit operator MsgPackToken(bool b)
        {
            return CastValueToToken(b, BoolConverter);
        }

        public static explicit operator bool(MsgPackToken token)
        {
            return CastTokenToValue(token, BoolConverter);
        }

        #endregion

        #region String type conversion

        public static explicit operator MsgPackToken(string str)
        {
            return CastValueToToken(str, StringConverter);
        }

        public static explicit operator string(MsgPackToken token)
        {
            return CastTokenToValue(token, StringConverter);
        }

        #endregion

        #region byte[] type conversion

        public static explicit operator MsgPackToken(byte[] data)
        {
            return CastValueToToken(data, _binaryConverter);
        }

        public static explicit operator byte[] (MsgPackToken token)
        {
            return CastTokenToValue<byte[]>(token, _binaryConverter);
        }

        #endregion

        #region ulong type conversion

        public static explicit operator MsgPackToken(ulong value)
        {
            return CastValueToToken(value, NumberConverter);
        }

        public static explicit operator ulong(MsgPackToken token)
        {
            return CastTokenToValue<ulong>(token, NumberConverter);
        }

        #endregion

        #region uint type conversion

        public static explicit operator MsgPackToken(uint value)
        {
            return CastValueToToken(value, NumberConverter);
        }

        public static explicit operator uint(MsgPackToken token)
        {
            return CastTokenToValue<uint>(token, NumberConverter);
        }

        #endregion

        #region ushort type conversion

        public static explicit operator MsgPackToken(ushort value)
        {
            return CastValueToToken(value, NumberConverter);
        }

        public static explicit operator ushort(MsgPackToken token)
        {
            return CastTokenToValue<ushort>(token, NumberConverter);
        }

        #endregion

        #region byte type conversion

        public static explicit operator MsgPackToken(byte value)
        {
            return CastValueToToken(value, NumberConverter);
        }

        public static explicit operator byte(MsgPackToken token)
        {
            return CastTokenToValue<byte>(token, NumberConverter);
        }

        #endregion

        #region long type conversion

        public static explicit operator MsgPackToken(long value)
        {
            return CastValueToToken(value, NumberConverter);
        }

        public static explicit operator long(MsgPackToken token)
        {
            return CastTokenToValue<long>(token, NumberConverter);
        }

        #endregion

        #region int type conversion

        public static explicit operator MsgPackToken(int value)
        {
            return CastValueToToken(value, NumberConverter);
        }

        public static explicit operator int(MsgPackToken token)
        {
            return CastTokenToValue<int>(token, NumberConverter);
        }

        #endregion

        #region short type conversion

        public static explicit operator MsgPackToken(short value)
        {
            return CastValueToToken(value, NumberConverter);
        }

        public static explicit operator short(MsgPackToken token)
        {
            return CastTokenToValue<short>(token, NumberConverter);
        }

        #endregion

        #region sbyte type conversion

        public static explicit operator MsgPackToken(sbyte value)
        {
            return CastValueToToken(value, NumberConverter);
        }

        public static explicit operator sbyte(MsgPackToken token)
        {
            return CastTokenToValue<sbyte>(token, NumberConverter);
        }

        #endregion

        #region float type conversion

        public static explicit operator MsgPackToken(float value)
        {
            return CastValueToToken(value, NumberConverter);
        }

        public static explicit operator float(MsgPackToken token)
        {
            return CastTokenToValue<float>(token, NumberConverter);
        }

        #endregion

        #region double type conversion

        public static explicit operator MsgPackToken(double value)
        {
            return CastValueToToken(value, NumberConverter);
        }

        public static explicit operator double(MsgPackToken token)
        {
            return CastTokenToValue<double>(token, NumberConverter);
        }

        #endregion

        #region DateTime type conversion

        public static explicit operator MsgPackToken(DateTime value)
        {
            return CastValueToToken(value, _dateTimeConverter);
        }

        public static explicit operator DateTime(MsgPackToken token)
        {
            return CastTokenToValue<DateTime>(token, _dateTimeConverter);
        }

        #endregion

        #region DateTimeOffset type conversion

        public static explicit operator MsgPackToken(DateTimeOffset value)
        {
            return CastValueToToken(value, _dateTimeOffsetConverter);
        }

        public static explicit operator DateTimeOffset(MsgPackToken token)
        {
            return CastTokenToValue<DateTimeOffset>(token, _dateTimeOffsetConverter);
        }

        #endregion

        #region DateTimeOffset type conversion

        public static explicit operator MsgPackToken(TimeSpan value)
        {
            return CastValueToToken(value, _timeSpanConverter);
        }

        public static explicit operator TimeSpan(MsgPackToken token)
        {
            return CastTokenToValue<TimeSpan>(token, _timeSpanConverter);
        }

        #endregion

        #region MsgPackToken[] type conversion

        public static explicit operator MsgPackToken(MsgPackToken[] value)
        {
            return CastValueToToken(value, _arrayConverter);
        }

        public static explicit operator MsgPackToken[] (MsgPackToken token)
        {
            return CastTokenToValue<MsgPackToken[]>(token, _arrayConverter);
        }

        #endregion

        #region Dictionary<MsgPackToken,MsgPackToken> type conversion

        public static explicit operator MsgPackToken(Dictionary<MsgPackToken, MsgPackToken> value)
        {
            return CastValueToToken(value, _dictionaryConverter);
        }

        public static explicit operator Dictionary<MsgPackToken, MsgPackToken>(MsgPackToken token)
        {
            return CastTokenToValue<Dictionary<MsgPackToken, MsgPackToken>>(token, _dictionaryConverter);
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