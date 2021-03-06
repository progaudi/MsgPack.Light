using System.IO;
using System.Text;

using BenchmarkDotNet.Attributes;

using ProGaudi.MsgPack.Light.Benchmark.Data;

namespace ProGaudi.MsgPack.Light.Benchmark
{
    [Config(typeof(BenchmarkConfig))]
    public class MapGeneratedDeserializeBenchmark
    {
        private readonly MemoryStream _json;

        private readonly MemoryStream _msgPack;

        private readonly byte[] _msgPackArray;

        public MapGeneratedDeserializeBenchmark()
        {
            var serializer = new BeerSerializeBenchmark();
            _json = PrepareJson(serializer);
            _msgPack = PrepareMsgPack(serializer);
            _msgPackArray = _msgPack.ToArray();
        }

        private MemoryStream PrepareMsgPack(BeerSerializeBenchmark serializer)
        {
            var memoryStream = new MemoryStream();
            serializer.MsgPackSerialize(memoryStream);
            return memoryStream;
        }

        private MemoryStream PrepareJson(BeerSerializeBenchmark serializer)
        {
            var memoryStream = new MemoryStream();
            serializer.JsonSerialize(memoryStream);
            return memoryStream;
        }

        [Benchmark]
        public void JsonNet()
        {
            _json.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(_json, Encoding.UTF8, false, 1024, true))
            {
                var beer = Serializers.Newtonsoft.Deserialize(reader, typeof(Beer));
            }
        }

        [Benchmark(Baseline = true)]
        public void MPCli_Array()
        {
            var beer = Serializers.MsgPack.GetSerializer<Beer>().UnpackSingleObject(_msgPackArray);
        }

        [Benchmark]
        public void MPLight_Array()
        {
            var beer = MsgPackSerializer.Deserialize<Beer>(_msgPackArray, Serializers.MsgPackLight);
        }

        [Benchmark]
        public void MPLight_Array_AutoMap()
        {
            var beer = MsgPackSerializer.Deserialize<Beer>(_msgPackArray, Serializers.MsgPackLightMapAutoGeneration);
        }

        [Benchmark]
        public void MPLight_Array_AutoArray()
        {
            var beer = MsgPackSerializer.Deserialize<Beer>(_msgPackArray, Serializers.MsgPackLightArrayAutoGeneration);
        }
    }
}