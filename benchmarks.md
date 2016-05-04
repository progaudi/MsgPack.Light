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
Frequency=2435769 ticks, Resolution=410.5480 ns, Timer=TSC
HostCLR=MS.NET 4.0.30319.42000, Arch=64-bit RELEASE [RyuJIT]
JitModules=clrjit-v4.6.1078.0

Type=IntSerialize  Mode=Throughput  

```
         Method |     Median |    StdDev | Scaled |
--------------- |----------- |---------- |------- |
    MPCli_Array | 44.2989 us | 0.8690 us |   1.01 |
   MPCli_Stream | 43.8341 us | 2.5219 us |   1.00 |
  MPLight_Array | 48.7440 us | 2.4879 us |   1.11 |
 MPLight_Stream | 50.4261 us | 1.0596 us |   1.15 |


### Int array deserialize

```ini

BenchmarkDotNet=v0.9.5.0
OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-4300U CPU @ 1.90GHz, ProcessorCount=4
Frequency=2435769 ticks, Resolution=410.5480 ns, Timer=TSC
HostCLR=MS.NET 4.0.30319.42000, Arch=64-bit RELEASE [RyuJIT]
JitModules=clrjit-v4.6.1078.0

Type=IntDeserialize  Mode=Throughput  

```
         Method |     Median |    StdDev | Scaled |
--------------- |----------- |---------- |------- |
    MPCli_Array | 54.3273 us | 0.9087 us |   1.03 |
   MPCli_Stream | 52.6379 us | 1.2348 us |   1.00 |
  MPLight_Array | 36.7169 us | 0.7710 us |   0.70 |
 MPLight_Stream | 47.5450 us | 0.3847 us |   0.90 |


### Double array serialize

```ini

BenchmarkDotNet=v0.9.5.0
OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-4300U CPU @ 1.90GHz, ProcessorCount=4
Frequency=2435769 ticks, Resolution=410.5480 ns, Timer=TSC
HostCLR=MS.NET 4.0.30319.42000, Arch=64-bit RELEASE [RyuJIT]
JitModules=clrjit-v4.6.1078.0

Type=DoubleSerialize  Mode=Throughput  

```
         Method |     Median |    StdDev | Scaled |
--------------- |----------- |---------- |------- |
    MPCli_Array | 83.8307 us | 4.6941 us |   1.02 |
   MPCli_Stream | 82.5322 us | 7.4756 us |   1.00 |
  MPLight_Array | 54.4167 us | 8.5853 us |   0.66 |
 MPLight_Stream | 53.8102 us | 9.0536 us |   0.65 |


###Double array deserialize

```ini

BenchmarkDotNet=v0.9.5.0
OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-4300U CPU @ 1.90GHz, ProcessorCount=4
Frequency=2435769 ticks, Resolution=410.5480 ns, Timer=TSC
HostCLR=MS.NET 4.0.30319.42000, Arch=64-bit RELEASE [RyuJIT]
JitModules=clrjit-v4.6.1078.0

Type=DoubleDeserialize  Mode=Throughput  

```
         Method |     Median |    StdDev | Scaled |
--------------- |----------- |---------- |------- |
    MPCli_Array | 61.2256 us | 1.1206 us |   0.98 |
   MPCli_Stream | 62.5016 us | 1.5110 us |   1.00 |
  MPLight_Array | 47.8698 us | 0.8309 us |   0.77 |
 MPLight_Stream | 77.0925 us | 1.2832 us |   1.23 |


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
Frequency=2435769 ticks, Resolution=410.5480 ns, Timer=TSC
HostCLR=MS.NET 4.0.30319.42000, Arch=64-bit RELEASE [RyuJIT]
JitModules=clrjit-v4.6.1078.0

Type=BeerListSerializeBenchmark  Mode=Throughput  

```
          Method |    Median |    StdDev | Scaled |
---------------- |---------- |---------- |------- |
         JsonNet | 4.5902 ms | 0.1218 ms |   1.94 |
       JsonStack | 3.5143 ms | 0.2123 ms |   1.48 |
    MPCli_Stream | 2.3717 ms | 0.2183 ms |   1.00 |
     MPCli_Array | 2.5020 ms | 0.2045 ms |   1.05 |
  MPLight_Stream | 1.8023 ms | 0.1722 ms |   0.76 |
   MPLight_Array | 1.9056 ms | 0.2487 ms |   0.80 |
   MPCliH_Stream | 2.3718 ms | 0.0365 ms |   1.00 |
    MPCliH_Array | 2.5071 ms | 0.0263 ms |   1.06 |
 MPLightH_Stream | 1.3780 ms | 0.0177 ms |   0.58 |
  MPLightH_Array | 1.4804 ms | 0.0082 ms |   0.62 |


### List of complex objects deserialize

```ini

BenchmarkDotNet=v0.9.5.0
OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-4300U CPU @ 1.90GHz, ProcessorCount=4
Frequency=2435769 ticks, Resolution=410.5480 ns, Timer=TSC
HostCLR=MS.NET 4.0.30319.42000, Arch=64-bit RELEASE [RyuJIT]
JitModules=clrjit-v4.6.1078.0

Type=BeerListDeserializeBenchmark  Mode=Throughput  

```
          Method |    Median |    StdDev | Scaled |
---------------- |---------- |---------- |------- |
         JsonNet | 4.6915 ms | 0.3156 ms |   0.54 |
       JsonStack | 2.9909 ms | 0.0483 ms |   0.35 |
    MPCli_Stream | 8.6678 ms | 0.1343 ms |   1.00 |
     MPCli_Array | 8.6034 ms | 0.0804 ms |   0.99 |
  MPLight_Stream | 2.4119 ms | 0.0430 ms |   0.28 |
   MPLight_Array | 1.6990 ms | 0.0452 ms |   0.20 |
   MPCliH_Stream | 8.5637 ms | 0.1457 ms |   0.99 |
    MPCliH_Array | 8.5886 ms | 0.1011 ms |   0.99 |
 MPLightH_Stream | 2.4198 ms | 0.1155 ms |   0.28 |
  MPLightH_Array | 1.6885 ms | 0.0280 ms |   0.19 |

