using System;

namespace ProGaudi.MsgPack.Converters.Date
{
    public class Ticks : IMsgPackFormatter<DateTime>, IMsgPackFormatter<DateTimeOffset>, IMsgPackFormatter<TimeSpan>, IMsgPackParser<DateTime>, IMsgPackParser<DateTimeOffset>, IMsgPackParser<TimeSpan>
    {
        private static readonly int BufferSize = DataLengths.GetMinAndMaxLength(DataCodes.Int64).max;

        private static readonly DateTime UnixEpochUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static readonly Ticks Instance = new Ticks();

        public bool HasConstantSize => false;

        int IMsgPackFormatter<DateTime>.GetBufferSize(DateTime value) => BufferSize;

        int IMsgPackFormatter<DateTimeOffset>.GetBufferSize(DateTimeOffset value) => BufferSize;

        int IMsgPackFormatter<TimeSpan>.GetBufferSize(TimeSpan value) => BufferSize;

        int IMsgPackFormatter<DateTimeOffset>.Format(Span<byte> destination, DateTimeOffset value) => MsgPackSpec.WriteInt64(destination, value.ToUniversalTime().Subtract(UnixEpochUtc).Ticks);

        int IMsgPackFormatter<DateTime>.Format(Span<byte> destination, DateTime value) => MsgPackSpec.WriteInt64(destination, value.ToUniversalTime().Subtract(UnixEpochUtc).Ticks);

        int IMsgPackFormatter<TimeSpan>.Format(Span<byte> destination, TimeSpan value) => MsgPackSpec.WriteInt64(destination, value.Ticks);

        DateTime IMsgPackParser<DateTime>.Parse(ReadOnlySpan<byte> source, out int readSize) => UnixEpochUtc.AddTicks(ReadTicks(source, out readSize));

        DateTimeOffset IMsgPackParser<DateTimeOffset>.Parse(ReadOnlySpan<byte> source, out int readSize) => UnixEpochUtc.AddTicks(ReadTicks(source, out readSize));

        TimeSpan IMsgPackParser<TimeSpan>.Parse(ReadOnlySpan<byte> source, out int readSize) => new TimeSpan(ReadTicks(source, out readSize));

        private static long ReadTicks(ReadOnlySpan<byte> source, out int readSize)
        {
            return MsgPackSpec.TryReadInt64(source, out var value, out readSize)
                ? value
                : (long)MsgPackSpec.ReadUInt64(source, out readSize);
        }
    }
}
