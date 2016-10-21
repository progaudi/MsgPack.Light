using System.IO;
using System.Text;

using BenchmarkDotNet.Attributes;

using ProGaudi.MsgPack.Light;

namespace ProGaudi.MsgPack.Light.benchmark
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
                Serializers<Beer[]>.Newtonsoft.Serialize(writer, Data.Belgium);
                writer.Flush();
            }
        }

        [Benchmark]
        public void JsonStack()
        {
            var memoryStream = new MemoryStream();
            using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
            {
                Serializers<Beer[]>.ServiceStack.SerializeToWriter(Data.Belgium, writer);
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
            Serializers<Beer[]>.MsgPack.GetSerializer<Beer[]>().Pack(memoryStream, Data.Belgium);
        }

        [Benchmark]
        public void MPCli_Array()
        {
            var bytes = Serializers<Beer[]>.MsgPack.GetSerializer<Beer[]>().PackSingleObject(Data.Belgium);
        }

        [Benchmark]
        public void MPLight_Stream()
        {
            var memoryStream = new MemoryStream();
            MsgPackLightSerialize(memoryStream);
        }

        internal void MsgPackLightSerialize(MemoryStream memoryStream)
        {
            MsgPackSerializer.Serialize(Data.Belgium, memoryStream, Serializers<Beer[]>.MsgPackLight);
        }

        [Benchmark]
        public void MPLight_Array()
        {
            var bytes = MsgPackSerializer.Serialize(Data.Belgium, Serializers<Beer[]>.MsgPackLight);
        }

        [Benchmark]
        public void MPCliH_Stream()
        {
            var memoryStream = new MemoryStream();
            Serializers<Beer[]>.MsgPackHardcore.GetSerializer<Beer[]>().Pack(memoryStream, Data.Belgium);
        }

        [Benchmark]
        public void MPCliH_Array()
        {
            var bytes = Serializers<Beer[]>.MsgPackHardcore.GetSerializer<Beer[]>().PackSingleObject(Data.Belgium);
        }

        [Benchmark]
        public void MPLightH_Stream()
        {
            var memoryStream = new MemoryStream();
            MsgPackSerializer.Serialize(Data.Belgium, memoryStream, Serializers<Beer[]>.MsgPackLightHardcore);
        }

        [Benchmark]
        public void MPLightH_Array()
        {
            var bytes = MsgPackSerializer.Serialize(Data.Belgium, Serializers<Beer[]>.MsgPackLightHardcore);
        }
    }
}
