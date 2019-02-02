using System;

namespace ProGaudi.MsgPack.Light.Benchmark.Data
{
    public class BeerTypeConverter : IMsgPackFormatter<BeerType>, IMsgPackParser<BeerType>
    {
        public int GetBufferSize(BeerType value) => DataLengths.PositiveFixInt;

        public bool HasConstantSize => true;

        public int Format(Span<byte> destination, BeerType value) => MsgPackSpec.WritePositiveFixInt(destination, (byte)value);

        public BeerType Parse(ReadOnlySpan<byte> source, out int readSize) => (BeerType) MsgPackSpec.ReadPositiveFixInt(source, out readSize);
    }
}
