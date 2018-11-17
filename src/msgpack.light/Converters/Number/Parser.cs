using System;

namespace ProGaudi.MsgPack.Converters.Number
{
    public sealed class Parser :
        IMsgPackParser<byte>,
        IMsgPackParser<sbyte>,
        IMsgPackParser<short>,
        IMsgPackParser<ushort>,
        IMsgPackParser<int>,
        IMsgPackParser<uint>,
        IMsgPackParser<long>,
        IMsgPackParser<ulong>,
        IMsgPackParser<float>,
        IMsgPackParser<double>
    {
        byte IMsgPackParser<byte>.Parse(ReadOnlySpan<byte> source, out int readSize) => MsgPackSpec.ReadUInt8(source, out readSize);

        sbyte IMsgPackParser<sbyte>.Parse(ReadOnlySpan<byte> source, out int readSize) => MsgPackSpec.ReadInt8(source, out readSize);

        short IMsgPackParser<short>.Parse(ReadOnlySpan<byte> source, out int readSize) => MsgPackSpec.ReadInt16(source, out readSize);

        ushort IMsgPackParser<ushort>.Parse(ReadOnlySpan<byte> source, out int readSize) => MsgPackSpec.ReadUInt16(source, out readSize);

        int IMsgPackParser<int>.Parse(ReadOnlySpan<byte> source, out int readSize) => MsgPackSpec.ReadInt32(source, out readSize);

        uint IMsgPackParser<uint>.Parse(ReadOnlySpan<byte> source, out int readSize) => MsgPackSpec.ReadUInt32(source, out readSize);

        long IMsgPackParser<long>.Parse(ReadOnlySpan<byte> source, out int readSize) => MsgPackSpec.ReadInt64(source, out readSize);

        ulong IMsgPackParser<ulong>.Parse(ReadOnlySpan<byte> source, out int readSize) => MsgPackSpec.ReadUInt64(source, out readSize);

        float IMsgPackParser<float>.Parse(ReadOnlySpan<byte> source, out int readSize) => MsgPackSpec.TryReadFloat(source, out var result, out readSize)
            ? result
            : MsgPackSpec.ReadInt32(source, out readSize);

        double IMsgPackParser<double>.Parse(ReadOnlySpan<byte> source, out int readSize) => MsgPackSpec.TryReadDouble(source, out var result, out readSize)
            ? result
            : MsgPackSpec.ReadInt64(source, out readSize);
    }
}
