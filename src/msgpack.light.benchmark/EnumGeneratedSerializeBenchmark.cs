using BenchmarkDotNet.Attributes;

using ProGaudi.MsgPack.Light.Benchmark.Data;

namespace ProGaudi.MsgPack.Light.Benchmark
{
    [Config(typeof(BenchmarkConfig))]
    public class EnumGeneratedSerializeBenchmark
    {
        private readonly BeerType _beerType;

        private readonly int _beerTypeIntRepresentation;

        public EnumGeneratedSerializeBenchmark()
        {
            _beerType = BeerType.Malt;
            _beerTypeIntRepresentation = (int) _beerType;
        }

        [Benchmark(Baseline = true)]
        public void MPLight_IntEnum()
        {
            var bytes = MsgPackSerializer.Serialize(_beerType, Serializers.MsgPackLightIntEnum);
        }

        [Benchmark]
        public void MPLight_AutoEnum()
        {
            var bytes = MsgPackSerializer.Serialize(_beerType, Serializers.MsgPackLight);
        }

        [Benchmark]
        public void MPLight_Int()
        {
            var bytes = MsgPackSerializer.Serialize(_beerTypeIntRepresentation, Serializers.MsgPackLight);
        }
    }
}
