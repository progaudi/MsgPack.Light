using System;

namespace MsgPack.Light.Converters
{
    public class DateTimeConverter : IMsgPackConverter<DateTime>, IMsgPackConverter<DateTimeOffset>
    {
        private MsgPackContext _context;

        public void Initialize(MsgPackContext context)
        {
            _context = context;
        }

        public void Write(DateTime value, IMsgPackWriter writer)
        {
            var longValue = DateTimeUtils.FromDateTime(value);
            var longConverter = _context.GetConverter<long>();

            longConverter.Write(longValue, writer);
        }

        DateTime IMsgPackConverter<DateTime>.Read(IMsgPackReader reader)
        {
            var longConverter = _context.GetConverter<long>();
            var longValue = longConverter.Read(reader);
            return DateTimeUtils.ToDateTime(longValue);
        }

        public void Write(DateTimeOffset value, IMsgPackWriter writer)
        {
            var longValue = DateTimeUtils.FromDateTimeOffset(value);
            var longConverter = _context.GetConverter<long>();

            longConverter.Write(longValue, writer);
        }

        DateTimeOffset IMsgPackConverter<DateTimeOffset>.Read(IMsgPackReader reader)
        {
            var longConverter = _context.GetConverter<long>();
            var longValue = longConverter.Read(reader);
            return DateTimeUtils.ToDateTimeOffset(longValue);
        }
    }
}
