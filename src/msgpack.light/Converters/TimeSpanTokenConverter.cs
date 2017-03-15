using System;

namespace ProGaudi.MsgPack.Light.Converters
{
    public class TimeSpanTokenConverter : IMsgPackTokenConverter<TimeSpan>
    {
        private Lazy<IMsgPackTokenConverter<long>> _longConverter = new Lazy<IMsgPackTokenConverter<long>>();

        public void Initialize(MsgPackContext context)
        {
            _longConverter = new Lazy<IMsgPackTokenConverter<long>>(context.GetConverter<long>);
        }

        public MsgPackToken ConvertFrom(TimeSpan value)
        {
            return _longConverter.Value.ConvertFrom(value.Ticks);
        }

        public TimeSpan ConvertTo(MsgPackToken token)
        {
            var longValue = _longConverter.Value.ConvertTo(token);

            return TimeSpan.FromTicks(longValue);
        }
    }
}