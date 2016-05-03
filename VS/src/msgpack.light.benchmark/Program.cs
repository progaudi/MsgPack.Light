using BenchmarkDotNet.Running;

namespace msgpack.light.benchmark
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var switcher = new BenchmarkSwitcher(
                new[]
                {
                    typeof(BeerSerializeBenchmark),
                    typeof(BeerDeserializeBenchmark),
                    typeof(BeerListSerializeBenchmark),
                    typeof(BeerListDeserializeBenchmark),
                    typeof(IntSerialize),
                    typeof(IntDeserialize)
                });
            switcher.Run(args);
        }
    }
}
