using System.Buffers;

namespace ProGaudi.MsgPack.Converters.String
{
    public sealed class SequenceParser : IMsgPackSequenceParser<string>
    {
        public string Parse(ReadOnlySequence<byte> source, out int readSize)
        {
            if (MsgPackSpec.TryReadNil(source, out readSize)) return null;

            return MsgPackSpec.ReadString(source, out readSize);
        }
    }
}
