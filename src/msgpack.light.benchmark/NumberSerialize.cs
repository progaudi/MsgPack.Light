using System.Buffers;
using System.IO;

using BenchmarkDotNet.Attributes;

using MsgPack.Serialization;

namespace ProGaudi.MsgPack.Light.Benchmark
{
    [Config(typeof(BenchmarkConfig))]
    public abstract class NumberSerialize<T>
    {
        private readonly MessagePackSerializer<T[]> _messagePackSerializer;

        private readonly MsgPackContext _mpLightContext;

        private readonly MemoryStream _stream = Serializers.CreateStream();

        private readonly IMemoryOwner<byte> _buffer;

        protected abstract T[] Numbers { get; }

        protected NumberSerialize()
        {
            _messagePackSerializer = SerializationContext.Default.GetSerializer<T[]>();
            _mpLightContext = new MsgPackContext();
            _buffer = MsgPackSerializer.Serialize(Numbers, _mpLightContext, out _);
        }

        [Benchmark]
        public byte[] MPCli_Array()
        {
            return _messagePackSerializer.PackSingleObject(Numbers);
        }

        [Benchmark(Baseline = true)]
        public long MPCli_Stream()
        {
            _stream.Seek(0, SeekOrigin.Begin);
            _messagePackSerializer.Pack(_stream, Numbers);
            return _stream.Position;
        }

        [Benchmark]
        public int MPLight_Array()
        {
            return MsgPackSerializer.Serialize(Numbers, _buffer.Memory.Span, _mpLightContext);
        }
    }
}
