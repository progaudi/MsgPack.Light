using BenchmarkDotNet.Running;

namespace ProGaudi.MsgPack.Light.Benchmark
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var switcher = new BenchmarkSwitcher(
                new[]
                {
                    typeof (BeerSerializeBenchmark),
                    typeof (BeerDeserializeBenchmark),
                    typeof (BeerListSerializeBenchmark),
                    typeof (BeerListDeserializeBenchmark),
                    typeof (IntDeserialize),
                    typeof (DoubleDeserialize),
                    typeof (IntSerialize),
                    typeof (DoubleSerialize),
                    typeof (BeerSkip),
                    typeof (BeerSkipList)
                });
            switcher.Run(args);
        }
    }
}
