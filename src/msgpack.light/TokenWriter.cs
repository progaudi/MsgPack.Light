namespace ProGaudi.MsgPack.Light
{
    public class TokenWriter
    {
        private readonly IMsgPackWriter _writer;

        public TokenWriter(IMsgPackWriter writer)
        {
            _writer = writer;
        }

        public void Write(MsgPackToken token)
        {
            if (token == null)
            {
                _writer.Write(DataTypes.Null);
                return;
            }

            _writer.Write(token.DataType);

            if (token.DataType == DataTypes.Bin8 ||
                token.DataType == DataTypes.Bin16 ||
                token.DataType == DataTypes.Bin32 ||
                token.DataType == DataTypes.Str8 ||
                token.DataType == DataTypes.Str16 ||
                token.DataType == DataTypes.Str32)
            {
                var dataLength = token.ValueBytes.Length;
                if (dataLength <= byte.MaxValue)
                {
                    WriteByteValue((byte)dataLength, _writer);
                }
                else if (dataLength <= ushort.MaxValue)
                {
                    WriteUShortValue((ushort)dataLength, _writer);
                }
                else
                {
                    WriteUIntValue((uint)dataLength, _writer);
                }
            }

            if (token.ValueBytes != null)
            {
                _writer.Write(token.ValueBytes);
            }
            else if (token.ArrayElements != null)
            {
                var length = (uint)token.ArrayElements.Length;
                if (length <= 15)
                {
                }
                else if (length <= ushort.MaxValue)
                {
                    _writer.Write(DataTypes.Array16);
                    WriteUShortValue((ushort)length, _writer);
                }
                else
                {
                    _writer.Write(DataTypes.Array32);
                    WriteUIntValue(length, _writer);
                }

                foreach (var arrayElement in token.ArrayElements)
                {
                    Write(arrayElement);
                }
            }
            else if (token.MapElements != null)
            {
                var length = (uint)token.MapElements.Length;
                if (length <= 15)
                {
                }
                else if (length <= ushort.MaxValue)
                {
                    _writer.Write(DataTypes.Map16);
                    WriteUShortValue((ushort)length, _writer);
                }
                else
                {
                    _writer.Write(DataTypes.Map16);
                    WriteUIntValue(length, _writer);
                }

                foreach (var tokenMapElement in token.MapElements)
                {
                    Write(tokenMapElement.Key);
                    Write(tokenMapElement.Value);
                }
            }
        }

        private static void WriteUShortValue(ushort value, IMsgPackWriter writer)
        {
            unchecked
            {
                writer.Write((byte)((value >> 8) & 0xff));
                writer.Write((byte)(value & 0xff));
            }
        }

        private static void WriteUIntValue(uint value, IMsgPackWriter writer)
        {
            unchecked
            {
                writer.Write((byte)((value >> 24) & 0xff));
                writer.Write((byte)((value >> 16) & 0xff));
                writer.Write((byte)((value >> 8) & 0xff));
                writer.Write((byte)(value & 0xff));
            }
        }

        private static void WriteByteValue(byte value, IMsgPackWriter writer)
        {
            writer.Write(value);
        }
    }
}