using System;

namespace ProGaudi.MsgPack.Light.Converters
{
    public class TimeSpanConverter : IMsgPackConverter<TimeSpan>
    {
        private Lazy<IMsgPackConverter<long>> _longConverter = new Lazy<IMsgPackConverter<long>>();

        public void Initialize(MsgPackContext context)
        {
            _longConverter = new Lazy<IMsgPackConverter<long>>(context.GetConverter<long>);
        }

        public MsgPackToken Write(TimeSpan value)
        {
            return _longConverter.Value.Write(value.Ticks);
        }

        public TimeSpan Read(MsgPackToken token)
        {
            var longValue = _longConverter.Value.Read(token);

            return TimeSpan.FromTicks(longValue);
        }
    }
}