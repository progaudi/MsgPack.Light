﻿using System;
using System.IO;

using MsgPack.Light.Converters;

namespace MsgPack.Light
{
    internal abstract class BaseMsgPackReader : IMsgPackReader
    {
        public abstract byte ReadByte();

        public abstract byte[] ReadBytes(uint length);

        public abstract void Seek(long offset, SeekOrigin origin);

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

        public virtual DataTypes ReadDataType()
        {
            return (DataTypes)ReadByte();
        }

        public void SkipToken()
        {
            var dataType = ReadDataType();

            switch (dataType)
            {
                case DataTypes.Null:
                case DataTypes.False:
                case DataTypes.True:
                    return;
                case DataTypes.UInt8:
                case DataTypes.Int8:
                    SkipBytes(1);
                    return;
                case DataTypes.UInt16:
                case DataTypes.Int16:
                    SkipBytes(2);
                    return;
                case DataTypes.UInt32:
                case DataTypes.Int32:
                case DataTypes.Single:
                    SkipBytes(4);
                    return;
                case DataTypes.UInt64:
                case DataTypes.Int64:
                case DataTypes.Double:
                    SkipBytes(8);
                    return;
                case DataTypes.Array16:
                    SkipArrayItems(IntConverter.ReadUInt16(this));
                    return;
                case DataTypes.Array32:
                    SkipArrayItems(IntConverter.ReadUInt32(this));
                    return;
                case DataTypes.Map16:
                    SkipMapItems(IntConverter.ReadUInt16(this));
                    return;
                case DataTypes.Map32:
                    SkipMapItems(IntConverter.ReadUInt32(this));
                    return;
                case DataTypes.Str8:
                    SkipBytes(IntConverter.ReadUInt8(this));
                    return;
                case DataTypes.Str16:
                    SkipBytes(IntConverter.ReadUInt16(this));
                    return;
                case DataTypes.Str32:
                    SkipBytes(IntConverter.ReadUInt32(this));
                    return;
                case DataTypes.Bin8:
                    SkipBytes(IntConverter.ReadUInt8(this));
                    return;
                case DataTypes.Bin16:
                    SkipBytes(IntConverter.ReadUInt16(this));
                    return;
                case DataTypes.Bin32:
                    SkipBytes(IntConverter.ReadUInt32(this));
                    return;
            }

            if (dataType.GetHighBits(1) == DataTypes.PositiveFixNum.GetHighBits(1) ||
                dataType.GetHighBits(3) == DataTypes.NegativeFixNum.GetHighBits(3))
            {
                return;
            }

            var arrayLength = TryGetLengthFromFixArray(dataType);
            if (arrayLength.HasValue)
            {
                SkipArrayItems(arrayLength.Value);
                return;
            }

            var mapLength = TryGetLengthFromFixMap(dataType);
            if (mapLength.HasValue)
            {
                SkipMapItems(mapLength.Value);
                return;
            }

            var stringLength = TryGetLengthFromFixStr(dataType);
            if (stringLength.HasValue)
            {
                SkipBytes(stringLength.Value);
                return;
            }
            
            throw new ArgumentOutOfRangeException();
        }
        
        private void SkipMapItems(uint count)
        {
            for (var i = 0; i < count; i++)
            {
                SkipToken();
                SkipToken();
            }
        }

        private void SkipArrayItems(uint count)
        {
            for (var i = 0; i < count; i++)
            {
                SkipToken();
            }
        }

        private void SkipBytes(uint bytesCount)
        {
            Seek(bytesCount, SeekOrigin.Current);
        }

        private static uint? TryGetLengthFromFixStr(DataTypes type)
        {
            var length = type - DataTypes.FixStr;
            return type.GetHighBits(3) == DataTypes.FixStr.GetHighBits(3) ? length : (uint?)null;
        }

        protected static uint? TryGetLengthFromFixArray(DataTypes type)
        {
            var length = type - DataTypes.FixArray;
            return type.GetHighBits(4) == DataTypes.FixArray.GetHighBits(4) ? length : (uint?)null;
        }

        protected static uint? TryGetLengthFromFixMap(DataTypes type)
        {
            var length = type - DataTypes.FixMap;
            return type.GetHighBits(4) == DataTypes.FixMap.GetHighBits(4) ? length : (uint?)null;
        }
    }
}