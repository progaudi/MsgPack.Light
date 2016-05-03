using System.IO;

using BenchmarkDotNet.Attributes;

using MsgPack.Light;
using MsgPack.Serialization;

namespace msgpack.light.benchmark
{
    public class IntSerialize
    {
        private readonly MessagePackSerializer<int[]> _messagePackSerializer;

        public IntSerialize()
        {
            _messagePackSerializer = SerializationContext.Default.GetSerializer<int[]>();
        }

        [Benchmark]
        public void MPCli_Array()
        {
            var bytes = _messagePackSerializer.PackSingleObject(Integers.Data);
        }

        [Benchmark(Baseline = true)]
        public void MPCli_Stream()
        {
            var stream = new MemoryStream();
            _messagePackSerializer.Pack(stream, Integers.Data);
        }

        [Benchmark]
        public void MPLight_Array()
        {
            var bytes = MsgPackSerializer.Serialize(Integers.Data);
        }

        [Benchmark]
        public void MPLight_Stream()
        {
            var stream = new MemoryStream();
            MsgPackSerializer.Serialize(Integers.Data, stream);
        }
    }
}
