using System;

namespace ProGaudi.MsgPack.Light
{
    public static class DataTypesExtension
    {
        internal static byte GetHighBits(this DataTypeInternal type, byte bitsCount)
        {
            return (byte)((byte)type >> (8 - bitsCount));
        }

        internal static DataType GetDataType(this DataTypeInternal dataTypeInternal)
        {
            switch (dataTypeInternal)
            {
                case DataTypeInternal.Null:
                    return DataType.Null;
                case DataTypeInternal.False:
                case DataTypeInternal.True:
                    return DataType.Boolean;
                case DataTypeInternal.Single:
                    return DataType.Single;
                case DataTypeInternal.Double:
                    return DataType.Double;
                case DataTypeInternal.UInt8:
                    return DataType.UInt8;
                case DataTypeInternal.UInt16:
                    return DataType.UInt16;
                case DataTypeInternal.UInt32:
                    return DataType.UInt32;
                case DataTypeInternal.UInt64:
                    return DataType.UInt64;
                case DataTypeInternal.NegativeFixNum:
                case DataTypeInternal.PositiveFixNum:
                case DataTypeInternal.Int8:
                    return DataType.Int8;
                case DataTypeInternal.Int16:
                    return DataType.Int16;
                case DataTypeInternal.Int32:
                    return DataType.Int32;
                case DataTypeInternal.Int64:
                    return DataType.Int64;
                case DataTypeInternal.FixArray:
                case DataTypeInternal.Array16:
                case DataTypeInternal.Array32:
                    return DataType.Array;
                case DataTypeInternal.FixMap:
                case DataTypeInternal.Map16:
                case DataTypeInternal.Map32:
                    return DataType.Map;
                case DataTypeInternal.FixStr:
                case DataTypeInternal.Str8:
                case DataTypeInternal.Str16:
                case DataTypeInternal.Str32:
                    return DataType.String;
                case DataTypeInternal.Bin8:
                case DataTypeInternal.Bin16:
                case DataTypeInternal.Bin32:
                    return DataType.Binary;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataTypeInternal), dataTypeInternal, null);
            }
        }
    }
}
