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
 |    MPCli_Array | 317.9976 ns | 2.2592 ns |   1.18 |          0.01 |
 |   MPCli_Stream | 270.1779 ns | 0.6756 ns |   1.00 |          0.00 |
 |  MPLight_Array | 483.8765 ns | 2.0833 ns |   1.79 |          0.01 |
 | MPLight_Stream | 457.0131 ns | 5.8785 ns |   1.69 |          0.02 |
 

### Int array deserialize

``` ini

BenchmarkDotNet=v0.10.3.0, OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-6600 CPU 3.30GHz, ProcessorCount=4
Frequency=3234373 Hz, Resolution=309.1789 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0


```
 |         Method |        Mean |     StdDev | Scaled | Scaled-StdDev |
 |--------------- |------------ |----------- |------- |-------------- |
 |    MPCli_Array | 379.2173 ns |  2.5562 ns |   1.19 |          0.01 |
 |   MPCli_Stream | 319.7132 ns |  1.6539 ns |   1.00 |          0.00 |
 |  MPLight_Array | 707.9764 ns |  4.4916 ns |   2.21 |          0.02 |
 | MPLight_Stream | 826.0472 ns | 20.3832 ns |   2.58 |          0.06 |


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
 |    MPCli_Array | 597.2335 ns | 4.0136 ns |   1.08 |          0.01 |
 |   MPCli_Stream | 552.5873 ns | 1.8813 ns |   1.00 |          0.00 |
 |  MPLight_Array | 704.1176 ns | 3.1067 ns |   1.27 |          0.01 |
 | MPLight_Stream | 651.5843 ns | 6.7932 ns |   1.18 |          0.01 |


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
 |    MPCli_Array | 425.8500 ns | 2.9099 ns |   1.14 |          0.01 |
 |   MPCli_Stream | 374.5874 ns | 1.3184 ns |   1.00 |          0.00 |
 |  MPLight_Array | 618.4924 ns | 2.8508 ns |   1.65 |          0.01 |
 | MPLight_Stream | 795.7636 ns | 4.1306 ns |   2.12 |          0.01 |


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
 |         JsonNet | 2,810.9666 ns | 38.5453 ns |   2.36 |          0.04 |
 |    MPCli_Stream | 1,193.7118 ns | 16.3394 ns |   1.00 |          0.00 |
 |     MPCli_Array | 1,276.6213 ns | 20.8317 ns |   1.07 |          0.02 |
 |  MPLight_Stream |   953.0368 ns |  9.4350 ns |   0.80 |          0.01 |
 |   MPLight_Array | 1,005.2127 ns | 21.1055 ns |   0.84 |          0.02 |
 |   MPCliH_Stream | 1,224.0906 ns | 10.6578 ns |   1.03 |          0.02 |
 |    MPCliH_Array | 1,261.7811 ns |  9.6635 ns |   1.06 |          0.02 |
 | MPLightH_Stream |   794.2807 ns |  4.6535 ns |   0.67 |          0.01 |
 |  MPLightH_Array |   844.0825 ns |  7.6836 ns |   0.71 |          0.01 |


### Complex object deserialize

``` ini

BenchmarkDotNet=v0.10.3.0, OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-6600 CPU 3.30GHz, ProcessorCount=4
Frequency=3234373 Hz, Resolution=309.1789 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0


```
 |          Method |      Mean |    StdDev | Scaled | Scaled-StdDev |
 |---------------- |---------- |---------- |------- |-------------- |
 |         JsonNet | 3.1211 us | 0.0461 us |   0.82 |          0.01 |
 |    MPCli_Stream | 3.8234 us | 0.0226 us |   1.00 |          0.00 |
 |     MPCli_Array | 4.0229 us | 0.0353 us |   1.05 |          0.01 |
 |  MPLight_Stream | 1.8126 us | 0.0220 us |   0.47 |          0.01 |
 |   MPLight_Array | 1.5828 us | 0.0117 us |   0.41 |          0.00 |
 |   MPCliH_Stream | 3.5989 us | 0.0525 us |   0.94 |          0.01 |
 |    MPCliH_Array | 3.7282 us | 0.0280 us |   0.98 |          0.01 |
 | MPLightH_Stream | 1.8149 us | 0.0149 us |   0.47 |          0.00 |
 |  MPLightH_Array | 1.5887 us | 0.0146 us |   0.42 |          0.00 |


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
 |         JsonNet | 15.9414 us | 0.1109 us |   2.13 |          0.02 |
 |    MPCli_Stream |  7.4911 us | 0.0448 us |   1.00 |          0.00 |
 |     MPCli_Array |  7.6036 us | 0.0531 us |   1.02 |          0.01 |
 |  MPLight_Stream |  6.4354 us | 0.0269 us |   0.86 |          0.01 |
 |   MPLight_Array |  6.5832 us | 0.0424 us |   0.88 |          0.01 |
 |   MPCliH_Stream |  7.5702 us | 0.0593 us |   1.01 |          0.01 |
 |    MPCliH_Array |  7.6189 us | 0.0536 us |   1.02 |          0.01 |
 | MPLightH_Stream |  5.3471 us | 0.0256 us |   0.71 |          0.01 |
 |  MPLightH_Array |  5.4473 us | 0.0144 us |   0.73 |          0.00 |


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
 |         JsonNet | 16.4895 us | 0.2246 us |   0.60 |          0.01 |
 |    MPCli_Stream | 27.4785 us | 0.1346 us |   1.00 |          0.00 |
 |     MPCli_Array | 27.8853 us | 0.1244 us |   1.01 |          0.01 |
 |  MPLight_Stream | 12.8042 us | 0.1266 us |   0.47 |          0.00 |
 |   MPLight_Array | 10.8801 us | 0.1009 us |   0.40 |          0.00 |
 |   MPCliH_Stream | 28.1459 us | 0.2864 us |   1.02 |          0.01 |
 |    MPCliH_Array | 27.8820 us | 0.3003 us |   1.01 |          0.01 |
 | MPLightH_Stream | 13.0098 us | 0.1279 us |   0.47 |          0.01 |
 |  MPLightH_Array | 11.0571 us | 0.0915 us |   0.40 |          0.00 |


### Object skip

``` ini

BenchmarkDotNet=v0.10.3.0, OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-6600 CPU 3.30GHz, ProcessorCount=4
Frequency=3234373 Hz, Resolution=309.1789 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0


```
 |                   Method |          Mean |    StdDev | Scaled | Scaled-StdDev |
 |------------------------- |-------------- |---------- |------- |-------------- |
 |            MPackCli_Skip |   328.5450 ns | 1.2711 ns |   1.00 |          0.00 |
 | MsgPackLight_Skip_Stream | 1,181.1686 ns | 7.4485 ns |   3.60 |          0.03 |
 |  MsgPackLight_Skip_Array |   944.9971 ns | 7.9800 ns |   2.88 |          0.03 |


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
 |            MPackCli_Skip | 2.4251 us | 0.0086 us |   1.00 |          0.00 |
 | MsgPackLight_Skip_Stream | 8.3459 us | 0.0498 us |   3.44 |          0.02 |
 |  MsgPackLight_Skip_Array | 6.4616 us | 0.0492 us |   2.66 |          0.02 |
