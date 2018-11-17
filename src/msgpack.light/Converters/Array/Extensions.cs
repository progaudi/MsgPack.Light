using System;

namespace ProGaudi.MsgPack.Converters.Array
{
    internal static class Extensions
    {
        public static int GetBufferSize<TElement>(this ReadOnlyMemory<TElement> value, IMsgPackFormatter<TElement> elementFormatter)
        {
            var span = value.Span;
            if (span.Length == 0)
                return DataLengths.FixArrayHeader;

            if (elementFormatter.HasConstantSize)
                return span.Length * elementFormatter.GetBufferSize(span[0]) + DataLengths.GetArrayHeaderLength(span.Length);

            var result = DataLengths.Array32Header;
            for (var i = 0; i < span.Length; i++)
            {
                result += elementFormatter.GetBufferSize(span[i]);
            }

            return result;
        }
    }
}
