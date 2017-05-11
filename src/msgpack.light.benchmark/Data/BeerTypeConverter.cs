using System;

namespace ProGaudi.MsgPack.Light.Benchmark.Data
{
    public class BeerTypeConverter : IMsgPackConverter<BeerType>
    {
        private Lazy<IMsgPackConverter<int>> _intConverter;

        public void Initialize(MsgPackContext context)
        {
            _intConverter = new Lazy<IMsgPackConverter<int>>(context.GetConverter<int>);
        }

        public void Write(BeerType value, IMsgPackWriter writer)
        {
            _intConverter.Value.Write((int)value, writer);
        }

        public BeerType Read(IMsgPackReader reader)
        {
            return (BeerType)_intConverter.Value.Read(reader);
        }
    }
}