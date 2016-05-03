using System.IO;
using System.Text;

using BenchmarkDotNet.Attributes;

using MsgPack.Light;

namespace msgpack.light.benchmark
{
    [Config(typeof(BenchmarkConfig))]
    public class BeerListSerializeBenchmark
    {
        [Benchmark]
        public void JsonNet()
        {
            var memoryStream = new MemoryStream();
            JsonSerialize(memoryStream);
        }

        internal void JsonSerialize(MemoryStream memoryStream)
        {
            using (var writer = new StreamWriter(memoryStream, Encoding.UTF8, 1024, true))
            {
                Serializers<Beer[]>.Newtonsoft.Serialize(writer, Beer.Belgium);
                writer.Flush();
            }
        }

        [Benchmark]
        public void JsonStack()
        {
            var memoryStream = new MemoryStream();
            using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
            {
                Serializers<Beer[]>.ServiceStack.SerializeToWriter(Beer.Belgium, writer);
                writer.Flush();
            }
        }

        [Benchmark(Baseline = true)]
        public void MPCli_Stream()
        {
            var memoryStream = new MemoryStream();
            MsgPackSerialize(memoryStream);
        }

        internal void MsgPackSerialize(MemoryStream memoryStream)
        {
            Serializers<Beer[]>.MsgPack.Pack(memoryStream, Beer.Belgium);
        }

        [Benchmark]
        public void MPCli_Array()
        {
            var memoryStream = new MemoryStream(Serializers<Beer[]>.MsgPack.PackSingleObject(Beer.Belgium));
        }

        [Benchmark]
        public void MPLight_Stream()
        {
            var memoryStream = new MemoryStream();
            MsgPackLightSerialize(memoryStream);
        }

        internal void MsgPackLightSerialize(MemoryStream memoryStream)
        {
            MsgPackSerializer.Serialize(Beer.Belgium, memoryStream, Serializers<Beer[]>.MsgPackLight);
        }

        [Benchmark]
        public void MPLight_Array()
        {
            var memoryStream = new MemoryStream(MsgPackSerializer.Serialize(Beer.Belgium, Serializers<Beer[]>.MsgPackLight));
        }
    }
}
