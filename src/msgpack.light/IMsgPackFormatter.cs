using System;

namespace ProGaudi.MsgPack
{
    public interface IMsgPackFormatter<in T>
    {
        /// <summary>
        /// Gets maximum possible size for <paramref name="value"/> instance. Implementation should be balanced about speed
        /// and precision. Overestimating is usually better, since underestimating will result to errors in <see cref="Format"/>
        /// method.
        /// </summary>
        /// <param name="value">Value to estimate.</param>
        /// <returns>Size of buffer to serialize value.</returns>
        int GetBufferSize(T value);
        
        /// <summary>
        /// If your type is always serializes to same amount of bytes, seal it and set this to true.
        /// It will speed up serialization of your arrays.
        /// </summary>
        bool HasConstantSize { get; }

        /// <summary>
        /// Formats <paramref name="value"/> into <paramref name="destination"/>.
        /// </summary>
        /// <param name="destination">Buffer, large enough to hold <paramref name="value"/>.</param>
        /// <param name="value">Value to serialize.</param>
        /// <returns>Actual count of used bytes.</returns>
        int Format(Span<byte> destination, T value);
    }
}