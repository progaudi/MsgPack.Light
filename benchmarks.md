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
 |         Method |        Mean |     StdDev | Scaled | Scaled-StdDev |  Gen 0 | Allocated |
 |--------------- |------------ |----------- |------- |-------------- |------- |---------- |
 |    MPCli_Array | 324.3986 ns |  2.4595 ns |   1.15 |          0.01 | 0.1354 |     464 B |
 |   MPCli_Stream | 282.9804 ns |  1.9713 ns |   1.00 |          0.00 | 0.1199 |     416 B |
 |  MPLight_Array | 713.9852 ns | 13.2507 ns |   2.52 |          0.05 | 0.3858 |   1.29 kB |
 | MPLight_Stream | 690.0378 ns |  6.7355 ns |   2.44 |          0.03 | 0.3724 |   1.25 kB |


### Int array deserialize

``` ini

BenchmarkDotNet=v0.10.3.0, OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-6600 CPU 3.30GHz, ProcessorCount=4
Frequency=3234373 Hz, Resolution=309.1789 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0


```
 |         Method |        Mean |    StdDev | Scaled | Scaled-StdDev |  Gen 0 | Allocated |
 |--------------- |------------ |---------- |------- |-------------- |------- |---------- |
 |    MPCli_Array | 380.0690 ns | 4.1156 ns |   1.17 |          0.02 | 0.0781 |     288 B |
 |   MPCli_Stream | 324.8467 ns | 3.9237 ns |   1.00 |          0.00 | 0.0546 |     208 B |
 |  MPLight_Array | 278.7431 ns | 4.9682 ns |   0.86 |          0.02 | 0.0136 |      80 B |
 | MPLight_Stream | 335.6105 ns | 1.8717 ns |   1.03 |          0.01 | 0.0131 |      80 B |


### Double array serialize

``` ini

BenchmarkDotNet=v0.10.3.0, OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-6600 CPU 3.30GHz, ProcessorCount=4
Frequency=3234373 Hz, Resolution=309.1789 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0


```
 |         Method |        Mean |    StdDev | Scaled | Scaled-StdDev |  Gen 0 | Allocated |
 |--------------- |------------ |---------- |------- |-------------- |------- |---------- |
 |    MPCli_Array | 608.6606 ns | 6.1428 ns |   1.08 |          0.01 | 0.2169 |     760 B |
 |   MPCli_Stream | 565.3792 ns | 3.7022 ns |   1.00 |          0.00 | 0.1940 |     688 B |
 |  MPLight_Array | 584.0085 ns | 8.6239 ns |   1.03 |          0.02 | 0.2928 |     992 B |
 | MPLight_Stream | 557.7645 ns | 5.7787 ns |   0.99 |          0.01 | 0.2689 |     920 B |


### Double array deserialize

``` ini

BenchmarkDotNet=v0.10.3.0, OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-6600 CPU 3.30GHz, ProcessorCount=4
Frequency=3234373 Hz, Resolution=309.1789 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0


```
 |         Method |        Mean |    StdDev | Scaled | Scaled-StdDev |  Gen 0 | Allocated |
 |--------------- |------------ |---------- |------- |-------------- |------- |---------- |
 |    MPCli_Array | 430.3499 ns | 6.8997 ns |   1.13 |          0.03 | 0.0720 |     304 B |
 |   MPCli_Stream | 379.5234 ns | 6.6924 ns |   1.00 |          0.00 | 0.0477 |     224 B |
 |  MPLight_Array | 328.7889 ns | 6.2055 ns |   0.87 |          0.02 | 0.0179 |      96 B |
 | MPLight_Stream | 440.2761 ns | 2.4187 ns |   1.16 |          0.02 | 0.0572 |     256 B |


### Complex object serialize

``` ini

BenchmarkDotNet=v0.10.3.0, OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-6600 CPU 3.30GHz, ProcessorCount=4
Frequency=3234373 Hz, Resolution=309.1789 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0


```
 |          Method |          Mean |     StdDev | Scaled | Scaled-StdDev |  Gen 0 | Allocated |
 |---------------- |-------------- |----------- |------- |-------------- |------- |---------- |
 |         JsonNet | 2,943.3977 ns | 28.0940 ns |   2.46 |          0.04 | 1.9587 |   6.48 kB |
 |    MPCli_Stream | 1,199.1435 ns | 18.2798 ns |   1.00 |          0.00 | 0.2108 |     816 B |
 |     MPCli_Array | 1,290.6559 ns | 24.0992 ns |   1.08 |          0.03 | 0.2500 |     936 B |
 |  MPLight_Stream |   791.3383 ns | 10.6041 ns |   0.66 |          0.01 | 0.1897 |     752 B |
 |   MPLight_Array |   837.6572 ns |  5.2336 ns |   0.70 |          0.01 | 0.2299 |     872 B |
 |   MPCliH_Stream | 1,209.6436 ns | 13.8834 ns |   1.01 |          0.02 | 0.2129 |     816 B |
 |    MPCliH_Array | 1,267.6657 ns | 13.2526 ns |   1.06 |          0.02 | 0.2490 |     936 B |
 | MPLightH_Stream |   601.4984 ns |  6.5826 ns |   0.50 |          0.01 | 0.1731 |     624 B |
 |  MPLightH_Array |   636.7675 ns |  6.2320 ns |   0.53 |          0.01 | 0.2107 |     744 B |


### Complex object deserialize

``` ini

BenchmarkDotNet=v0.10.3.0, OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-6600 CPU 3.30GHz, ProcessorCount=4
Frequency=3234373 Hz, Resolution=309.1789 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0


```
 |          Method |          Mean |     StdDev | Scaled | Scaled-StdDev |  Gen 0 | Allocated |
 |---------------- |-------------- |----------- |------- |-------------- |------- |---------- |
 |         JsonNet | 3,186.7006 ns | 27.9212 ns |   0.88 |          0.01 | 1.7812 |   6.24 kB |
 |    MPCli_Stream | 3,620.5613 ns | 39.9484 ns |   1.00 |          0.00 | 0.3174 |   1.55 kB |
 |     MPCli_Array | 3,705.8617 ns | 30.7222 ns |   1.02 |          0.01 | 0.3408 |   1.63 kB |
 |  MPLight_Stream |   948.1123 ns | 12.5935 ns |   0.26 |          0.00 | 0.2424 |     904 B |
 |   MPLight_Array |   735.1687 ns |  8.0861 ns |   0.20 |          0.00 | 0.1703 |     608 B |
 |   MPCliH_Stream | 3,646.0951 ns | 32.8870 ns |   1.01 |          0.01 | 0.3133 |   1.55 kB |
 |    MPCliH_Array | 3,704.1674 ns | 37.3530 ns |   1.02 |          0.01 | 0.3367 |   1.63 kB |
 | MPLightH_Stream |   935.3351 ns | 11.5240 ns |   0.26 |          0.00 | 0.2424 |     904 B |
 |  MPLightH_Array |   727.2057 ns |  6.5636 ns |   0.20 |          0.00 | 0.1703 |     608 B |


### List of complex objects serialize

``` ini

BenchmarkDotNet=v0.10.3.0, OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-6600 CPU 3.30GHz, ProcessorCount=4
Frequency=3234373 Hz, Resolution=309.1789 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0


```
 |          Method |       Mean |    StdDev | Scaled | Scaled-StdDev |  Gen 0 | Allocated |
 |---------------- |----------- |---------- |------- |-------------- |------- |---------- |
 |         JsonNet | 16.6842 us | 0.1935 us |   2.23 |          0.03 | 1.8840 |   8.04 kB |
 |    MPCli_Stream |  7.4890 us | 0.0663 us |   1.00 |          0.00 | 1.1373 |   4.75 kB |
 |     MPCli_Array |  7.6265 us | 0.0760 us |   1.02 |          0.01 | 1.3590 |   5.43 kB |
 |  MPLight_Stream |  5.4254 us | 0.0768 us |   0.72 |          0.01 | 1.2522 |   4.48 kB |
 |   MPLight_Array |  5.5761 us | 0.1151 us |   0.74 |          0.02 | 1.4496 |   5.16 kB |
 |   MPCliH_Stream |  7.5517 us | 0.1012 us |   1.01 |          0.02 | 1.1617 |   4.75 kB |
 |    MPCliH_Array |  8.6123 us | 0.0715 us |   1.15 |          0.01 | 1.3753 |   5.43 kB |
 | MPLightH_Stream |  3.8995 us | 0.0376 us |   0.52 |          0.01 | 0.9583 |   3.58 kB |
 |  MPLightH_Array |  4.0550 us | 0.0389 us |   0.54 |          0.01 | 1.1597 |   4.26 kB |


### List of complex objects deserialize

``` ini

BenchmarkDotNet=v0.10.3.0, OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-6600 CPU 3.30GHz, ProcessorCount=4
Frequency=3234373 Hz, Resolution=309.1789 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0


```
 |          Method |       Mean |    StdDev | Scaled | Scaled-StdDev |  Gen 0 | Allocated |
 |---------------- |----------- |---------- |------- |-------------- |------- |---------- |
 |         JsonNet | 16.8679 us | 0.1679 us |   0.59 |          0.01 | 2.2339 |   9.07 kB |
 |    MPCli_Stream | 28.3922 us | 0.5130 us |   1.00 |          0.00 | 2.5472 |  12.14 kB |
 |     MPCli_Array | 29.2684 us | 0.3780 us |   1.03 |          0.02 | 2.5798 |  12.22 kB |
 |  MPLight_Stream |  6.9067 us | 0.0467 us |   0.24 |          0.00 | 1.6256 |   6.28 kB |
 |   MPLight_Array |  5.2911 us | 0.0329 us |   0.19 |          0.00 | 1.1220 |   4.11 kB |
 |   MPCliH_Stream | 28.5138 us | 0.2435 us |   1.00 |          0.02 | 2.4821 |  12.14 kB |
 |    MPCliH_Array | 29.0823 us | 0.7092 us |   1.02 |          0.03 | 2.3844 |  12.22 kB |
 | MPLightH_Stream |  7.0071 us | 0.0342 us |   0.25 |          0.00 | 1.6174 |   6.28 kB |
 |  MPLightH_Array |  5.3208 us | 0.0595 us |   0.19 |          0.00 | 1.1302 |   4.11 kB |


### Object skip

``` ini

BenchmarkDotNet=v0.10.3.0, OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-6600 CPU 3.30GHz, ProcessorCount=4
Frequency=3234373 Hz, Resolution=309.1789 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0


```
 |                   Method |        Mean |    StdDev | Scaled | Scaled-StdDev |  Gen 0 | Allocated |
 |------------------------- |------------ |---------- |------- |-------------- |------- |---------- |
 |            MPackCli_Skip | 346.8357 ns | 3.3332 ns |   1.00 |          0.00 | 0.1120 |     392 B |
 | MsgPackLight_Skip_Stream | 385.6519 ns | 3.9323 ns |   1.11 |          0.02 |      - |      32 B |
 |  MsgPackLight_Skip_Array | 351.4116 ns | 2.7263 ns |   1.01 |          0.01 |      - |      32 B |


### Objects list skip

``` ini

BenchmarkDotNet=v0.10.3.0, OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i5-6600 CPU 3.30GHz, ProcessorCount=4
Frequency=3234373 Hz, Resolution=309.1789 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1586.0


```
 |                   Method |      Mean |    StdDev | Scaled | Scaled-StdDev |  Gen 0 | Allocated |
 |------------------------- |---------- |---------- |------- |-------------- |------- |---------- |
 |            MPackCli_Skip | 2.5528 us | 0.0317 us |   1.00 |          0.00 | 0.6317 |   2.26 kB |
 | MsgPackLight_Skip_Stream | 2.5117 us | 0.0156 us |   0.98 |          0.01 |      - |      32 B |
 |  MsgPackLight_Skip_Array | 2.2431 us | 0.0141 us |   0.88 |          0.01 |      - |      32 B |
