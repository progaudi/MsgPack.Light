using System;
using System.Text;

namespace MsgPack.Light.Converters
{
    internal class StringConverter : IMsgPackConverter<string>
    {
        private static readonly Encoding Utf8 = new UTF8Encoding(false);
        private MsgPackContext _context;

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

            var data = Utf8.GetBytes(value);

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
                    return ReadString(reader, IntConverter.ReadUInt8(reader));

                case DataTypes.Str16:
                    return ReadString(reader, IntConverter.ReadUInt16(reader));

                case DataTypes.Str32:
                    return ReadString(reader, IntConverter.ReadUInt32(reader));
            }

            uint length;
            if (TryGetFixstrLength(type, out length))
            {
                return ReadString(reader, length);
            }

            throw ExceptionUtils.BadTypeException(type, DataTypes.FixStr, DataTypes.Str8, DataTypes.Str16, DataTypes.Str32);
        }

        private string ReadString(IMsgPackReader reader, uint length)
        {
            var buffer = reader.ReadBytes(length);

            return Utf8.GetString(buffer, 0, buffer.Length);
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
                IntConverter.WriteValue((byte)length, writer);
                return;
            }

            if (length <= ushort.MaxValue)
            {
                writer.Write(DataTypes.Str16);
                IntConverter.WriteValue((ushort)length, writer);
            }
            else
            {
                writer.Write(DataTypes.Str32);
                IntConverter.WriteValue((uint)length, writer);
            }
        }
    }
}