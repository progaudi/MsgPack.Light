using System.IO;

using BenchmarkDotNet.Attributes;

using MsgPack.Serialization;

namespace ProGaudi.MsgPack.Light.Benchmark
{
    [Config(typeof(BenchmarkConfig))]
    public abstract class NumberDeserialize<T>
    {
        private readonly MessagePackSerializer<T[]> _messagePackSerializer;

        private readonly byte[] _bytes;

        private readonly MemoryStream _stream;

        private readonly MsgPackContext _mpLightContext;

        protected abstract  T[] Numbers { get; }

        protected NumberDeserialize()
        {
            _messagePackSerializer = SerializationContext.Default.GetSerializer<T[]>();
            _bytes = _messagePackSerializer.PackSingleObject(Numbers);
            _stream = new MemoryStream(_bytes);
            _mpLightContext = new MsgPackContext();
        }

        [Benchmark]
        public T[] MPCli_Array()
        {
            return _messagePackSerializer.UnpackSingleObject(_bytes);
        }

        [Benchmark(Baseline = true)]
        public T[] MPCli_Stream()
        {
            _stream.Seek(0, SeekOrigin.Begin);
            return _messagePackSerializer.Unpack(_stream);
        }

        [Benchmark]
        public T[] MPLight_Array()
        {
            return MsgPackSerializer.Deserialize<T[]>(_bytes, _mpLightContext, out _);
        }
    }
}
