using System;

namespace ProGaudi.MsgPack.Light.Converters
{
    public class DateTimeConverter : IMsgPackConverter<DateTime>, IMsgPackConverter<DateTimeOffset>
    {
        private Lazy<IMsgPackConverter<ulong>> _ulongConverter;
        private Lazy<IMsgPackConverter<long>> _longConverter;

        public void Initialize(MsgPackContext context)
        {
            _ulongConverter = new Lazy<IMsgPackConverter<ulong>>(context.GetConverter<ulong>);
            _longConverter = new Lazy<IMsgPackConverter<long>>(context.GetConverter<long>);
        }
        
        public MsgPackToken Write(DateTime value)
        {
            var longValue = DateTimeUtils.FromDateTime(value);

            return _longConverter.Value.Write(longValue);
        }

        DateTime IMsgPackConverter<DateTime>.Read(MsgPackToken token)
        {
            var ulongValue = _ulongConverter.Value.Read(token);
            return DateTimeUtils.ToDateTime((long)ulongValue);
        }

        public MsgPackToken Write(DateTimeOffset value)
        {
            var longValue = DateTimeUtils.FromDateTimeOffset(value);

            return _longConverter.Value.Write(longValue);
        }

        DateTimeOffset IMsgPackConverter<DateTimeOffset>.Read(MsgPackToken token)
        {
            var ulongValue = _ulongConverter.Value.Read(token);
            return DateTimeUtils.ToDateTimeOffset((long)ulongValue);
        }
    }
}