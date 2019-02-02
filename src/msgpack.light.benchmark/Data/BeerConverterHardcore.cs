using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ProGaudi.MsgPack.Light.Benchmark.Data
{
    internal sealed class BeerConverterHardCore : IMsgPackFormatter<Beer>, IMsgPackParser<Beer>
    {
        private static readonly byte[] Brand;
        private static readonly byte[] Alcohol;
        private static readonly byte[] Sort;
        private static readonly byte[] Brewery;

        private readonly IMsgPackFormatter<string> _stringFormatter;
        private readonly IMsgPackFormatter<List<string>> _listStringFormatter;
        private readonly IMsgPackFormatter<float> _floatFormatter;

        private readonly IMsgPackParser<string> _stringParser;
        private readonly IMsgPackParser<List<string>> _listStringParser;
        private readonly IMsgPackParser<float> _floatParser;

        static BeerConverterHardCore()
        {
            Brand = new byte[6];
            MsgPackSpec.WriteFixString(Brand, nameof(Beer.Brand).AsSpan());
            Alcohol = new byte[8];
            MsgPackSpec.WriteFixString(Alcohol, nameof(Beer.Alcohol).AsSpan());
            Sort = new byte[5];
            MsgPackSpec.WriteFixString(Sort, nameof(Beer.Sort).AsSpan());
            Brewery = new byte[8];
            MsgPackSpec.WriteFixString(Brewery, nameof(Beer.Brewery).AsSpan());
        }

        public BeerConverterHardCore(MsgPackContext context)
        {
            _stringFormatter = context.GetRequiredFormatter<string>();
            _listStringFormatter = context.GetRequiredFormatter<List<string>>();
            _floatFormatter = context.GetRequiredFormatter<float>();

            _stringParser = context.GetRequiredParser<string>();
            _listStringParser = context.GetRequiredParser<List<string>>();
            _floatParser = context.GetRequiredParser<float>();
        }

        public int GetBufferSize(Beer value) => value == null
            ? DataLengths.Nil
            : _stringFormatter.GetBufferSize(value.Brand)
            + _stringFormatter.GetBufferSize(value.Brewery)
            + _listStringFormatter.GetBufferSize(value.Sort)
            + _floatFormatter.GetBufferSize(value.Alcohol)
            + 27;

        public bool HasConstantSize => false;

        public int Format(Span<byte> destination, Beer value)
        {
            if (value == null) return MsgPackSpec.WriteNil(destination);

            var result = MsgPackSpec.WriteMapHeader(destination, 4);

            Brand.CopyTo(destination.Slice(result));
            result += Brand.Length;
            result += _stringFormatter.Format(destination, value.Brand);

            Sort.CopyTo(destination.Slice(result));
            result += Sort.Length;
            result += _listStringFormatter.Format(destination, value.Sort);

            Alcohol.CopyTo(destination.Slice(result));
            result += Alcohol.Length;
            result += _floatFormatter.Format(destination, value.Alcohol);

            Brewery.CopyTo(destination.Slice(result));
            result += Brewery.Length;
            result += _stringFormatter.Format(destination, value.Brewery);

            return result;
        }

        public Beer Parse(ReadOnlySpan<byte> source, out int readSize)
        {
            if (MsgPackSpec.TryReadNil(source, out readSize)) return null;

            var length = MsgPackSpec.ReadMapHeader(source, out readSize);
            if (length != 4)
            {
                throw new SerializationException("Bad format");
            }

            var result = new Beer();
            for (var i = 0; i < length; i++)
            {
                var propertyName = _stringParser.Parse(source, out var temp);
                readSize += temp;
                switch (propertyName)
                {
                    case nameof(result.Brand):
                        result.Brand = _stringParser.Parse(source, out temp);
                        readSize += temp;
                        break;
                    case nameof(result.Sort):
                        result.Sort = _listStringParser.Parse(source, out temp);
                        readSize += temp;
                        break;
                    case nameof(result.Alcohol):
                        result.Alcohol = _floatParser.Parse(source, out temp);
                        readSize += temp;
                        break;
                    case nameof(result.Brewery):
                        result.Brewery = _stringParser.Parse(source, out temp);
                        readSize += temp;
                        break;
                    default:
                        throw new SerializationException("Bad format");
                }
            }

            return result;
        }
    }
}
