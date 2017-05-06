// <copyright file="EnumConverterGenerator.cs" company="eVote">
//   Copyright © eVote
// </copyright>

using System;
using System.Globalization;

namespace ProGaudi.MsgPack.Light.Converters.Generation
{
    internal class EnumConverterGenerator
    {
        private readonly Lazy<IMsgPackConverter<string>> _stringConverter;
        private readonly Lazy<IMsgPackConverter<sbyte>> _sbyteConverter;
        private readonly Lazy<IMsgPackConverter<byte>> _byteConverter;
        private readonly Lazy<IMsgPackConverter<short>> _shortConverter;
        private readonly Lazy<IMsgPackConverter<ushort>> _ushortConverter;
        private readonly Lazy<IMsgPackConverter<int>> _intConverter;
        private readonly Lazy<IMsgPackConverter<uint>> _uintConverter;
        private readonly Lazy<IMsgPackConverter<long>> _longConverter;
        private readonly Lazy<IMsgPackConverter<ulong>> _ulongConverter;
        private readonly bool _convertEnumsAsStrings;

        public EnumConverterGenerator(MsgPackContext context, bool convertEnumsAsStrings)
        {
            _convertEnumsAsStrings = convertEnumsAsStrings;

            _stringConverter = new Lazy<IMsgPackConverter<string>>(() => context.GetConverter<string>());
            _sbyteConverter = new Lazy<IMsgPackConverter<sbyte>>(() => context.GetConverter<sbyte>());
            _byteConverter = new Lazy<IMsgPackConverter<byte>>(() => context.GetConverter<byte>());
            _shortConverter = new Lazy<IMsgPackConverter<short>>(() => context.GetConverter<short>());
            _ushortConverter = new Lazy<IMsgPackConverter<ushort>>(() => context.GetConverter<ushort>());
            _intConverter = new Lazy<IMsgPackConverter<int>>(() => context.GetConverter<int>());
            _uintConverter = new Lazy<IMsgPackConverter<uint>>(() => context.GetConverter<uint>());
            _longConverter = new Lazy<IMsgPackConverter<long>>(() => context.GetConverter<long>());
            _ulongConverter = new Lazy<IMsgPackConverter<ulong>>(() => context.GetConverter<ulong>());
        }

        public EnumConverter<T> CreateConverter<T>()
        {
            var enumUnderlyingType = Enum.GetUnderlyingType(typeof(T));
            Action<IConvertible, IMsgPackWriter> writeMethod;
            Func< IMsgPackReader, T > readMethod;

            if (_convertEnumsAsStrings)
            {
                readMethod = reader => (T)Enum.Parse(typeof(T), _stringConverter.Value.Read(reader));
                writeMethod = (value, writer) => _stringConverter.Value.Write(value.ToString(), writer);
            }
            else if (enumUnderlyingType == typeof(sbyte))
            {
                readMethod = reader => (T)Enum.ToObject(typeof(T), _sbyteConverter.Value.Read(reader));
                writeMethod = (value, writer) => _sbyteConverter.Value.Write(value.ToSByte(CultureInfo.InvariantCulture), writer);
            }
            else if (enumUnderlyingType == typeof(byte))
            {
                readMethod = reader => (T)Enum.ToObject(typeof(T), _byteConverter.Value.Read(reader));
                writeMethod = (value, writer) => _byteConverter.Value.Write(value.ToByte(CultureInfo.InvariantCulture), writer);
            }
            else if (enumUnderlyingType == typeof(short))
            {
                readMethod = reader => (T)Enum.ToObject(typeof(T), _shortConverter.Value.Read(reader));
                writeMethod = (value, writer) => _shortConverter.Value.Write(value.ToInt16(CultureInfo.InvariantCulture), writer);
            }
            else if (enumUnderlyingType == typeof(ushort))
            {
                readMethod = reader => (T)Enum.ToObject(typeof(T), _ushortConverter.Value.Read(reader));
                writeMethod = (value, writer) => _ushortConverter.Value.Write(value.ToUInt16(CultureInfo.InvariantCulture), writer);
            }
            else if (enumUnderlyingType == typeof(int))
            {
                readMethod = reader => (T)Enum.ToObject(typeof(T), _intConverter.Value.Read(reader));
                writeMethod = (value, writer) => _intConverter.Value.Write(value.ToInt32(CultureInfo.InvariantCulture), writer);
            }
            else if (enumUnderlyingType == typeof(uint))
            {
                readMethod = reader => (T)Enum.ToObject(typeof(T), _uintConverter.Value.Read(reader));
                writeMethod = (value, writer) => _uintConverter.Value.Write(value.ToUInt32(CultureInfo.InvariantCulture), writer);
            }
            else if (enumUnderlyingType == typeof(long))
            {
                readMethod = reader => (T)Enum.ToObject(typeof(T), _longConverter.Value.Read(reader));
                writeMethod = (value, writer) => _longConverter.Value.Write(value.ToInt64(CultureInfo.InvariantCulture), writer);
            }
            else if (enumUnderlyingType == typeof(ulong))
            {
                readMethod = reader => (T) Enum.ToObject(typeof(T), _ulongConverter.Value.Read(reader));
                writeMethod = (value, writer) => _ulongConverter.Value.Write(value.ToUInt64(CultureInfo.InvariantCulture), writer);
            }
            else
            {
                throw ExceptionUtils.UnexpectedEnumUnderlyingType(enumUnderlyingType);
            }

            return new EnumConverter<T>(writeMethod, readMethod);
        }
    }
}