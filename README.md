# MsgPack.Light
MsgPack.Light is a lightweight [MsgPack](http://msgpack.org/) serialization library.

## Key features
* Performance
* .Net core compatability
* Extensibility
* Simple usage

## Install
Simpliest way to start using MsgPack.Light is to install it from [NuGet](https://www.nuget.org/packages/MsgPack.Light/).

## Usage
### Serialization to bytes array:
```C#
var bytes = MsgPackSerializer.Serialize(value);
```

### Deserialization:
```C#
var value = MsgPackSerializer.Deserialize<string>(bytes);
```

### Your type serialization/deserialization:
For example you would like to serialize your own type:
```C#
public class Beer
  {
      public string Brand { get; set; }

      public List<string> Sort { get; set; }

      public float Alcohol { get; set; }

      public string Brewery { get; set; }
  }
```

Firstly, implement IMsgPackConverter<> interface:
```C#
 public class BeerConverter : IMsgPackConverter<Beer>
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
```

Then you should create create MsgPackContext and register your converter:
```C#
var context= new MsgPackContext();
context.RegisterConverter(new BeerConverter());
```

And then you can serialize:
```C#
var bytes = MsgPackSerializer.Serialize(beer, context);
```

And deserialize:
```C#
var beer = MsgPackSerializer.Deserialize<Beer>(bytes, context);
```

## Performance
* Serialization performance is comparable with msgpack.cli
* Deserialization performance 2-3 times faster
* MsgPack.Light works best if a data reside a memory (\_*Array benchmarks).
* Perfoming some IO operations, performance is suboptimal, but comparable with MsgPack.Cli (\*_Stream benchmarks).
* More details can be found [here](https://github.com/roman-kozachenko/MsgPack.Light/blob/master/benchmarks.md).

## Credits
* Benchmark data copied from [thekvs cpp-serializers project](https://github.com/thekvs/cpp-serializers/blob/c6b305fe3659d2df3b492698bb5d7cb284ab2f9f/data.hpp).
* Thanks to [MessagePack for CLI](https://github.com/msgpack/msgpack-cli) authors for inspiration.

## Roadmap
* Code-generator for field-based converters, we will support array and map modes.
* Possible optimizations with IO handling and asyncing our API.


## Build statuses for master branch

Windows build status:

[![Windows build status](https://ci.appveyor.com/api/projects/status/42f0d1sdyn5kkcpc?svg=true)](https://ci.appveyor.com/project/roman-kozachenko/msgpack-light/branch/master)

Linux and OSX build status (it's not possible to separate build status per OS, so if any OS is failing build status will be failing):

[![Linux and OSX build status](https://travis-ci.org/roman-kozachenko/MsgPack.Light.svg?branch=master)](https://travis-ci.org/roman-kozachenko/MsgPack.Light)
