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
Processor=Intel(R) Core(TM) i5-4590 CPU @ 3.30GHz, ProcessorCount=4
Frequency=3215208 ticks, Resolution=311.0219 ns, Timer=TSC
HostCLR=MS.NET 4.0.30319.42000, Arch=64-bit RELEASE [RyuJIT]
JitModules=clrjit-v4.6.1078.0

Type=IntSerialize  Mode=Throughput

```
         Method |     Median |    StdDev | Scaled |
--------------- |----------- |---------- |------- |
    MPCli_Array | 39.0271 us | 0.1559 us |   1.01 |
   MPCli_Stream | 38.6512 us | 0.1966 us |   1.00 |
  MPLight_Array | 44.3018 us | 0.2752 us |   1.15 |
 MPLight_Stream | 45.5666 us | 0.2001 us |   1.18 |


### Int array deserialize

```ini

BenchmarkDotNet=v0.9.5.0
OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-4590 CPU @ 3.30GHz, ProcessorCount=4
Frequency=3215208 ticks, Resolution=311.0219 ns, Timer=TSC
HostCLR=MS.NET 4.0.30319.42000, Arch=64-bit RELEASE [RyuJIT]
JitModules=clrjit-v4.6.1078.0

Type=IntDeserialize  Mode=Throughput

```
         Method |     Median |    StdDev | Scaled |
--------------- |----------- |---------- |------- |
    MPCli_Array | 42.8873 us | 0.3238 us |   0.99 |
   MPCli_Stream | 43.1403 us | 0.2773 us |   1.00 |
  MPLight_Array | 29.5179 us | 0.2694 us |   0.68 |
 MPLight_Stream | 37.7499 us | 0.3186 us |   0.88 |


### Complex object serialize

```ini

BenchmarkDotNet=v0.9.5.0
OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-4590 CPU @ 3.30GHz, ProcessorCount=4
Frequency=3215208 ticks, Resolution=311.0219 ns, Timer=TSC
HostCLR=MS.NET 4.0.30319.42000, Arch=64-bit RELEASE [RyuJIT]
JitModules=clrjit-v4.6.1078.0

Type=BeerSerializeBenchmark  Mode=Throughput

```
          Method |        Median |     StdDev | Scaled |
---------------- |-------------- |----------- |------- |
         JsonNet | 2,565.0263 ns | 32.9102 ns |   2.22 |
       JsonStack | 2,117.3641 ns | 14.1567 ns |   1.83 |
    MPCli_Stream | 1,154.6653 ns |  7.6785 ns |   1.00 |
     MPCli_Array | 1,232.0829 ns | 12.4839 ns |   1.07 |
  MPLight_Stream |   785.0415 ns |  6.6414 ns |   0.68 |
   MPLight_Array |   832.2704 ns |  5.9592 ns |   0.72 |
   MPCliH_Stream | 1,151.3963 ns |  8.8246 ns |   1.00 |
    MPCliH_Array | 1,233.6585 ns | 13.5660 ns |   1.07 |
 MPLightH_Stream |   589.4625 ns |  3.2217 ns |   0.51 |
  MPLightH_Array |   629.3650 ns |  3.4761 ns |   0.55 |


### Complex object deserialize

```ini

BenchmarkDotNet=v0.9.5.0
OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-4590 CPU @ 3.30GHz, ProcessorCount=4
Frequency=3215208 ticks, Resolution=311.0219 ns, Timer=TSC
HostCLR=MS.NET 4.0.30319.42000, Arch=64-bit RELEASE [RyuJIT]
JitModules=clrjit-v4.6.1078.0

Type=BeerDeserializeBenchmark  Mode=Throughput

```
          Method |        Median |     StdDev | Scaled |
---------------- |-------------- |----------- |------- |
         JsonNet | 2,738.1217 ns | 28.9291 ns |   0.79 |
       JsonStack | 1,349.2892 ns | 10.5102 ns |   0.39 |
    MPCli_Stream | 3,458.3789 ns | 33.1723 ns |   1.00 |
     MPCli_Array | 3,527.2680 ns | 41.9361 ns |   1.02 |
  MPLight_Stream |   902.2564 ns | 14.3799 ns |   0.26 |
   MPLight_Array |   695.4335 ns |  4.7616 ns |   0.20 |
   MPCliH_Stream | 3,486.6842 ns | 26.0948 ns |   1.01 |
    MPCliH_Array | 3,539.7104 ns | 38.8739 ns |   1.02 |
 MPLightH_Stream |   899.6040 ns |  4.9646 ns |   0.26 |
  MPLightH_Array |   700.9544 ns |  7.6878 ns |   0.20 |


### List of complex objects serialize

```ini

BenchmarkDotNet=v0.9.5.0
OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-4590 CPU @ 3.30GHz, ProcessorCount=4
Frequency=3215208 ticks, Resolution=311.0219 ns, Timer=TSC
HostCLR=MS.NET 4.0.30319.42000, Arch=64-bit RELEASE [RyuJIT]
JitModules=clrjit-v4.6.1078.0

Type=BeerListSerializeBenchmark  Mode=Throughput

```
          Method |    Median |    StdDev | Scaled |
---------------- |---------- |---------- |------- |
         JsonNet | 3.5874 ms | 0.0512 ms |   1.93 |
       JsonStack | 2.6559 ms | 0.0159 ms |   1.43 |
    MPCli_Stream | 1.8605 ms | 0.0140 ms |   1.00 |
     MPCli_Array | 1.9261 ms | 0.0099 ms |   1.04 |
  MPLight_Stream | 1.4107 ms | 0.0084 ms |   0.76 |
   MPLight_Array | 1.4498 ms | 0.0192 ms |   0.78 |
   MPCliH_Stream | 1.8561 ms | 0.0177 ms |   1.00 |
    MPCliH_Array | 1.9331 ms | 0.0125 ms |   1.04 |
 MPLightH_Stream | 1.0609 ms | 0.0076 ms |   0.57 |
  MPLightH_Array | 1.1557 ms | 0.0079 ms |   0.62 |


### List of complex objects deserialize

```ini

BenchmarkDotNet=v0.9.5.0
OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-4590 CPU @ 3.30GHz, ProcessorCount=4
Frequency=3215208 ticks, Resolution=311.0219 ns, Timer=TSC
HostCLR=MS.NET 4.0.30319.42000, Arch=64-bit RELEASE [RyuJIT]
JitModules=clrjit-v4.6.1078.0

Type=BeerListDeserializeBenchmark  Mode=Throughput

```
          Method |    Median |    StdDev | Scaled |
---------------- |---------- |---------- |------- |
         JsonNet | 3.5287 ms | 0.0269 ms |   0.53 |
       JsonStack | 2.3483 ms | 0.0463 ms |   0.35 |
    MPCli_Stream | 6.6458 ms | 0.0547 ms |   1.00 |
     MPCli_Array | 6.5375 ms | 0.0514 ms |   0.98 |
  MPLight_Stream | 1.7332 ms | 0.0158 ms |   0.26 |
   MPLight_Array | 1.3194 ms | 0.0147 ms |   0.20 |
   MPCliH_Stream | 6.6445 ms | 0.0556 ms |   1.00 |
    MPCliH_Array | 6.5601 ms | 0.0937 ms |   0.99 |
 MPLightH_Stream | 1.7421 ms | 0.0217 ms |   0.26 |
  MPLightH_Array | 1.3150 ms | 0.0115 ms |   0.20 |
