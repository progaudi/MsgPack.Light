using System;

namespace ProGaudi.MsgPack.Converters.String
{
    public sealed class UsualFormatter : IMsgPackFormatter<string>
    {
        public int GetBufferSize(string value) => value == null
            ? DataLengths.Nil
            : DataLengths.GetStringHeaderLengthByBytesCount(value.Length * 4) + value.Length * 4;

        public bool HasConstantSize => false;

        public int Format(Span<byte> destination, string value)
        {
            if (value == null) return MsgPackSpec.WriteNil(destination);

            return MsgPackSpec.WriteString(destination, value.AsSpan());
        }
    }
}
