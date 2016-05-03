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

            writer.WriteArrayHeader(4);
            _stringConverter.Write(value.Brand, writer);
            _listStringConverter.Write(value.Sort, writer);
            _floatConverter.Write(value.Alcohol, writer);
            _stringConverter.Write(value.Brewery, writer);
        }

        public Beer Read(IMsgPackReader reader)
        {
            var length = reader.ReadArrayLength();
            if (length == null)
            {
                return null;
            }

            if (length != 4)
            {
                throw new SerializationException("Bad format");
            }

            return new Beer
            {
                Brand = _stringConverter.Read(reader),
                Sort = _listStringConverter.Read(reader),
                Alcohol = _floatConverter.Read(reader),
                Brewery = _stringConverter.Read(reader)
            };
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