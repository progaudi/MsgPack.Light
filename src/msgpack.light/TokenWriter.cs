namespace ProGaudi.MsgPack.Light
{
    internal class TokenWriter
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
                _writer.Write(DataTypeInternal.Null);
                return;
            }

            _writer.Write(token.DataTypeInternal);

            if (token.DataTypeInternal == DataTypeInternal.Bin8 ||
                token.DataTypeInternal == DataTypeInternal.Bin16 ||
                token.DataTypeInternal == DataTypeInternal.Bin32 ||
                token.DataTypeInternal == DataTypeInternal.Str8 ||
                token.DataTypeInternal == DataTypeInternal.Str16 ||
                token.DataTypeInternal == DataTypeInternal.Str32)
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
                    _writer.Write(DataTypeInternal.Array16);
                    WriteUShortValue((ushort)length, _writer);
                }
                else
                {
                    _writer.Write(DataTypeInternal.Array32);
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
                    _writer.Write(DataTypeInternal.Map16);
                    WriteUShortValue((ushort)length, _writer);
                }
                else
                {
                    _writer.Write(DataTypeInternal.Map16);
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