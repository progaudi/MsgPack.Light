using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using ProGaudi.MsgPack.Light;

namespace ProGaudi.MsgPack.Light.benchmark
{
    internal class BeerConverterHardCore : IMsgPackTokenConverter<Beer>
    {
        private IMsgPackTokenConverter<string> _stringConverter;

        private IMsgPackTokenConverter<List<string>> _listStringConverter;

        private IMsgPackTokenConverter<float> _floatConverter;

        private MsgPackContext _context;

        private byte[] _brand;

        private byte[] _alcohol;

        private byte[] _sort;

        private byte[] _brewery;

        public MsgPackToken ConvertFrom(Beer value)
        {
            if (value == null)
            {
                return _context.NullTokenConverter.ConvertFrom(null);
            }
            var map = new[]
            {
                new KeyValuePair<MsgPackToken, MsgPackToken>(
                    (MsgPackToken)_brand,
                    _stringConverter.ConvertFrom(value.Brand)),
                new KeyValuePair<MsgPackToken, MsgPackToken>(
                    (MsgPackToken)_sort,
                    _listStringConverter.ConvertFrom(value.Sort)),
                new KeyValuePair<MsgPackToken, MsgPackToken>(
                    (MsgPackToken)_alcohol,
                    _floatConverter.ConvertFrom(value.Alcohol)),
                new KeyValuePair<MsgPackToken, MsgPackToken>(
                    (MsgPackToken)_brewery,
                    _stringConverter.ConvertFrom(value.Brewery))
            };

            return (MsgPackToken)map;
        }

        public Beer ConvertTo(MsgPackToken token)
        {
            var propertiesMap = (KeyValuePair<MsgPackToken, MsgPackToken>[])token;
            if (propertiesMap == null)
            {
                return null;
            }

            if (propertiesMap.Length != 4)
            {
                throw new SerializationException("Bad format");
            }

            var result = new Beer();
            foreach (var pair in propertiesMap)
            {
                var propertyName = _stringConverter.ConvertTo(pair.Key);
                switch (propertyName)
                {
                    case nameof(result.Brand):
                        result.Brand = _stringConverter.ConvertTo(pair.Value);
                        break;
                    case nameof(result.Sort):
                        result.Sort = _listStringConverter.ConvertTo(pair.Value);
                        break;
                    case nameof(result.Alcohol):
                        result.Alcohol = _floatConverter.ConvertTo(pair.Value);
                        break;
                    case nameof(result.Brewery):
                        result.Brewery = _stringConverter.ConvertTo(pair.Value);
                        break;
                    default:
                        throw new SerializationException("Bad format");
                }
            }

            return result;
        }

        public void Initialize(MsgPackContext context)
        {
            _stringConverter = context.GetConverter<string>();
            _listStringConverter = context.GetConverter<List<string>>();
            _floatConverter = context.GetConverter<float>();
            _brand = MsgPackSerializer.Serialize(nameof(Beer.Brand));
            _alcohol = MsgPackSerializer.Serialize(nameof(Beer.Alcohol));
            _sort = MsgPackSerializer.Serialize(nameof(Beer.Sort));
            _brewery = MsgPackSerializer.Serialize(nameof(Beer.Brewery));
            _context = context;
        }
    }
}