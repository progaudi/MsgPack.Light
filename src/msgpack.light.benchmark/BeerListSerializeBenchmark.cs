using System.Buffers;
using System.IO;
using System.Text;

using BenchmarkDotNet.Attributes;

using ProGaudi.MsgPack.Light.Benchmark.Data;

namespace ProGaudi.MsgPack.Light.Benchmark
{
    [Config(typeof(BenchmarkConfig))]
    public class BeerListSerializeBenchmark
    {
        private readonly MemoryStream _memoryStream = Serializers.CreateStream();

        private readonly IMemoryOwner<byte> _buffer = MsgPackSerializer.Serialize(BenchmarkData.Belgium, Serializers.MsgPackLight, out _);

        [Benchmark]
        public long JsonNet()
        {
            _memoryStream.Seek(0, SeekOrigin.Begin);
            JsonSerialize(_memoryStream);
            return _memoryStream.Position;
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
        public long MPCli_Stream()
        {
            _memoryStream.Seek(0, SeekOrigin.Begin);
            MsgPackSerialize(_memoryStream);
            return _memoryStream.Position;
        }

        internal void MsgPackSerialize(MemoryStream memoryStream)
        {
            Serializers.MsgPack.GetSerializer<Beer[]>().Pack(memoryStream, BenchmarkData.Belgium);
        }

        [Benchmark]
        public byte[] MPCli_Array()
        {
            return Serializers.MsgPack.GetSerializer<Beer[]>().PackSingleObject(BenchmarkData.Belgium);
        }

        [Benchmark]
        public int MPLight_Array()
        {
            return MsgPackSerializer.Serialize(BenchmarkData.Belgium, _buffer.Memory.Span, Serializers.MsgPackLight);
        }

        [Benchmark]
        public long MPCliH_Stream()
        {
            _memoryStream.Seek(0, SeekOrigin.Begin);
            Serializers.MsgPackHardcore.GetSerializer<Beer[]>().Pack(_memoryStream, BenchmarkData.Belgium);
            return _memoryStream.Position;
        }

        [Benchmark]
        public byte[] MPCliH_Array()
        {
            return Serializers.MsgPackHardcore.GetSerializer<Beer[]>().PackSingleObject(BenchmarkData.Belgium);
        }

        [Benchmark]
        public int MPLightH_Array()
        {
            return MsgPackSerializer.Serialize(BenchmarkData.Belgium, _buffer.Memory.Span, Serializers.MsgPackLightHardcore);
        }
//
//        [Benchmark]
//        public void MPLightH_Array_AutoMap()
//        {
//            var bytes = MsgPackSerializer.Serialize(BenchmarkData.Belgium, Serializers.MsgPackLightMapAutoGeneration);
//        }
    }
}
