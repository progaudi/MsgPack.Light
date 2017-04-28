using ProGaudi.MsgPack.Light.Benchmark.Data;

namespace ProGaudi.MsgPack.Light.Benchmark
{
    public class DoubleDeserialize : NumberDeserialize<double>
    {
        protected override double[] Numbers => BenchmarkData.Doubles;
    }
}