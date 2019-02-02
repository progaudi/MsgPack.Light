using System;
using System.Buffers;

namespace ProGaudi.MsgPack.Converters.Number
{
    public sealed class SequenceParser :
        IMsgPackSequenceParser<byte>,
        IMsgPackSequenceParser<sbyte>,
        IMsgPackSequenceParser<short>,
        IMsgPackSequenceParser<ushort>,
        IMsgPackSequenceParser<int>,
        IMsgPackSequenceParser<uint>,
        IMsgPackSequenceParser<long>,
        IMsgPackSequenceParser<ulong>,
        IMsgPackSequenceParser<float>,
        IMsgPackSequenceParser<double>
    {
        public static SequenceParser Instance = new SequenceParser();

        byte IMsgPackSequenceParser<byte>.Parse(ReadOnlySequence<byte> source, out int readSize) => MsgPackSpec.ReadUInt8(source, out readSize);

        sbyte IMsgPackSequenceParser<sbyte>.Parse(ReadOnlySequence<byte> source, out int readSize) => MsgPackSpec.ReadInt8(source, out readSize);

        short IMsgPackSequenceParser<short>.Parse(ReadOnlySequence<byte> source, out int readSize) => MsgPackSpec.ReadInt16(source, out readSize);

        ushort IMsgPackSequenceParser<ushort>.Parse(ReadOnlySequence<byte> source, out int readSize) => MsgPackSpec.ReadUInt16(source, out readSize);

        int IMsgPackSequenceParser<int>.Parse(ReadOnlySequence<byte> source, out int readSize) => MsgPackSpec.ReadInt32(source, out readSize);

        uint IMsgPackSequenceParser<uint>.Parse(ReadOnlySequence<byte> source, out int readSize) => MsgPackSpec.ReadUInt32(source, out readSize);

        long IMsgPackSequenceParser<long>.Parse(ReadOnlySequence<byte> source, out int readSize) => MsgPackSpec.ReadInt64(source, out readSize);

        ulong IMsgPackSequenceParser<ulong>.Parse(ReadOnlySequence<byte> source, out int readSize) => MsgPackSpec.ReadUInt64(source, out readSize);

        float IMsgPackSequenceParser<float>.Parse(ReadOnlySequence<byte> source, out int readSize) => MsgPackSpec.TryReadFloat(source, out var result, out readSize)
            ? result
            : MsgPackSpec.ReadInt32(source, out readSize);

        double IMsgPackSequenceParser<double>.Parse(ReadOnlySequence<byte> source, out int readSize) => MsgPackSpec.TryReadDouble(source, out var result, out readSize)
            ? result
            : MsgPackSpec.ReadInt64(source, out readSize);
    }
}
