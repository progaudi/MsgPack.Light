using ProGaudi.MsgPack.Light.Converters;

namespace ProGaudi.MsgPack.Light
{
    internal abstract class MsgPackWriterBase : IMsgPackWriter
    {
        public abstract void Write(DataTypes dataType);

        public abstract void Write(byte value);

        public abstract void Write(byte[] array);

        public void WriteArrayHeader(uint length)
        {
            if (length <= 15)
            {
                NumberConverter.WriteByteValue((byte) ((byte) DataTypes.FixArray + length), this);
                return;
            }

            if (length <= ushort.MaxValue)
            {
                Write(DataTypes.Array16);
                NumberConverter.WriteUShortValue((ushort) length, this);
            }
            else
            {
                Write(DataTypes.Array32);
                NumberConverter.WriteUIntValue(length, this);
            }

        }

        public void WriteMapHeader(uint length)
        {
            if (length <= 15)
            {
                NumberConverter.WriteByteValue((byte) ((byte) DataTypes.FixMap + length), this);
                return;
            }

            if (length <= ushort.MaxValue)
            {
                Write(DataTypes.Map16);
                NumberConverter.WriteUShortValue((ushort) length, this);
            }
            else
            {
                Write(DataTypes.Map32);
                NumberConverter.WriteUIntValue(length, this);
            }
        }

        public abstract byte[] ToArray();
    }
}