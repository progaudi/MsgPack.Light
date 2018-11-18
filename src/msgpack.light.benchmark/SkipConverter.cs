using System;

namespace ProGaudi.MsgPack.Light.Benchmark
{
    public class SkipConverter<T> : IMsgPackParser<T>
    {
        public T Parse(ReadOnlySpan<byte> source, out int readSize)
        {
            var token = MsgPackSpec.ReadToken(source);
            readSize = token.Length;
            return default;
        }
    }
}
