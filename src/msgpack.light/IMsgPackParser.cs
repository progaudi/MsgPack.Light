using System;

namespace ProGaudi.MsgPack
{
    public interface IMsgPackParser<out T>
    {
        /// <summary>
        /// Parses source into <typeparamref name="T"/>.
        /// </summary>
        /// <param name="source">Buffer to read.</param>
        /// <param name="readSize">Count of read bytes</param>
        /// <returns>Value</returns>
        T Parse(Span<byte> source, out int readSize);
    }
}