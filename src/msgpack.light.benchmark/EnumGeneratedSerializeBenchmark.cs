using System.Collections.Generic;
using System.IO;
using System.Text;

using BenchmarkDotNet.Attributes;

using ProGaudi.MsgPack.Light.Benchmark.Data;

namespace ProGaudi.MsgPack.Light.Benchmark
{
    [Config(typeof(BenchmarkConfig))]
    public class EnumGeneratedSerializeBenchmark
    {
        private readonly BeerType _beerType;

        public EnumGeneratedSerializeBenchmark()
        {
            _beerType = BeerType.Malt;
        }

        [Benchmark]
        public void JsonNet()
        {
            var memoryStream = new MemoryStream();
            using (var writer = new StreamWriter(memoryStream, Encoding.UTF8, 1024, true))
            {
                Serializers.Newtonsoft.Serialize(writer, _beerType);
                writer.Flush();
            }
        }

        [Benchmark(Baseline = true)]
        public void MPCli_Array()
        {
            var bytes = Serializers.MsgPack.GetSerializer<BeerType>().PackSingleObject(_beerType);
        }

        [Benchmark]
        public void MPLight_IntEnum()
        {
            var bytes = MsgPackSerializer.Serialize(_beerType, Serializers.MsgPackLightIntEnum);
        }

        [Benchmark]
        public void MPLight_AutoEnum()
        {
            var bytes = MsgPackSerializer.Serialize(_beerType, Serializers.MsgPackLight);
        }
    }
}
