# MsgPack.Light

MsgPack.Light is a lightweight [MsgPack](http://msgpack.org/) serialization library.

## Key features
* Performance
* .Net core compatability
* Extensibility
* Simple usage

## Install
Simpliest way to start using ProGaudi.MsgPack.Light is to install it from [NuGet](https://www.nuget.org/packages/MsgPack.Light/).

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
If you want to work with your own types, first thing you need - type converter.
Example of Beer type converter you can find [here](https://github.com/progaudi/MsgPack.Light/blob/master/VS/src/msgpack.light.benchmark/Beer.cs).
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

## Build
To build MsgPack.Light you have to install dotnet SDK version ```1.0.0-preview4-004233``` or higher (you can find corresponding build on [dotnet/cli project](https://github.com/dotnet/cli).
After dotnet is installed, run following commands:
```
git clone https://github.com/progaudi/MsgPack.Light.git
cd MsgPack.Light
dotnet build -c Release -f netstandard1.4 src/msgpack.light/msgpack.light.csproj
dotnet build -c Release -f netcoreapp1.0 tests/msgpack.light.tests/msgpack.light.tests.csproj
dotnet test -c Release --noBuild tests/msgpack.light.tests/msgpack.light.tests.csproj
```
MsgPack.Light should built and all test should be passed.

## Performance
* Serialization performance is comparable with msgpack.cli
* Deserialization performance 2-3 times faster
* MsgPack.Light works best if a data reside a memory (\_*Array benchmarks).
* Perfoming some IO operations, performance is suboptimal, but comparable with MsgPack.Cli (\*_Stream benchmarks).
* More details can be found [here](https://github.com/progaudi/MsgPack.Light/blob/master/benchmarks.md).

## Credits
* Benchmark data copied from [thekvs cpp-serializers project](https://github.com/thekvs/cpp-serializers/blob/c6b305fe3659d2df3b492698bb5d7cb284ab2f9f/data.hpp).
* Other benchmarks got from [maximn SerializationPerformanceTest_CSharp project](https://github.com/maximn/SerializationPerformanceTest_CSharp).
* Thanks to [MessagePack for CLI](https://github.com/msgpack/msgpack-cli) authors for inspiration.

## Roadmap
* Code-generator for field-based converters, we will support array and map modes.
* Possible optimizations with IO handling and asyncing our API.


## Build statuses for master branch

Linux and OSX build status (it's not possible to separate build status per OS, so if any OS is failing build status will be failing):

[![Linux and OSX build status](https://travis-ci.org/progaudi/MsgPack.Light.svg?branch=master)](https://travis-ci.org/progaudi/MsgPack.Light)
