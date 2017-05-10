using System.IO;
using System.Text;

using BenchmarkDotNet.Attributes;

using ProGaudi.MsgPack.Light.Benchmark.Data;

namespace ProGaudi.MsgPack.Light.Benchmark
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
                Serializers.Newtonsoft.Serialize(writer, BenchmarkData.Belgium);
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
            Serializers.MsgPack.GetSerializer<Beer[]>().Pack(memoryStream, BenchmarkData.Belgium);
        }

        [Benchmark]
        public void MPCli_Array()
        {
            var bytes = Serializers.MsgPack.GetSerializer<Beer[]>().PackSingleObject(BenchmarkData.Belgium);
        }

        [Benchmark]
        public void MPLight_Stream()
        {
            var memoryStream = new MemoryStream();
            MsgPackLightSerialize(memoryStream);
        }

        internal void MsgPackLightSerialize(MemoryStream memoryStream)
        {
            MsgPackSerializer.Serialize(BenchmarkData.Belgium, memoryStream, Serializers.MsgPackLight);
        }

        [Benchmark]
        public void MPLight_Array()
        {
            var bytes = MsgPackSerializer.Serialize(BenchmarkData.Belgium, Serializers.MsgPackLight);
        }

        [Benchmark]
        public void MPCliH_Stream()
        {
            var memoryStream = new MemoryStream();
            Serializers.MsgPackHardcore.GetSerializer<Beer[]>().Pack(memoryStream, BenchmarkData.Belgium);
        }

        [Benchmark]
        public void MPCliH_Array()
        {
            var bytes = Serializers.MsgPackHardcore.GetSerializer<Beer[]>().PackSingleObject(BenchmarkData.Belgium);
        }

        [Benchmark]
        public void MPLightH_Stream()
        {
            var memoryStream = new MemoryStream();
            MsgPackSerializer.Serialize(BenchmarkData.Belgium, memoryStream, Serializers.MsgPackLightHardcore);
        }

        [Benchmark]
        public void MPLightH_Array()
        {
            var bytes = MsgPackSerializer.Serialize(BenchmarkData.Belgium, Serializers.MsgPackLightHardcore);
        }

        [Benchmark]
        public void MPLightH_Stream_AutoMap()
        {
            var memoryStream = new MemoryStream();
            MsgPackSerializer.Serialize(BenchmarkData.Belgium, memoryStream, Serializers.MsgPackLightMapAutoGeneration);
        }

        [Benchmark]
        public void MPLightH_Array_AutoMap()
        {
            var bytes = MsgPackSerializer.Serialize(BenchmarkData.Belgium, Serializers.MsgPackLightMapAutoGeneration);
        }

        [Benchmark]
        public void MPLightH_Stream_AutoArray()
        {
            var memoryStream = new MemoryStream();
            MsgPackSerializer.Serialize(BenchmarkData.Belgium, memoryStream, Serializers.MsgPackLightArrayAutoGeneration);
        }

        [Benchmark]
        public void MPLightH_Array_AutoArray()
        {
            var bytes = MsgPackSerializer.Serialize(BenchmarkData.Belgium, Serializers.MsgPackLightArrayAutoGeneration);
        }
    }
}
