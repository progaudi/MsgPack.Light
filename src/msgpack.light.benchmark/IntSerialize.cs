using ProGaudi.MsgPack.Light.Benchmark.Data;

namespace ProGaudi.MsgPack.Light.Benchmark
{
    public class IntSerialize : NumberSerialize<int>
    {
        protected override int[] Numbers => BenchmarkData.Integers;
    }
}