using System;

namespace ProGaudi.MsgPack.Converters.Number
{
    public sealed class UsualFormatter :
        IMsgPackFormatter<byte>,
        IMsgPackFormatter<sbyte>,
        IMsgPackFormatter<short>,
        IMsgPackFormatter<ushort>,
        IMsgPackFormatter<int>,
        IMsgPackFormatter<uint>,
        IMsgPackFormatter<long>,
        IMsgPackFormatter<ulong>,
        IMsgPackFormatter<float>,
        IMsgPackFormatter<double>
    {
        private static readonly int ByteLength = DataLengths.GetHeaderLength(DataCodes.UInt8);
        private static readonly int SByteLength = DataLengths.GetHeaderLength(DataCodes.Int8);
        private static readonly int ShortLength = DataLengths.GetHeaderLength(DataCodes.Int16);
        private static readonly int UShortLength = DataLengths.GetHeaderLength(DataCodes.UInt16);
        private static readonly int IntLength = DataLengths.GetHeaderLength(DataCodes.Int32);
        private static readonly int UIntLength = DataLengths.GetHeaderLength(DataCodes.UInt32);
        private static readonly int LongLength = DataLengths.GetHeaderLength(DataCodes.Int64);
        private static readonly int ULongLength = DataLengths.GetHeaderLength(DataCodes.UInt64);
        private static readonly int FloatLength = DataLengths.GetHeaderLength(DataCodes.Float32);
        private static readonly int DoubleLength = DataLengths.GetHeaderLength(DataCodes.Float64);

        int IMsgPackFormatter<byte>.GetBufferSize(byte value) => ByteLength;

        int IMsgPackFormatter<sbyte>.GetBufferSize(sbyte value) => SByteLength;

        int IMsgPackFormatter<short>.GetBufferSize(short value) => ShortLength;

        int IMsgPackFormatter<ushort>.GetBufferSize(ushort value) => UShortLength;

        int IMsgPackFormatter<int>.GetBufferSize(int value) => IntLength;

        int IMsgPackFormatter<uint>.GetBufferSize(uint value) => UIntLength;

        int IMsgPackFormatter<long>.GetBufferSize(long value) => LongLength;

        int IMsgPackFormatter<ulong>.GetBufferSize(ulong value) => ULongLength;

        int IMsgPackFormatter<float>.GetBufferSize(float value) => FloatLength;

        int IMsgPackFormatter<double>.GetBufferSize(double value) => DoubleLength;

        public bool HasConstantSize => true;

        int IMsgPackFormatter<double>.Format(Span<byte> destination, double value) => MsgPackSpec.WriteDouble(destination, value);

        int IMsgPackFormatter<float>.Format(Span<byte> destination, float value) => MsgPackSpec.WriteFloat(destination, value);

        int IMsgPackFormatter<ulong>.Format(Span<byte> destination, ulong value) => MsgPackSpec.WriteUInt64(destination, value);

        int IMsgPackFormatter<long>.Format(Span<byte> destination, long value) => MsgPackSpec.WriteInt64(destination, value);

        int IMsgPackFormatter<uint>.Format(Span<byte> destination, uint value) => MsgPackSpec.WriteUInt32(destination, value);

        int IMsgPackFormatter<int>.Format(Span<byte> destination, int value) => MsgPackSpec.WriteInt32(destination, value);

        int IMsgPackFormatter<ushort>.Format(Span<byte> destination, ushort value) => MsgPackSpec.WriteUInt16(destination, value);

        int IMsgPackFormatter<short>.Format(Span<byte> destination, short value) => MsgPackSpec.WriteInt16(destination, value);

        int IMsgPackFormatter<sbyte>.Format(Span<byte> destination, sbyte value) => MsgPackSpec.WriteInt8(destination, value);

        int IMsgPackFormatter<byte>.Format(Span<byte> destination, byte value) => MsgPackSpec.WriteUInt8(destination, value);
    }
}
