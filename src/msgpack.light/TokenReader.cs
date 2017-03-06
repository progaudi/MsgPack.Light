using System;
using System.Collections.Generic;
using System.Linq;

namespace ProGaudi.MsgPack.Light
{
    public class TokenReader
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
                case DataTypes.Null:
                case DataTypes.False:
                case DataTypes.True:
                    return CreateToken(dataType);
                case DataTypes.UInt8:
                case DataTypes.Int8:
                    return CreateToken(dataType, ReadBytes(reader, 1));
                case DataTypes.UInt16:
                case DataTypes.Int16:
                    return CreateToken(dataType, ReadBytes(reader, 2));
                case DataTypes.UInt32:
                case DataTypes.Int32:
                case DataTypes.Single:
                    return CreateToken(dataType, ReadBytes(reader, 4));
                case DataTypes.UInt64:
                case DataTypes.Int64:
                case DataTypes.Double:
                    return CreateToken(dataType, ReadBytes(reader, 8));
                case DataTypes.Array16:
                    return CreateToken(
                        dataType,
                        ReadArrayItems(reader, ReadUInt16(reader)));
                case DataTypes.Array32:
                    return CreateToken(
                        dataType,
                        ReadArrayItems(reader, ReadUInt32(reader)));
                case DataTypes.Map16:
                    return CreateToken(
                        dataType,
                        ReadMapItems(reader, ReadUInt16(reader)));
                case DataTypes.Map32:
                    return CreateToken(
                        dataType,
                        ReadMapItems(reader, ReadUInt32(reader)));
                case DataTypes.Str8:
                    return CreateToken(dataType, ReadBytes(reader, ReadUInt8(reader)));
                case DataTypes.Str16:
                    return CreateToken(dataType, ReadBytes(reader, ReadUInt16(reader)));
                case DataTypes.Str32:
                    return CreateToken(dataType, ReadBytes(reader, ReadUInt32(reader)));
                case DataTypes.Bin8:
                    return CreateToken(dataType, ReadBytes(reader, ReadUInt8(reader)));
                case DataTypes.Bin16:
                    return CreateToken(dataType, ReadBytes(reader, ReadUInt16(reader)));
                case DataTypes.Bin32:
                    return CreateToken(dataType, ReadBytes(reader, ReadUInt32(reader)));
            }

            if (dataType.GetHighBits(1) == DataTypes.PositiveFixNum.GetHighBits(1) ||
                dataType.GetHighBits(3) == DataTypes.NegativeFixNum.GetHighBits(3))
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

        private byte[] ReadBytes(IMsgPackReader reader, uint bytesCount)
        {
            return reader.ReadBytes(bytesCount).ToArray();
        }

        private static uint? TryGetLengthFromFixStr(DataTypes type)
        {
            var length = type - DataTypes.FixStr;
            return type.GetHighBits(3) == DataTypes.FixStr.GetHighBits(3) ? length : (uint?)null;
        }

        private static uint? TryGetLengthFromFixArray(DataTypes type)
        {
            var length = type - DataTypes.FixArray;
            return type.GetHighBits(4) == DataTypes.FixArray.GetHighBits(4) ? length : (uint?)null;
        }

        private static uint? TryGetLengthFromFixMap(DataTypes type)
        {
            var length = type - DataTypes.FixMap;
            return type.GetHighBits(4) == DataTypes.FixMap.GetHighBits(4) ? length : (uint?)null;
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

        private static MsgPackToken CreateToken(DataTypes dataType)
        {
            return new MsgPackToken(dataType);
        }

        private static MsgPackToken CreateToken(DataTypes dataType, byte[] valueBytes)
        {
            return new MsgPackToken(dataType, valueBytes);
        }

        private static MsgPackToken CreateToken(DataTypes dataType, KeyValuePair<MsgPackToken, MsgPackToken>[] mapItems)
        {
            return new MsgPackToken(dataType, mapItems);
        }

        private static MsgPackToken CreateToken(DataTypes dataType, MsgPackToken[] arrayElements)
        {
            return new MsgPackToken(dataType, arrayElements);
        }
    }
}