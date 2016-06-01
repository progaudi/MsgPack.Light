using System.IO;
using System.Linq;

using BenchmarkDotNet.Attributes;

using MsgPack.Light;
using MsgPack.Serialization;

namespace msgpack.light.benchmark
{
    [Config(typeof(BenchmarkConfig))]
    public abstract class NumberDeserialize<T>
    {
        private readonly MessagePackSerializer<T[]> _messagePackSerializer;

        private readonly byte[] _bytes;

        private readonly MemoryStream _stream;

        private readonly MsgPackContext _mplightContext;

        protected abstract  T[] Numbers { get; }

        protected NumberDeserialize()
        {
            _messagePackSerializer = SerializationContext.Default.GetSerializer<T[]>();
            _bytes = _messagePackSerializer.PackSingleObject(Numbers);
            _stream = new MemoryStream(_bytes);
            _mplightContext = new MsgPackContext();
        }

        [Benchmark]
        public void MPCli_Array()
        {
            var data = _messagePackSerializer.UnpackSingleObject(_bytes);
        }

        [Benchmark(Baseline = true)]
        public void MPCli_Stream()
        {
            _stream.Seek(0, SeekOrigin.Begin);
            var data = _messagePackSerializer.Unpack(_stream);
        }

        [Benchmark]
        public void MPLight_Array()
        {
            var data = MsgPackSerializer.Deserialize<T[]>(_bytes, _mplightContext);
        }

        [Benchmark]
        public void MPLight_Stream()
        {
            _stream.Seek(0, SeekOrigin.Begin);
            var data = MsgPackSerializer.Deserialize<T[]>(_stream, _mplightContext);
        }
    }

    public class IntDeserialize : NumberDeserialize<int>
    {
        protected override int[] Numbers => Data.Integers;
    }

    public class DoubleDeserialize : NumberDeserialize<double>
    {
        protected override double[] Numbers => Data.Doubles;
    }
}
