using ProGaudi.MsgPack.Light.Benchmark.Data;

namespace ProGaudi.MsgPack.Light.Benchmark
{
    public class IntDeserialize : NumberDeserialize<int>
    {
        protected override int[] Numbers => BenchmarkData.Integers;
    }
}