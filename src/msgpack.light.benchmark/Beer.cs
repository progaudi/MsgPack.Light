using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using ProGaudi.MsgPack.Light;

namespace ProGaudi.MsgPack.Light.benchmark
{
    public class Beer
    {
        public string Brand { get; set; }

        public List<string> Sort { get; set; }

        public float Alcohol { get; set; }

        public string Brewery { get; set; }
    }

    internal class BeerConverter : IMsgPackTokenConverter<Beer>
    {
        private IMsgPackTokenConverter<string> _stringConverter;

        private IMsgPackTokenConverter<List<string>> _listStringConverter;

        private IMsgPackTokenConverter<float> _floatConverter;

        private MsgPackContext _context;

        public MsgPackToken ConvertFrom(Beer value)
        {
            if (value == null)
            {
                return _context.NullTokenConverter.ConvertFrom(null);
            }
            var map = new[]
            {
                new KeyValuePair<MsgPackToken, MsgPackToken>(
                    _stringConverter.ConvertFrom(nameof(value.Brand)),
                    _stringConverter.ConvertFrom(value.Brand)),
                new KeyValuePair<MsgPackToken, MsgPackToken>(
                    _stringConverter.ConvertFrom(nameof(value.Sort)),
                    _listStringConverter.ConvertFrom(value.Sort)),
                new KeyValuePair<MsgPackToken, MsgPackToken>(
                    _stringConverter.ConvertFrom(nameof(value.Alcohol)),
                    _floatConverter.ConvertFrom(value.Alcohol)),
                new KeyValuePair<MsgPackToken, MsgPackToken>(
                    _stringConverter.ConvertFrom(nameof(value.Brewery)),
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
            _context = context;
        }
    }
}