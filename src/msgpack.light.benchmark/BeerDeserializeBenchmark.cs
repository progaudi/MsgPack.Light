using System.IO;
using System.Text;

using BenchmarkDotNet.Attributes;

using MessagePack;

using ProGaudi.MsgPack.Light.Benchmark.Data;

namespace ProGaudi.MsgPack.Light.Benchmark
{
    [Config(typeof(BenchmarkConfig))]
    public class BeerDeserializeBenchmark
    {
        private readonly MemoryStream _json;

        private readonly MemoryStream _msgPack;

        private readonly byte[] _msgPackArray;

        public BeerDeserializeBenchmark()
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
        public object JsonNet()
        {
            _json.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(_json, Encoding.UTF8, false, 1024, true))
            {
                return Serializers.Newtonsoft.Deserialize(reader, typeof(Beer));
            }
        }

        [Benchmark(Baseline = true)]
        public Beer MPCli_Stream()
        {
            _msgPack.Seek(0, SeekOrigin.Begin);
            return Serializers.MsgPack.GetSerializer<Beer>().Unpack(_msgPack);
        }

        [Benchmark]
        public Beer MPCli_Array()
        {
            return Serializers.MsgPack.GetSerializer<Beer>().UnpackSingleObject(_msgPackArray);
        }

        [Benchmark]
        public Beer MPSharp_Stream()
        {
            _msgPack.Seek(0, SeekOrigin.Begin);
            return MessagePackSerializer.Deserialize<Beer>(_msgPack);
        }

        [Benchmark]
        public Beer MPSharp_Array()
        {
            return MessagePackSerializer.Deserialize<Beer>(_msgPackArray);
        }

        [Benchmark]
        public Beer MPLight_Array()
        {
            return MsgPackSerializer.Deserialize<Beer>(_msgPackArray, Serializers.MsgPackLight);
        }

        [Benchmark]
        public Beer MPCliH_Stream()
        {
            _msgPack.Seek(0, SeekOrigin.Begin);
            return Serializers.MsgPackHardcore.GetSerializer<Beer>().Unpack(_msgPack);
        }

        [Benchmark]
        public Beer MPCliH_Array()
        {
            return Serializers.MsgPackHardcore.GetSerializer<Beer>().UnpackSingleObject(_msgPackArray);
        }

        [Benchmark]
        public Beer MPLightH_Array()
        {
            return MsgPackSerializer.Deserialize<Beer>(_msgPackArray, Serializers.MsgPackLightHardcore, out _);
        }

//        [Benchmark]
//        public void MPLightH_Array_AutoMap()
//        {
//            var beer = MsgPackSerializer.Deserialize<Beer>(_msgPackArray, Serializers.MsgPackLightMapAutoGeneration);
//        }
    }
}
