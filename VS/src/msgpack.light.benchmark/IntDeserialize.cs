using System.IO;

using BenchmarkDotNet.Attributes;

using MsgPack.Light;
using MsgPack.Serialization;

namespace msgpack.light.benchmark
{
    public class IntDeserialize
    {
        private readonly MessagePackSerializer<int[]> _messagePackSerializer;

        private readonly byte[] _bytes;

        private readonly MemoryStream _stream;

        private readonly MsgPackContext _mplightContext;

        public IntDeserialize()
        {
            _messagePackSerializer = SerializationContext.Default.GetSerializer<int[]>();
            _bytes = _messagePackSerializer.PackSingleObject(Integers.Data);
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
            var data = MsgPackSerializer.Deserialize<int[]>(_bytes, _mplightContext);
        }

        [Benchmark]
        public void MPLight_Stream()
        {
            _stream.Seek(0, SeekOrigin.Begin);
            var data = MsgPackSerializer.Deserialize<int[]>(_stream, _mplightContext);
        }
    }
}
