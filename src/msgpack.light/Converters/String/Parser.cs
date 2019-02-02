using System;

namespace ProGaudi.MsgPack.Converters.String
{
    public sealed class Parser : IMsgPackParser<string>
    {
        public string Parse(ReadOnlySpan<byte> source, out int readSize)
        {
            if (MsgPackSpec.TryReadNil(source, out readSize)) return null;

            return MsgPackSpec.ReadString(source, out readSize);
        }
    }
}
