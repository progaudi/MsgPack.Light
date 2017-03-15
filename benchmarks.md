## Building and running

Open solution in VS folder, build benchmark console app, close VS, run app, choose benchmark, wait.

## Data

All objects serialized in map mode with custom serializer (except for jsons, they use default). We do this,
because in our projects we prefer be safe than sorry and use map mode of serialization for backward and forward
compatibility on data.

Int arrays of course serialized as int arrays.

## Results

### Int array serialize

``` ini

BenchmarkDotNet=v0.10.3.0, OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-6600 CPU 3.30GHz, ProcessorCount=4
Frequency=3234373 Hz, Resolution=309.1789 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0


```
 |         Method |        Mean |    StdDev | Scaled | Scaled-StdDev |
 |--------------- |------------ |---------- |------- |-------------- |
 |    MPCli_Array | 326.5151 ns | 1.2686 ns |   1.15 |          0.01 |
 |   MPCli_Stream | 284.0344 ns | 3.2835 ns |   1.00 |          0.00 |
 |  MPLight_Array | 719.9674 ns | 5.3835 ns |   2.54 |          0.03 |
 | MPLight_Stream | 693.6267 ns | 5.8638 ns |   2.44 |          0.03 |


### Int array deserialize

``` ini

BenchmarkDotNet=v0.10.3.0, OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-6600 CPU 3.30GHz, ProcessorCount=4
Frequency=3234373 Hz, Resolution=309.1789 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0


```
 |         Method |        Mean |    StdDev | Scaled | Scaled-StdDev |
 |--------------- |------------ |---------- |------- |-------------- |
 |    MPCli_Array | 398.0515 ns | 8.6489 ns |   1.26 |          0.03 |
 |   MPCli_Stream | 316.7317 ns | 5.6249 ns |   1.00 |          0.00 |
 |  MPLight_Array | 285.9894 ns | 3.0310 ns |   0.90 |          0.02 |
 | MPLight_Stream | 337.5709 ns | 2.2369 ns |   1.07 |          0.02 |


### Double array serialize

``` ini

BenchmarkDotNet=v0.10.3.0, OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-6600 CPU 3.30GHz, ProcessorCount=4
Frequency=3234373 Hz, Resolution=309.1789 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0


```
 |         Method |        Mean |    StdDev | Scaled | Scaled-StdDev |
 |--------------- |------------ |---------- |------- |-------------- |
 |    MPCli_Array | 594.9763 ns | 9.2855 ns |   1.08 |          0.02 |
 |   MPCli_Stream | 549.9461 ns | 9.3395 ns |   1.00 |          0.00 |
 |  MPLight_Array | 573.7299 ns | 8.8065 ns |   1.04 |          0.02 |
 | MPLight_Stream | 538.2552 ns | 2.4989 ns |   0.98 |          0.02 |


###Double array deserialize

``` ini

BenchmarkDotNet=v0.10.3.0, OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-6600 CPU 3.30GHz, ProcessorCount=4
Frequency=3234373 Hz, Resolution=309.1789 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0


```
 |         Method |        Mean |    StdDev | Scaled | Scaled-StdDev |
 |--------------- |------------ |---------- |------- |-------------- |
 |    MPCli_Array | 421.9673 ns | 7.9321 ns |   1.03 |          0.02 |
 |   MPCli_Stream | 410.8337 ns | 6.4082 ns |   1.00 |          0.00 |
 |  MPLight_Array | 312.5716 ns | 3.8393 ns |   0.76 |          0.01 |
 | MPLight_Stream | 443.7935 ns | 4.1870 ns |   1.08 |          0.02 |


### Complex object serialize

``` ini

BenchmarkDotNet=v0.10.3.0, OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-6600 CPU 3.30GHz, ProcessorCount=4
Frequency=3234373 Hz, Resolution=309.1789 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0


```
 |          Method |          Mean |     StdDev | Scaled | Scaled-StdDev |
 |---------------- |-------------- |----------- |------- |-------------- |
 |         JsonNet | 2,954.5610 ns | 34.1478 ns |   2.42 |          0.03 |
 |    MPCli_Stream | 1,219.1689 ns | 11.3381 ns |   1.00 |          0.00 |
 |     MPCli_Array | 1,273.8864 ns |  9.9659 ns |   1.04 |          0.01 |
 |  MPLight_Stream |   801.8811 ns |  8.6501 ns |   0.66 |          0.01 |
 |   MPLight_Array |   846.8823 ns |  7.7187 ns |   0.69 |          0.01 |
 |   MPCliH_Stream | 1,244.8945 ns |  5.3075 ns |   1.02 |          0.01 |
 |    MPCliH_Array | 1,269.9564 ns | 24.1021 ns |   1.04 |          0.02 |
 | MPLightH_Stream |   582.3763 ns | 10.3649 ns |   0.48 |          0.01 |
 |  MPLightH_Array |   631.9099 ns |  7.6106 ns |   0.52 |          0.01 |


### Complex object deserialize

``` ini

BenchmarkDotNet=v0.10.3.0, OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-6600 CPU 3.30GHz, ProcessorCount=4
Frequency=3234373 Hz, Resolution=309.1789 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0


```
 |          Method |          Mean |     StdDev | Scaled | Scaled-StdDev |
 |---------------- |-------------- |----------- |------- |-------------- |
 |         JsonNet | 3,171.1121 ns | 48.0032 ns |   0.90 |          0.02 |
 |    MPCli_Stream | 3,533.6110 ns | 53.3277 ns |   1.00 |          0.00 |
 |     MPCli_Array | 3,582.0768 ns | 31.1420 ns |   1.01 |          0.02 |
 |  MPLight_Stream |   927.4914 ns |  8.1517 ns |   0.26 |          0.00 |
 |   MPLight_Array |   738.3759 ns |  4.2332 ns |   0.21 |          0.00 |
 |   MPCliH_Stream | 3,646.5071 ns | 45.1809 ns |   1.03 |          0.02 |
 |    MPCliH_Array | 3,793.1899 ns | 45.6794 ns |   1.07 |          0.02 |
 | MPLightH_Stream |   968.9605 ns | 20.1985 ns |   0.27 |          0.01 |
 |  MPLightH_Array |   736.3122 ns |  7.3144 ns |   0.21 |          0.00 |


### List of complex objects serialize

``` ini

BenchmarkDotNet=v0.10.3.0, OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-6600 CPU 3.30GHz, ProcessorCount=4
Frequency=3234373 Hz, Resolution=309.1789 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0


```
 |          Method |       Mean |    StdDev | Scaled | Scaled-StdDev |
 |---------------- |----------- |---------- |------- |-------------- |
 |         JsonNet | 16.7256 us | 0.5130 us |   2.20 |          0.07 |
 |    MPCli_Stream |  7.6166 us | 0.0972 us |   1.00 |          0.00 |
 |     MPCli_Array |  7.6824 us | 0.1639 us |   1.01 |          0.02 |
 |  MPLight_Stream |  5.4751 us | 0.1097 us |   0.72 |          0.02 |
 |   MPLight_Array |  5.5560 us | 0.0854 us |   0.73 |          0.01 |
 |   MPCliH_Stream |  7.8998 us | 0.0422 us |   1.04 |          0.01 |
 |    MPCliH_Array |  7.9609 us | 0.0502 us |   1.05 |          0.01 |
 | MPLightH_Stream |  3.8676 us | 0.0640 us |   0.51 |          0.01 |
 |  MPLightH_Array |  4.1350 us | 0.0561 us |   0.54 |          0.01 |


### List of complex objects deserialize

``` ini

BenchmarkDotNet=v0.10.3.0, OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-6600 CPU 3.30GHz, ProcessorCount=4
Frequency=3234373 Hz, Resolution=309.1789 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0


```
 |          Method |       Mean |    StdDev | Scaled | Scaled-StdDev |
 |---------------- |----------- |---------- |------- |-------------- |
 |         JsonNet | 17.5222 us | 0.2132 us |   0.64 |          0.01 |
 |    MPCli_Stream | 27.5270 us | 0.3796 us |   1.00 |          0.00 |
 |     MPCli_Array | 28.2738 us | 0.2978 us |   1.03 |          0.02 |
 |  MPLight_Stream |  7.0688 us | 0.0684 us |   0.26 |          0.00 |
 |   MPLight_Array |  5.4291 us | 0.0576 us |   0.20 |          0.00 |
 |   MPCliH_Stream | 28.0262 us | 0.6462 us |   1.02 |          0.03 |
 |    MPCliH_Array | 27.9221 us | 0.4272 us |   1.01 |          0.02 |
 | MPLightH_Stream |  7.1373 us | 0.0661 us |   0.26 |          0.00 |
 |  MPLightH_Array |  5.2915 us | 0.1143 us |   0.19 |          0.00 |


### Object skip

``` ini

BenchmarkDotNet=v0.10.3.0, OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-6600 CPU 3.30GHz, ProcessorCount=4
Frequency=3234373 Hz, Resolution=309.1789 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0


```
 |                   Method |        Mean |    StdDev | Scaled | Scaled-StdDev |
 |------------------------- |------------ |---------- |------- |-------------- |
 |            MPackCli_Skip | 349.5143 ns | 5.0397 ns |   1.00 |          0.00 |
 | MsgPackLight_Skip_Stream | 388.7140 ns | 3.2076 ns |   1.11 |          0.02 |
 |  MsgPackLight_Skip_Array | 352.9610 ns | 2.2396 ns |   1.01 |          0.02 |


### Objects list skip

``` ini

BenchmarkDotNet=v0.10.3.0, OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-6600 CPU 3.30GHz, ProcessorCount=4
Frequency=3234373 Hz, Resolution=309.1789 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0


```
 |                   Method |      Mean |    StdDev | Scaled | Scaled-StdDev |
 |------------------------- |---------- |---------- |------- |-------------- |
 |            MPackCli_Skip | 2.5750 us | 0.0551 us |   1.00 |          0.00 |
 | MsgPackLight_Skip_Stream | 2.5163 us | 0.0266 us |   0.98 |          0.02 |
 |  MsgPackLight_Skip_Array | 2.2537 us | 0.0324 us |   0.88 |          0.02 |
