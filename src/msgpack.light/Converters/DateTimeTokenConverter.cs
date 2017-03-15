using System;

namespace ProGaudi.MsgPack.Light.Converters
{
    public class DateTimeTokenConverter : IMsgPackTokenConverter<DateTime>, IMsgPackTokenConverter<DateTimeOffset>
    {
        private Lazy<IMsgPackTokenConverter<ulong>> _ulongConverter;
        private Lazy<IMsgPackTokenConverter<long>> _longConverter;

        public void Initialize(MsgPackContext context)
        {
            _ulongConverter = new Lazy<IMsgPackTokenConverter<ulong>>(context.GetConverter<ulong>);
            _longConverter = new Lazy<IMsgPackTokenConverter<long>>(context.GetConverter<long>);
        }
        
        public MsgPackToken ConvertFrom(DateTime value)
        {
            var longValue = DateTimeUtils.FromDateTime(value);

            return _longConverter.Value.ConvertFrom(longValue);
        }

        DateTime IMsgPackTokenConverter<DateTime>.ConvertTo(MsgPackToken token)
        {
            var ulongValue = _ulongConverter.Value.ConvertTo(token);
            return DateTimeUtils.ToDateTime((long)ulongValue);
        }

        public MsgPackToken ConvertFrom(DateTimeOffset value)
        {
            var longValue = DateTimeUtils.FromDateTimeOffset(value);

            return _longConverter.Value.ConvertFrom(longValue);
        }

        DateTimeOffset IMsgPackTokenConverter<DateTimeOffset>.ConvertTo(MsgPackToken token)
        {
            var ulongValue = _ulongConverter.Value.ConvertTo(token);
            return DateTimeUtils.ToDateTimeOffset((long)ulongValue);
        }
    }
}