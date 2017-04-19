using System.IO;

using BenchmarkDotNet.Attributes;

using MsgPack.Serialization;

namespace ProGaudi.MsgPack.Light.Benchmark
{
    [Config(typeof(BenchmarkConfig))]
    public abstract class NumberSerialize<T>
    {
        private readonly MessagePackSerializer<T[]> _messagePackSerializer;

        private readonly MsgPackContext _mplightContext;

        protected abstract T[] Numbers { get; }

        protected NumberSerialize()
        {
            _messagePackSerializer = SerializationContext.Default.GetSerializer<T[]>();
            _mplightContext = new MsgPackContext();
        }

        [Benchmark]
        public void MPCli_Array()
        {
            var bytes = _messagePackSerializer.PackSingleObject(Numbers);
        }

        [Benchmark(Baseline = true)]
        public void MPCli_Stream()
        {
            var stream = new MemoryStream();
            _messagePackSerializer.Pack(stream, Numbers);
        }

        [Benchmark]
        public void MPLight_Array()
        {
            var bytes = MsgPackSerializer.Serialize(Numbers, _mplightContext);
        }

        [Benchmark]
        public void MPLight_Stream()
        {
            var stream = new MemoryStream();
            MsgPackSerializer.Serialize(Numbers, stream, _mplightContext);
        }
    }
}
