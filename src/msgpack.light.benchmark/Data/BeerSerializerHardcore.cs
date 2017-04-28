using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

using MsgPack;
using MsgPack.Serialization;

namespace ProGaudi.MsgPack.Light.Benchmark.Data
{
    internal class BeerSerializerHardcore : MessagePackSerializer<Beer>
    {
        private static readonly Encoding Utf8 = new UTF8Encoding(false);

        private readonly byte[] _brand;

        private readonly byte[] _alcohol;

        private readonly byte[] _sort;

        private readonly byte[] _brewery;

        public BeerSerializerHardcore(SerializationContext ownerContext)
            : base(ownerContext)
        {
            var serializer = ownerContext.GetSerializer<string>();
            _brand = serializer.PackSingleObject(nameof(Beer.Brand));
            _alcohol = serializer.PackSingleObject(nameof(Beer.Alcohol));
            _sort = serializer.PackSingleObject(nameof(Beer.Sort));
            _brewery = serializer.PackSingleObject(nameof(Beer.Brewery));
        }

        protected override void PackToCore(Packer packer, Beer beer)
        {
            if (beer == null)
            {
                packer.PackNull();
                return;
            }

            packer.PackMapHeader(4);

            packer.PackRawBody(_brand);
            packer.PackString(beer.Brand, Utf8);

            packer.PackRawBody(_brewery);
            packer.PackString(beer.Brewery, Utf8);

            packer.PackRawBody(_alcohol);
            packer.Pack(beer.Alcohol, OwnerContext);

            packer.PackRawBody(_sort);
            packer.Pack(beer.Sort, OwnerContext);
        }

        protected override Beer UnpackFromCore(Unpacker unpacker)
        {
            if (unpacker.LastReadData.IsNil)
            {
                return null;
            }

            if (!unpacker.IsMapHeader)
            {
                throw new SerializationException("Bad format");
            }

            var length = unpacker.Unpack<int>(OwnerContext);

            if (length != 4)
            {
                throw new SerializationException("Bad format");
            }

            var result = new Beer();
            unpacker.Read();
            for (var i = 0; i < length; i++)
            {
                var propertyName = unpacker.Unpack<string>(OwnerContext);
                unpacker.Read();
                switch (propertyName)
                {
                    case nameof(result.Brand):
                        result.Brand = unpacker.Unpack<string>(OwnerContext);
                        if (i + 1 < length)
                        {
                            unpacker.Read();
                        }
                        break;
                    case nameof(result.Sort):
                        result.Sort = unpacker.Unpack<List<string>>(OwnerContext);
                        if (i + 1 < length)
                        {
                            unpacker.Read();
                        }
                        break;
                    case nameof(result.Alcohol):
                        result.Alcohol = unpacker.Unpack<float>(OwnerContext);
                        if (i + 1 < length)
                        {
                            unpacker.Read();
                        }
                        break;
                    case nameof(result.Brewery):
                        result.Brewery = unpacker.Unpack<string>(OwnerContext);
                        if (i + 1 < length)
                        {
                            unpacker.Read();
                        }
                        break;
                    default:
                        throw new SerializationException("Bad format");
                }
            }

            return result;
        }
    }
}