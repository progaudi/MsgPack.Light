using System.Collections.Generic;
using System.IO;
using System.Text;

using BenchmarkDotNet.Attributes;

using ProGaudi.MsgPack.Light.Benchmark.Data;

namespace ProGaudi.MsgPack.Light.Benchmark
{
    [Config(typeof(BenchmarkConfig))]
    public class MapGeneratedSerializeBenchmark
    {
        private readonly Beer _testBeer;

        public MapGeneratedSerializeBenchmark()
        {
            _testBeer = new Beer
            {
                Brand = "Westvleteren Zes",
                Alcohol = 6.2F,
                Brewery = "Sint-Sixtusabdij van Westvleteren",
                Sort = new List<string> {"trappist"}
            };
        }

        [Benchmark]
        public void JsonNet()
        {
            var memoryStream = new MemoryStream();
            using (var writer = new StreamWriter(memoryStream, Encoding.UTF8, 1024, true))
            {
                Serializers.Newtonsoft.Serialize(writer, _testBeer);
                writer.Flush();
            }
        }

        [Benchmark(Baseline = true)]
        public void MPCli_Array()
        {
            var bytes = Serializers.MsgPack.GetSerializer<Beer>().PackSingleObject(_testBeer);
        }

        [Benchmark]
        public void MPLight_Array()
        {
            var bytes = MsgPackSerializer.Serialize(_testBeer, Serializers.MsgPackLight);
        }

        [Benchmark]
        public void MPLight_Array_AutoMap()
        {
            var bytes = MsgPackSerializer.Serialize(_testBeer, Serializers.MsgPackLightAutoGeneration);
        }
    }
}
