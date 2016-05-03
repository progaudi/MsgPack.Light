using System.Collections.Generic;
using System.IO;
using System.Text;

using BenchmarkDotNet.Attributes;

using MsgPack.Light;

namespace msgpack.light.benchmark
{
    [Config(typeof(BenchmarkConfig))]
    public class BeerDeserializeBenchmark
    {
        private readonly MemoryStream _json;

        private readonly MemoryStream _msgPackCli;

        private readonly byte[] _msgPackCliArray;

        private readonly MemoryStream _msgPackLight;

        private readonly byte[] _msgPackLightArray;

        public BeerDeserializeBenchmark()
        {
            var serializer = new BeerSerializeBenchmark();
            _json = PrepareJson(serializer);
            _msgPackCli = PrepareMsgPack(serializer);
            _msgPackCliArray = _msgPackCli.ToArray();

            // as I found, we have a different order of fields, so we'll have not compatible but equal on size representations
            _msgPackLight = PrepareMsgPackLight(serializer);
            _msgPackLightArray = _msgPackLight.ToArray();
        }

        private MemoryStream PrepareMsgPack(BeerSerializeBenchmark serializer)
        {
            var memoryStream = new MemoryStream();
            serializer.MsgPackSerialize(memoryStream);
            return memoryStream;
        }

        private MemoryStream PrepareMsgPackLight(BeerSerializeBenchmark serializer)
        {
            var memoryStream = new MemoryStream();
            serializer.MsgPackLightSerialize(memoryStream);
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
                var beer = Serializers<Beer>.Newtonsoft.Deserialize(reader, typeof(Beer));
            }
        }

        [Benchmark]
        public void JsonStack()
        {
            _json.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(_json, Encoding.UTF8, false, 1024, true))
            {
                var beer = Serializers<Beer>.ServiceStack.DeserializeFromReader(reader);
            }
        }

        [Benchmark(Baseline = true)]
        public void MPCli_Stream()
        {
            _msgPackCli.Seek(0, SeekOrigin.Begin);
            var beer = Serializers<Beer>.MsgPack.Unpack(_msgPackCli);
        }

        [Benchmark]
        public void MPCli_Array()
        {
            var beer = Serializers<Beer>.MsgPack.UnpackSingleObject(_msgPackCliArray);
        }

        [Benchmark]
        public void MPLight_Stream()
        {
            _msgPackLight.Seek(0, SeekOrigin.Begin);
            var beer = MsgPackSerializer.Deserialize<Beer>(_msgPackLight, Serializers<Beer>.MsgPackLight);
        }

        [Benchmark]
        public void MPLight_Array()
        {
            var beer = MsgPackSerializer.Deserialize<Beer>(_msgPackLightArray, Serializers<Beer>.MsgPackLight);
        }
    }
}