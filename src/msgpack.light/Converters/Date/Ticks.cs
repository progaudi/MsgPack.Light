using System;

namespace ProGaudi.MsgPack.Converters.Date
{
    public class Ticks : IMsgPackFormatter<DateTime>, IMsgPackFormatter<DateTimeOffset>, IMsgPackParser<DateTime>, IMsgPackParser<DateTimeOffset>
    {
        private static readonly int BufferSize = DataLengths.GetMinAndMaxLength(DataCodes.Int64).max;

        public bool HasConstantSize => false;

        int IMsgPackFormatter<DateTime>.GetBufferSize(DateTime value) => BufferSize;

        int IMsgPackFormatter<DateTimeOffset>.GetBufferSize(DateTimeOffset value) => BufferSize;

        int IMsgPackFormatter<DateTimeOffset>.Format(Span<byte> destination, DateTimeOffset value) => MsgPackSpec.WriteInt64(destination, DateTimeUtils.FromDateTimeOffset(value));

        int IMsgPackFormatter<DateTime>.Format(Span<byte> destination, DateTime value) => MsgPackSpec.WriteInt64(destination, DateTimeUtils.FromDateTime(value));

        DateTime IMsgPackParser<DateTime>.Parse(ReadOnlySpan<byte> source, out int readSize) => DateTimeUtils.ToDateTime(MsgPackSpec.ReadInt32(source, out readSize));

        DateTimeOffset IMsgPackParser<DateTimeOffset>.Parse(ReadOnlySpan<byte> source, out int readSize) => DateTimeUtils.ToDateTimeOffset(MsgPackSpec.ReadInt32(source, out readSize));
    }
}
