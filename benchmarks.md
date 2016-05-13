## Building and running

Open solution in VS folder, build benchmark console app, close VS, run app, choose benchmark, wait.

## Data

All objects serialized in map mode with custom serializer (except for jsons, they use default). We do this,
because in our projects we prefer be safe than sorry and use map mode of serialization for backward and forward
compatibility on data.

Int arrays of course serialized as int arrays.

## Results

### Int array serialize

```ini

BenchmarkDotNet=v0.9.5.0
OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-4300U CPU @ 1.90GHz, ProcessorCount=4
Frequency=2435767 ticks, Resolution=410.5483 ns, Timer=TSC
HostCLR=MS.NET 4.0.30319.42000, Arch=64-bit RELEASE [RyuJIT]
JitModules=clrjit-v4.6.1080.0

Type=IntSerialize  Mode=Throughput  

```
         Method |      Median |    StdDev | Scaled |
--------------- |------------ |---------- |------- |
    MPCli_Array | 385.5996 ns | 7.8754 ns |   1.16 |
   MPCli_Stream | 333.3653 ns | 3.7062 ns |   1.00 |
  MPLight_Array | 477.7349 ns | 7.1181 ns |   1.43 |
 MPLight_Stream | 438.5360 ns | 7.7678 ns |   1.32 |


### Int array deserialize

```ini

BenchmarkDotNet=v0.9.5.0
OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-4300U CPU @ 1.90GHz, ProcessorCount=4
Frequency=2435767 ticks, Resolution=410.5483 ns, Timer=TSC
HostCLR=MS.NET 4.0.30319.42000, Arch=64-bit RELEASE [RyuJIT]
JitModules=clrjit-v4.6.1080.0

Type=IntDeserialize  Mode=Throughput  

```
         Method |      Median |     StdDev | Scaled |
--------------- |------------ |----------- |------- |
    MPCli_Array | 435.0537 ns | 41.2969 ns |   1.18 |
   MPCli_Stream | 369.6012 ns |  6.5736 ns |   1.00 |
  MPLight_Array | 290.2282 ns |  2.3186 ns |   0.79 |
 MPLight_Stream | 359.7817 ns | 24.2372 ns |   0.97 |


### Double array serialize

```ini

BenchmarkDotNet=v0.9.5.0
OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-4300U CPU @ 1.90GHz, ProcessorCount=4
Frequency=2435767 ticks, Resolution=410.5483 ns, Timer=TSC
HostCLR=MS.NET 4.0.30319.42000, Arch=64-bit RELEASE [RyuJIT]
JitModules=clrjit-v4.6.1080.0

Type=DoubleSerialize  Mode=Throughput  

```
         Method |      Median |     StdDev | Scaled |
--------------- |------------ |----------- |------- |
    MPCli_Array | 720.6631 ns | 14.3266 ns |   1.06 |
   MPCli_Stream | 681.4267 ns |  2.1806 ns |   1.00 |
  MPLight_Array | 694.9290 ns |  7.8209 ns |   1.02 |
 MPLight_Stream | 654.0637 ns | 10.3032 ns |   0.96 |


###Double array deserialize

```ini

BenchmarkDotNet=v0.9.5.0
OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-4300U CPU @ 1.90GHz, ProcessorCount=4
Frequency=2435767 ticks, Resolution=410.5483 ns, Timer=TSC
HostCLR=MS.NET 4.0.30319.42000, Arch=64-bit RELEASE [RyuJIT]
JitModules=clrjit-v4.6.1080.0

Type=DoubleDeserialize  Mode=Throughput  

```
         Method |      Median |    StdDev | Scaled |
--------------- |------------ |---------- |------- |
    MPCli_Array | 491.0816 ns | 7.7126 ns |   1.08 |
   MPCli_Stream | 452.7518 ns | 6.9093 ns |   1.00 |
  MPLight_Array | 369.8179 ns | 5.9467 ns |   0.82 |
 MPLight_Stream | 532.9527 ns | 8.2603 ns |   1.18 |


### Complex object serialize

```ini

BenchmarkDotNet=v0.9.5.0
OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-4300U CPU @ 1.90GHz, ProcessorCount=4
Frequency=2435769 ticks, Resolution=410.5480 ns, Timer=TSC
HostCLR=MS.NET 4.0.30319.42000, Arch=64-bit RELEASE [RyuJIT]
JitModules=clrjit-v4.6.1078.0

Type=BeerSerializeBenchmark  Mode=Throughput  

```
          Method |        Median |      StdDev | Scaled |
---------------- |-------------- |------------ |------- |
         JsonNet | 3,234.4935 ns | 115.3640 ns |   2.19 |
       JsonStack | 2,670.0308 ns | 300.9519 ns |   1.81 |
    MPCli_Stream | 1,475.7027 ns |  14.1321 ns |   1.00 |
     MPCli_Array | 1,523.4873 ns |  45.4226 ns |   1.03 |
  MPLight_Stream |   958.7564 ns |  94.3329 ns |   0.65 |
   MPLight_Array | 1,043.7945 ns |  34.4739 ns |   0.71 |
   MPCliH_Stream | 1,465.7246 ns |  15.7688 ns |   0.99 |
    MPCliH_Array | 1,514.8384 ns |  28.5944 ns |   1.03 |
 MPLightH_Stream |   742.1639 ns |  15.7234 ns |   0.50 |
  MPLightH_Array |   798.1476 ns |  17.6526 ns |   0.54 |


### Complex object deserialize

```ini

BenchmarkDotNet=v0.9.5.0
OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-4300U CPU @ 1.90GHz, ProcessorCount=4
Frequency=2435769 ticks, Resolution=410.5480 ns, Timer=TSC
HostCLR=MS.NET 4.0.30319.42000, Arch=64-bit RELEASE [RyuJIT]
JitModules=clrjit-v4.6.1078.0

Type=BeerDeserializeBenchmark  Mode=Throughput  

```
          Method |        Median |      StdDev | Scaled |
---------------- |-------------- |------------ |------- |
         JsonNet | 3,546.7223 ns | 206.4157 ns |   0.80 |
       JsonStack | 1,736.9889 ns |  15.6411 ns |   0.39 |
    MPCli_Stream | 4,432.1433 ns |  46.5557 ns |   1.00 |
     MPCli_Array | 4,462.4930 ns |  48.4290 ns |   1.01 |
  MPLight_Stream | 1,118.3174 ns |  18.2712 ns |   0.25 |
   MPLight_Array |   962.6157 ns |  17.0023 ns |   0.22 |
   MPCliH_Stream | 4,406.9351 ns |  85.7725 ns |   0.99 |
    MPCliH_Array | 4,555.8163 ns |  57.3318 ns |   1.03 |
 MPLightH_Stream | 1,176.6035 ns |  45.5205 ns |   0.27 |
  MPLightH_Array |   888.6308 ns |  25.0875 ns |   0.20 |


### List of complex objects serialize

```ini

BenchmarkDotNet=v0.9.5.0
OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-4300U CPU @ 1.90GHz, ProcessorCount=4
Frequency=2435767 ticks, Resolution=410.5483 ns, Timer=TSC
HostCLR=MS.NET 4.0.30319.42000, Arch=64-bit RELEASE [RyuJIT]
JitModules=clrjit-v4.6.1080.0

Type=BeerListSerializeBenchmark  Mode=Throughput  

```
          Method |     Median |    StdDev | Scaled |
---------------- |----------- |---------- |------- |
         JsonNet | 24.5087 us | 2.0821 us |   2.24 |
       JsonStack | 15.7441 us | 1.6228 us |   1.44 |
    MPCli_Stream | 10.9527 us | 0.5778 us |   1.00 |
     MPCli_Array | 10.7575 us | 0.3782 us |   0.98 |
  MPLight_Stream |  7.7205 us | 0.0801 us |   0.70 |
   MPLight_Array |  6.8955 us | 0.1074 us |   0.63 |
   MPCliH_Stream |  9.4939 us | 0.1882 us |   0.87 |
    MPCliH_Array |  9.7598 us | 0.5073 us |   0.89 |
 MPLightH_Stream |  5.0625 us | 0.0388 us |   0.46 |
  MPLightH_Array |  5.1826 us | 0.0411 us |   0.47 |


### List of complex objects deserialize

```ini

BenchmarkDotNet=v0.9.5.0
OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-4300U CPU @ 1.90GHz, ProcessorCount=4
Frequency=2435767 ticks, Resolution=410.5483 ns, Timer=TSC
HostCLR=MS.NET 4.0.30319.42000, Arch=64-bit RELEASE [RyuJIT]
JitModules=clrjit-v4.6.1080.0

Type=BeerListDeserializeBenchmark  Mode=Throughput  

```
          Method |     Median |    StdDev | Scaled |
---------------- |----------- |---------- |------- |
         JsonNet | 20.4582 us | 0.2415 us |   0.61 |
       JsonStack | 10.8642 us | 0.2068 us |   0.32 |
    MPCli_Stream | 33.4607 us | 0.7212 us |   1.00 |
     MPCli_Array | 33.8802 us | 0.4199 us |   1.01 |
  MPLight_Stream |  8.5858 us | 0.0844 us |   0.26 |
   MPLight_Array |  6.5429 us | 0.1721 us |   0.20 |
   MPCliH_Stream | 34.0760 us | 6.1497 us |   1.02 |
    MPCliH_Array | 34.6421 us | 6.1892 us |   1.04 |
 MPLightH_Stream |  8.6054 us | 0.7411 us |   0.26 |
  MPLightH_Array |  6.5587 us | 1.1443 us |   0.20 |


### Object skip

```ini

BenchmarkDotNet=v0.9.5.0
OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-4300U CPU @ 1.90GHz, ProcessorCount=4
Frequency=2435767 ticks, Resolution=410.5483 ns, Timer=TSC
HostCLR=MS.NET 4.0.30319.42000, Arch=64-bit RELEASE [RyuJIT]
JitModules=clrjit-v4.6.1080.0

Type=BeerSkip  Mode=Throughput  

```
                   Method |      Median |     StdDev | Scaled |
------------------------- |------------ |----------- |------- |
            MPackCli_Skip | 362.0120 ns | 15.8975 ns |   1.00 |
 MsgPackLight_Skip_Stream | 454.8119 ns |  5.8138 ns |   1.26 |
  MsgPackLight_Skip_Array | 410.8044 ns |  5.4830 ns |   1.13 |


### Objects list skip

```ini

BenchmarkDotNet=v0.9.5.0
OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-4300U CPU @ 1.90GHz, ProcessorCount=4
Frequency=2435767 ticks, Resolution=410.5483 ns, Timer=TSC
HostCLR=MS.NET 4.0.30319.42000, Arch=64-bit RELEASE [RyuJIT]
JitModules=clrjit-v4.6.1080.0

Type=BeerSkipList  Mode=Throughput  

```
                   Method |    Median |    StdDev | Scaled |
------------------------- |---------- |---------- |------- |
            MPackCli_Skip | 2.6013 us | 0.1113 us |   1.00 |
 MsgPackLight_Skip_Stream | 2.8278 us | 0.0497 us |   1.09 |
  MsgPackLight_Skip_Array | 2.5962 us | 0.0371 us |   1.00 |
