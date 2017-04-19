using ProGaudi.MsgPack.Light.Benchmark.Data;

namespace ProGaudi.MsgPack.Light.Benchmark
{
    public class DoubleSerialize: NumberSerialize<double>
    {
        protected override double[] Numbers => BenchmarkData.Doubles;
    }
}