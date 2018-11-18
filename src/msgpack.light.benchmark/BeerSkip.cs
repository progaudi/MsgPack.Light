using System.IO;

using BenchmarkDotNet.Attributes;

using MsgPack;

using ProGaudi.MsgPack.Light.Benchmark.Data;

namespace ProGaudi.MsgPack.Light.Benchmark
{
    [Config(typeof(BenchmarkConfig))]
    public class BeerSkip
    {
        private readonly MemoryStream _inputStream;

        private readonly byte[] _inputBytes;

        private readonly MsgPackContext _msgPackContext;

        private readonly Unpacker _unpacker;

        public BeerSkip()
        {
            _inputStream = Serializers.CreateStream();
            new BeerSerializeBenchmark().MsgPackSerialize(_inputStream);
            _inputBytes = _inputStream.ToArray();

            _unpacker = Unpacker.Create(_inputStream);

            _msgPackContext = new MsgPackContext();
            _msgPackContext.RegisterParser(_ => new SkipConverter<Beer>());

        }

        [Benchmark(Baseline = true)]
        public long? MPackCli_Skip()
        {
            _inputStream.Position = 0;
            return _unpacker.Skip();
        }

        [Benchmark]
        public Beer MsgPackLight_Skip_Array()
        {
            _inputStream.Position = 0;
            return MsgPackSerializer.Deserialize<Beer>(_inputBytes, _msgPackContext, out _);
        }
    }
}
