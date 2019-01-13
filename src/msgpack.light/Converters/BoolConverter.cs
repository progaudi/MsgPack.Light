using System;
using System.Buffers;

namespace ProGaudi.MsgPack.Converters
{
    internal sealed class BoolConverter : IMsgPackFormatter<bool>, IMsgPackParser<bool>, IMsgPackSequenceParser<bool>
    {
        public static BoolConverter Instance = new BoolConverter();

        public int GetBufferSize(bool value) => DataLengths.Boolean;

        public bool HasConstantSize => true;

        public int Format(Span<byte> destination, bool value) => MsgPackSpec.WriteBoolean(destination, value);

        public bool Parse(ReadOnlySpan<byte> source, out int readSize) => MsgPackSpec.ReadBoolean(source, out readSize);

        public bool Parse(ReadOnlySequence<byte> source, out int readSize) => MsgPackSpec.ReadBoolean(source, out readSize);
    }
}
