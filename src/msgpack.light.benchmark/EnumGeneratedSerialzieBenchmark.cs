using System.IO;
using System.Text;

using BenchmarkDotNet.Attributes;

using ProGaudi.MsgPack.Light.Benchmark.Data;

namespace ProGaudi.MsgPack.Light.Benchmark
{
    [Config(typeof(BenchmarkConfig))]
    public class EnumGeneratedDeserializeBenchmark
    {
        private readonly MemoryStream _json;

        private readonly MemoryStream _msgPack;

        private readonly byte[] _msgPackArray;

        public EnumGeneratedDeserializeBenchmark()
        {
            var beerType = BeerType.Lager;
            _json = PrepareJson(beerType);
            _msgPack = PrepareMsgPack(beerType);
            _msgPackArray = _msgPack.ToArray();
        }

        private MemoryStream PrepareMsgPack(BeerType beerType)
        {
            var memoryStream = new MemoryStream();
            MsgPackSerializer.Serialize(beerType, memoryStream);
            return memoryStream;
        }

        private MemoryStream PrepareJson(BeerType beerType)
        {
            var memoryStream = new MemoryStream();

            using (var writer = new StreamWriter(memoryStream, Encoding.UTF8, 1024, true))
            {
                Serializers.Newtonsoft.Serialize(writer, beerType);
                writer.Flush();
            }

            return memoryStream;
        }

        [Benchmark]
        public void JsonNet()
        {
            _json.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(_json, Encoding.UTF8, false, 1024, true))
            {
                var beerType = Serializers.Newtonsoft.Deserialize(reader, typeof(BeerType));
            }
        }

        [Benchmark(Baseline = true)]
        public void MPCli()
        {
            var beerType = Serializers.MsgPack.GetSerializer<BeerType>().UnpackSingleObject(_msgPackArray);
        }

        [Benchmark]
        public void MPLight_AutoEnum()
        {
            var beerType = MsgPackSerializer.Deserialize<BeerType>(_msgPackArray, Serializers.MsgPackLight);
        }

        [Benchmark]
        public void MPLight_IntEnum()
        {
            var beerType = MsgPackSerializer.Deserialize<BeerType>(_msgPackArray, Serializers.MsgPackLightMapAutoGeneration);
        }
    }
}