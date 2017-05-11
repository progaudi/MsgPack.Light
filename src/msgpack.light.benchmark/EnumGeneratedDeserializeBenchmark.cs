using System.IO;

using BenchmarkDotNet.Attributes;

using ProGaudi.MsgPack.Light.Benchmark.Data;

namespace ProGaudi.MsgPack.Light.Benchmark
{
    [Config(typeof(BenchmarkConfig))]
    public class EnumGeneratedDeserializeBenchmark
    {
        private readonly byte[] _msgPackForInt;

        private readonly byte[] _msgPackArray;

        public EnumGeneratedDeserializeBenchmark()
        {
            var beerType = BeerType.Lager;
            _msgPackArray = PrepareMsgPack(beerType).ToArray();
            _msgPackForInt = PrepareMsgPack((int) beerType).ToArray();
        }

        private MemoryStream PrepareMsgPack<T>(T item)
        {
            var memoryStream = new MemoryStream();
            MsgPackSerializer.Serialize(item, memoryStream);
            return memoryStream;
        }

        [Benchmark]
        public void MPLight_AutoEnum()
        {
            var beerType = MsgPackSerializer.Deserialize<BeerType>(_msgPackArray, Serializers.MsgPackLight);
        }

        [Benchmark(Baseline = true)]
        public void MPLight_IntEnum()
        {
            var beerType = MsgPackSerializer.Deserialize<BeerType>(_msgPackArray, Serializers.MsgPackLightMapAutoGeneration);
        }

        [Benchmark]
        public void MPLight_Int()
        {
            var beerType = MsgPackSerializer.Deserialize<int>(_msgPackForInt, Serializers.MsgPackLight);
        }
    }
}