using System.Collections.Generic;
using System.Runtime.Serialization;

using MsgPack.Light;

namespace msgpack.light.benchmark
{
    internal class BeerConverter : IMsgPackConverter<Beer>
    {
        private IMsgPackConverter<string> _stringConverter;

        private IMsgPackConverter<List<string>> _listStringConverter;

        private IMsgPackConverter<float> _floatConverter;

        private MsgPackContext _context;

        public void Write(Beer value, IMsgPackWriter writer)
        {
            if (value == null)
            {
                _context.NullConverter.Write(null, writer);
                return;
            }

            writer.WriteMapHeader(4);
            _stringConverter.Write(nameof(value.Brand), writer);
            _stringConverter.Write(value.Brand, writer);

            _stringConverter.Write(nameof(value.Sort), writer);
            _listStringConverter.Write(value.Sort, writer);

            _stringConverter.Write(nameof(value.Alcohol), writer);
            _floatConverter.Write(value.Alcohol, writer);

            _stringConverter.Write(nameof(value.Brewery), writer);
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
            _context = context;
        }
    }
}