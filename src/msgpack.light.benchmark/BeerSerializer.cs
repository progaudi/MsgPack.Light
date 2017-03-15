using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

using MsgPack;
using MsgPack.Serialization;

namespace ProGaudi.MsgPack.Light.benchmark
{
    internal class BeerSerializer : MessagePackSerializer<Beer>
    {
        private static readonly Encoding Utf8 = new UTF8Encoding(false);

        public BeerSerializer(SerializationContext ownerContext)
            : base(ownerContext)
        {
        }

        protected override void PackToCore(Packer packer, Beer beer)
        {
            if (beer == null)
            {
                packer.PackNull();
                return;
            }

            packer.PackMapHeader(4);

            packer.PackString(nameof(beer.Brand), Utf8);
            packer.PackString(beer.Brand, Utf8);

            packer.PackString(nameof(beer.Brewery), Utf8);
            packer.PackString(beer.Brewery, Utf8);

            packer.PackString(nameof(beer.Alcohol), Utf8);
            packer.Pack(beer.Alcohol, OwnerContext);

            packer.PackString(nameof(beer.Sort), Utf8);
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