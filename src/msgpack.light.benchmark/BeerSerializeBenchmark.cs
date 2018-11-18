using System;
using System.Buffers;
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

        private readonly MemoryStream _memoryStream = new MemoryStream();

        private readonly IMemoryOwner<byte> _buffer;

        public BeerSerializeBenchmark()
        {
            _testBeer = new Beer
            {
                Brand = "Westvleteren Zes",
                Alcohol = 6.2F,
                Brewery = "Sint-Sixtusabdij van Westvleteren",
                Sort = new List<string> {"trappist"}
            };
            _buffer = MsgPackSerializer.Serialize(_testBeer, Serializers.MsgPackLightHardcore, out _);
        }

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
                Serializers.Newtonsoft.Serialize(writer, _testBeer);
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
            Serializers.MsgPack.GetSerializer<Beer>().Pack(memoryStream, _testBeer);
        }

        [Benchmark]
        public long MPSharp_Stream()
        {
            _memoryStream.Seek(0, SeekOrigin.Begin);
            MessagePackSerializer.Serialize(_memoryStream, _testBeer);
            return _memoryStream.Position;
        }

        [Benchmark]
        public byte[] MPSharp_Array()
        {
            return MessagePackSerializer.Serialize(_testBeer);
        }

        [Benchmark]
        public ArraySegment<byte> MPSharp_Array_Unsafe()
        {
            return MessagePackSerializer.SerializeUnsafe(_testBeer);
        }

        [Benchmark]
        public byte[] MPCli_Array()
        {
            return Serializers.MsgPack.GetSerializer<Beer>().PackSingleObject(_testBeer);
        }

        [Benchmark]
        public int MPLight_Array()
        {
            return MsgPackSerializer.Serialize(_testBeer, _buffer.Memory.Span, Serializers.MsgPackLight);
        }

        [Benchmark]
        public long MPCliH_Stream()
        {
            _memoryStream.Seek(0, SeekOrigin.Begin);
            Serializers.MsgPackHardcore.GetSerializer<Beer>().Pack(_memoryStream, _testBeer);
            return _memoryStream.Position;
        }

        [Benchmark]
        public byte[] MPCliH_Array()
        {
            return Serializers.MsgPackHardcore.GetSerializer<Beer>().PackSingleObject(_testBeer);
        }

        [Benchmark]
        public int MPLightH_Array()
        {
            return MsgPackSerializer.Serialize(_testBeer, _buffer.Memory.Span, Serializers.MsgPackLightHardcore);
        }

//        [Benchmark]
//        public void MPLightH_Array_AutoMap()
//        {
//            var bytes = MsgPackSerializer.Serialize(_testBeer, Serializers.MsgPackLightMapAutoGeneration);
//        }
//
//        [Benchmark]
//        public void MPLightH_Array_AutoArray()
//        {
//            var bytes = MsgPackSerializer.Serialize(_testBeer, Serializers.MsgPackLightArrayAutoGeneration);
//        }
    }
}
