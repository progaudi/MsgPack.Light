using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ProGaudi.MsgPack.Light.Benchmark.Data
{
    internal class BeerConverterHardCore : IMsgPackConverter<Beer>
    {
        private IMsgPackConverter<string> _stringConverter;

        private IMsgPackConverter<List<string>> _listStringConverter;

        private IMsgPackConverter<float> _floatConverter;

        private MsgPackContext _context;

        private byte[] _brand;

        private byte[] _alcohol;

        private byte[] _sort;

        private byte[] _brewery;

        public void Write(Beer value, IMsgPackWriter writer)
        {
            if (value == null)
            {
                _context.NullConverter.Write(null, writer);
                return;
            }

            writer.WriteMapHeader(4);
            writer.Write(_brand);
            _stringConverter.Write(value.Brand, writer);

            writer.Write(_sort);
            _listStringConverter.Write(value.Sort, writer);

            writer.Write(_alcohol);
            _floatConverter.Write(value.Alcohol, writer);

            writer.Write(_brewery);
            _stringConverter.Write(value.Brewery, writer);
        }

        public Beer Read(IMsgPackReader reader)
        {
            var length = reader.ReadMapLength();
            if (length == null)
            {
                return null;
            }

            if (length != 4)
            {
                throw new SerializationException("Bad format");
            }

            var result = new Beer();
            for (var i = 0; i < length.Value; i++)
            {
                var propertyName = _stringConverter.Read(reader);
                switch (propertyName)
                {
                    case nameof(result.Brand):
                        result.Brand = _stringConverter.Read(reader);
                        break;
                    case nameof(result.Sort):
                        result.Sort = _listStringConverter.Read(reader);
                        break;
                    case nameof(result.Alcohol):
                        result.Alcohol = _floatConverter.Read(reader);
                        break;
                    case nameof(result.Brewery):
                        result.Brewery = _stringConverter.Read(reader);
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