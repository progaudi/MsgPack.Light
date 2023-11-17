using System.Collections.Generic;
using System.IO;
using System.Text;

using BenchmarkDotNet.Attributes;

using MessagePack;

using ProGaudi.MsgPack.Light.Benchmark.Data;

namespace ProGaudi.MsgPack.Light.Benchmark
{
    [Config(typeof(BenchmarkConfig))]
    public class BeerSerializeBenchmark
    {
        private readonly Beer _testBeer;

        public BeerSerializeBenchmark()
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
            JsonSerialize(memoryStream);
        }

        internal void JsonSerialize(MemoryStream memoryStream)
        {
            using (var writer = new StreamWriter(memoryStream, Encoding.UTF8, 1024, true))
            {
                Serializers.Newtonsoft.Serialize(writer, _testBeer);
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
            Serializers.MsgPack.GetSerializer<Beer>().Pack(memoryStream, _testBeer);
        }

        [Benchmark]
        public void MPSharp_Stream()
        {
            var stream = new MemoryStream();
            MessagePackSerializer.Serialize(stream, _testBeer);
        }

        [Benchmark]
        public void MPSharp_Array()
        {
            MessagePackSerializer.Serialize(_testBeer);
        }

        [Benchmark]
        public void MPLight_Stream()
        {
            var memoryStream = new MemoryStream();
            MsgPackSerializer.Serialize(_testBeer, memoryStream, Serializers.MsgPackLight);
        }

        [Benchmark]
        public void MPLight_Array()
        {
            var bytes = MsgPackSerializer.Serialize(_testBeer, Serializers.MsgPackLight);
        }

        [Benchmark]
        public void MPCliH_Stream()
        {
            var memoryStream = new MemoryStream();
            Serializers.MsgPackHardcore.GetSerializer<Beer>().Pack(memoryStream, _testBeer);
        }

        [Benchmark]
        public void MPCliH_Array()
        {
            var bytes = Serializers.MsgPackHardcore.GetSerializer<Beer>().PackSingleObject(_testBeer);
        }

        [Benchmark]
        public void MPLightH_Stream()
        {
            var memoryStream = new MemoryStream();
            MsgPackSerializer.Serialize(_testBeer, memoryStream, Serializers.MsgPackLightHardcore);
        }

        [Benchmark]
        public void MPLightH_Array()
        {
            var bytes = MsgPackSerializer.Serialize(_testBeer, Serializers.MsgPackLightHardcore);
        }

        [Benchmark]
        public void MPLightH_Stream_AutoMap()
        {
            var memoryStream = new MemoryStream();
            MsgPackSerializer.Serialize(_testBeer, memoryStream, Serializers.MsgPackLightMapAutoGeneration);
        }

        [Benchmark]
        public void MPLightH_Array_AutoMap()
        {
            var bytes = MsgPackSerializer.Serialize(_testBeer, Serializers.MsgPackLightMapAutoGeneration);
        }

        [Benchmark]
        public void MPLightH_Stream_AutoArray()
        {
            var memoryStream = new MemoryStream();
            MsgPackSerializer.Serialize(_testBeer, memoryStream, Serializers.MsgPackLightArrayAutoGeneration);
        }

        [Benchmark]
        public void MPLightH_Array_AutoArray()
        {
            var bytes = MsgPackSerializer.Serialize(_testBeer, Serializers.MsgPackLightArrayAutoGeneration);
        }
    }
}
