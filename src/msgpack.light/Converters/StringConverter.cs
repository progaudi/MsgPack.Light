using System;
using System.Text;

namespace ProGaudi.MsgPack.Light.Converters
{
    public abstract class StringConverter : IMsgPackConverter<string>
    {
        private static readonly Encoding Utf8 = new UTF8Encoding(false);

        private MsgPackContext _context;

        protected readonly Encoding Encoding;

        protected StringConverter(Encoding encoding)
        {
            Encoding = encoding;
        }

        /// <summary>
        /// Use this if you want precision converter. Maybe slow as hell (need to test)
        /// </summary>
        public static readonly StringConverter Utf8Precision = new PrecisionEncodingStringConverter(Utf8);

        /// <summary>
        /// Use this if majority of your strings are Ascii-compatible (1 byte per char)
        /// https://en.wikipedia.org/wiki/UTF-8
        /// </summary>
        public static readonly StringConverter Utf8AsciiCompat = new ConstLengthStringConverter(Utf8, 1);

        /// <summary>
        /// Use this is majority of your string contain "almost all Latin-script alphabets, and also Greek,
        /// Cyrillic, Coptic, Armenian, Hebrew, Arabic, Syriac, Thaana and N'Ko alphabets" (2 bytes per char)
        /// https://en.wikipedia.org/wiki/UTF-8
        /// </summary>
        public static readonly StringConverter Utf8TwoBytes = new ConstLengthStringConverter(Utf8, 2);

        /// <summary>
        /// Use this is majority of your string contain <see cref="Utf8TwoBytes"/> and "virtually all
        /// characters in common use[11] including most Chinese, Japanese and Korean characters" (3 bytes per char)
        /// https://en.wikipedia.org/wiki/UTF-8
        /// </summary>
        public static readonly StringConverter Utf8ThreeBytes = new ConstLengthStringConverter(Utf8, 2);

        /// <summary>
        /// Use this is majority of your string will contain emoji (4 bytes per char)
        /// https://en.wikipedia.org/wiki/UTF-8
        /// </summary>
        public static readonly StringConverter Utf8FourBytes = new ConstLengthStringConverter(Utf8, 2);

        public void Initialize(MsgPackContext context)
        {
            _context = context;
        }

        public void Write(string value, IMsgPackWriter writer)
        {
            if (value == null)
            {
                _context.NullConverter.Write(value, writer);
                return;
            }

            var data = Encoding.GetBytes(value);

            WriteStringHeaderAndLength(writer, data.Length);

            writer.Write(data);
        }

        public string Read(IMsgPackReader reader)
        {
            var type = reader.ReadDataType();

            switch (type)
            {
                case DataTypes.Null:
                    return null;

                case DataTypes.Str8:
                    return ReadString(reader, NumberConverter.ReadUInt8(reader));

                case DataTypes.Str16:
                    return ReadString(reader, NumberConverter.ReadUInt16(reader));

                case DataTypes.Str32:
                    return ReadString(reader, NumberConverter.ReadUInt32(reader));
            }

            if (TryGetFixstrLength(type, out var length))
            {
                return ReadString(reader, length);
            }

            throw ExceptionUtils.BadTypeException(type, DataTypes.FixStr, DataTypes.Str8, DataTypes.Str16, DataTypes.Str32);
        }

        public abstract int GuessByteArrayLength(string value);

        public bool HasFixedLength => false;

        private string ReadString(IMsgPackReader reader, uint length)
        {
            var buffer = reader.ReadBytes(length);

            return Encoding.GetString(buffer.Array, buffer.Offset, buffer.Count);
        }

        private bool TryGetFixstrLength(DataTypes type, out uint length)
        {
            length = type - DataTypes.FixStr;
            return type.GetHighBits(3) == DataTypes.FixStr.GetHighBits(3);
        }

        private void WriteStringHeaderAndLength(IMsgPackWriter writer, int length)
        {
            if (length <= 31)
            {
                writer.Write((byte)(((byte)DataTypes.FixStr + length) % 256));
                return;
            }

            if (length <= byte.MaxValue)
            {
                writer.Write(DataTypes.Str8);
                NumberConverter.WriteByteValue((byte) length, writer);
            }
            else if (length <= ushort.MaxValue)
            {
                writer.Write(DataTypes.Str16);
                NumberConverter.WriteUShortValue((ushort)length, writer);
            }
            else
            {
                writer.Write(DataTypes.Str32);
                NumberConverter.WriteUIntValue((uint)length, writer);
            }
        }
    }
}
