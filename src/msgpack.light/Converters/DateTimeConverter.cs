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

        public DateTime Read(IMsgPackReader reader, Func<DateTime> creator)
        {
            var longConverter = _context.GetConverter<long>();
            var longValue = longConverter.Read(reader, null);
            return DateTimeUtils.ToDateTime(longValue);
        }

        public void Write(DateTimeOffset value, IMsgPackWriter writer)
        {
            var longValue = DateTimeUtils.FromDateTimeOffset(value);
            var longConverter = _context.GetConverter<long>();

            longConverter.Write(longValue, writer);
        }

        public DateTimeOffset Read(IMsgPackReader reader, Func<DateTimeOffset> creator)
        {
            var longConverter = _context.GetConverter<long>();
            var longValue = longConverter.Read(reader, null);
            return DateTimeUtils.ToDateTimeOffset(longValue);
        }
    }
}
