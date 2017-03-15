using System;
using System.Collections.Generic;
using System.Linq;

namespace ProGaudi.MsgPack.Light
{
    internal class TokenReader
    {
        private readonly IMsgPackReader _reader;

        public TokenReader(IMsgPackReader reader)
        {
            _reader = reader;
        }

        public MsgPackToken Read()
        {
            return ReadToken(_reader);
        }

        private MsgPackToken ReadToken(IMsgPackReader reader)
        {
            var dataType = reader.ReadDataType();

            switch (dataType)
            {
                case DataTypeInternal.Null:
                case DataTypeInternal.False:
                case DataTypeInternal.True:
                    return CreateToken(dataType);
                case DataTypeInternal.UInt8:
                case DataTypeInternal.Int8:
                    return CreateToken(dataType, ReadBytes(reader, 1));
                case DataTypeInternal.UInt16:
                case DataTypeInternal.Int16:
                    return CreateToken(dataType, ReadBytes(reader, 2));
                case DataTypeInternal.UInt32:
                case DataTypeInternal.Int32:
                case DataTypeInternal.Single:
                    return CreateToken(dataType, ReadBytes(reader, 4));
                case DataTypeInternal.UInt64:
                case DataTypeInternal.Int64:
                case DataTypeInternal.Double:
                    return CreateToken(dataType, ReadBytes(reader, 8));
                case DataTypeInternal.Array16:
                    return CreateToken(
                        dataType,
                        ReadArrayItems(reader, ReadUInt16(reader)));
                case DataTypeInternal.Array32:
                    return CreateToken(
                        dataType,
                        ReadArrayItems(reader, ReadUInt32(reader)));
                case DataTypeInternal.Map16:
                    return CreateToken(
                        dataType,
                        ReadMapItems(reader, ReadUInt16(reader)));
                case DataTypeInternal.Map32:
                    return CreateToken(
                        dataType,
                        ReadMapItems(reader, ReadUInt32(reader)));
                case DataTypeInternal.Str8:
                    return CreateToken(dataType, ReadBytes(reader, ReadUInt8(reader)));
                case DataTypeInternal.Str16:
                    return CreateToken(dataType, ReadBytes(reader, ReadUInt16(reader)));
                case DataTypeInternal.Str32:
                    return CreateToken(dataType, ReadBytes(reader, ReadUInt32(reader)));
                case DataTypeInternal.Bin8:
                    return CreateToken(dataType, ReadBytes(reader, ReadUInt8(reader)));
                case DataTypeInternal.Bin16:
                    return CreateToken(dataType, ReadBytes(reader, ReadUInt16(reader)));
                case DataTypeInternal.Bin32:
                    return CreateToken(dataType, ReadBytes(reader, ReadUInt32(reader)));
            }

            if (dataType.GetHighBits(1) == DataTypeInternal.PositiveFixNum.GetHighBits(1) ||
                dataType.GetHighBits(3) == DataTypeInternal.NegativeFixNum.GetHighBits(3))
            {
                return CreateToken(dataType);
            }

            var arrayLength = TryGetLengthFromFixArray(dataType);
            if (arrayLength.HasValue)
            {
                return CreateToken(dataType, ReadArrayItems(reader, arrayLength.Value));
            }

            var mapLength = TryGetLengthFromFixMap(dataType);
            if (mapLength.HasValue)
            {
                return CreateToken(dataType, ReadMapItems(reader, mapLength.Value));
            }

            var stringLength = TryGetLengthFromFixStr(dataType);
            if (stringLength.HasValue)
            {
                return CreateToken(dataType, ReadBytes(reader, stringLength.Value));
            }

            throw new ArgumentOutOfRangeException();
        }

        private KeyValuePair<MsgPackToken, MsgPackToken>[] ReadMapItems(IMsgPackReader reader, uint count)
        {
            var result = new KeyValuePair<MsgPackToken, MsgPackToken>[count];
            for (var i = 0; i < count; i++)
            {
                var key = ReadToken(reader);
                var value = ReadToken(reader);
                result[i] = new KeyValuePair<MsgPackToken, MsgPackToken>(key, value);
            }

            return result;
        }

        private MsgPackToken[] ReadArrayItems(IMsgPackReader reader, uint count)
        {
            var result = new MsgPackToken[count];

            for (var i = 0; i < count; i++)
            {
                result[i] = ReadToken(reader);
            }

            return result;
        }

        private static byte[] ReadBytes(IMsgPackReader reader, uint bytesCount)
        {
            return reader.ReadBytes(bytesCount).ToArray();
        }

        private static uint? TryGetLengthFromFixStr(DataTypeInternal typeInternal)
        {
            var length = typeInternal - DataTypeInternal.FixStr;
            return typeInternal.GetHighBits(3) == DataTypeInternal.FixStr.GetHighBits(3) ? length : (uint?)null;
        }

        private static uint? TryGetLengthFromFixArray(DataTypeInternal typeInternal)
        {
            var length = typeInternal - DataTypeInternal.FixArray;
            return typeInternal.GetHighBits(4) == DataTypeInternal.FixArray.GetHighBits(4) ? length : (uint?)null;
        }

        private static uint? TryGetLengthFromFixMap(DataTypeInternal typeInternal)
        {
            var length = typeInternal - DataTypeInternal.FixMap;
            return typeInternal.GetHighBits(4) == DataTypeInternal.FixMap.GetHighBits(4) ? length : (uint?)null;
        }

        private static byte ReadUInt8(IMsgPackReader reader)
        {
            return reader.ReadByte();
        }

        private static ushort ReadUInt16(IMsgPackReader reader)
        {
            return (ushort)((reader.ReadByte() << 8) + reader.ReadByte());
        }

        private static uint ReadUInt32(IMsgPackReader reader)
        {
            var temp = (uint)(reader.ReadByte() << 24);
            temp += (uint)reader.ReadByte() << 16;
            temp += (uint)reader.ReadByte() << 8;
            temp += reader.ReadByte();

            return temp;
        }

        private static MsgPackToken CreateToken(DataTypeInternal dataTypeInternal)
        {
            return new MsgPackToken(dataTypeInternal);
        }

        private static MsgPackToken CreateToken(DataTypeInternal dataTypeInternal, byte[] valueBytes)
        {
            return new MsgPackToken(dataTypeInternal, valueBytes);
        }

        private static MsgPackToken CreateToken(DataTypeInternal dataTypeInternal, KeyValuePair<MsgPackToken, MsgPackToken>[] mapItems)
        {
            return new MsgPackToken(dataTypeInternal, mapItems);
        }

        private static MsgPackToken CreateToken(DataTypeInternal dataTypeInternal, MsgPackToken[] arrayElements)
        {
            return new MsgPackToken(dataTypeInternal, arrayElements);
        }
    }
}