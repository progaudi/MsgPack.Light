using System;
using System.IO;

using MsgPack.Light.Converters;

namespace MsgPack.Light
{
    internal class MsgPackByteArrayReader : IMsgPackReader
    {
        private readonly byte[] _data;

        private uint _offset;

        public MsgPackByteArrayReader(byte[] data)
        {
            _data = data;
            _offset = 0;
        }

        public DataTypes ReadDataType()
        {
            return (DataTypes) ReadByte();
        }

        public byte ReadByte()
        {
            CheckLength(1);

            return _data[_offset++];
        }

        private void CheckLength(uint length)
        {
            if (_offset + length > _data.Length)
                throw ExceptionUtils.NotEnoughBytes(_data.Length - _offset, length);
        }

        public ArraySegment<byte> ReadBytes(uint length)
        {
            CheckLength(length);
            _offset += length;
            return new ArraySegment<byte>(_data, (int) (_offset - length), (int) length);
        }

        public void Seek(int offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    _offset = (uint) offset;
                    break;
                case SeekOrigin.Current:
                    _offset = (uint) (_offset + offset);
                    break;
                case SeekOrigin.End:
                    _offset = (uint) (_data.Length + offset);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(origin), origin, null);
            }
        }

        public uint? ReadArrayLength()
        {
            var type = ReadDataType();
            switch (type)
            {
                case DataTypes.Null:
                    return null;
                case DataTypes.Array16:
                    return IntConverter.ReadUInt16(this);

                case DataTypes.Array32:
                    return IntConverter.ReadUInt32(this);
            }

            var length = TryGetLengthFromFixArray(type);

            if (length.HasValue)
            {
                return length;
            }

            throw ExceptionUtils.BadTypeException(type, DataTypes.Array16, DataTypes.Array32, DataTypes.FixArray, DataTypes.Null);
        }

        public uint? ReadMapLength()
        {
            var type = ReadDataType();

            switch (type)
            {
                case DataTypes.Null:
                    return null;
                case DataTypes.Map16:
                    return IntConverter.ReadUInt16(this);

                case DataTypes.Map32:
                    return IntConverter.ReadUInt32(this);
            }

            var length = TryGetLengthFromFixMap(type);
            if (length.HasValue)
                return length.Value;

            throw ExceptionUtils.BadTypeException(type, DataTypes.Map16, DataTypes.Map32, DataTypes.FixMap, DataTypes.Null);
        }

        private static uint? TryGetLengthFromFixArray(DataTypes type)
        {
            var length = type - DataTypes.FixArray;
            return type.GetHighBits(4) == DataTypes.FixArray.GetHighBits(4) ? length : (uint?) null;
        }

        private static uint? TryGetLengthFromFixMap(DataTypes type)
        {
            var length = type - DataTypes.FixMap;
            return type.GetHighBits(4) == DataTypes.FixMap.GetHighBits(4) ? length : (uint?) null;
        }
    }
}
