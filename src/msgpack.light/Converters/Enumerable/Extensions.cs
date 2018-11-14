using System.Collections.Generic;

namespace ProGaudi.MsgPack.Converters.Enumerable
{
    internal static class Extensions
    {
        public static int GetBufferSize<TEnumerable, TElement>(this TEnumerable value, IMsgPackFormatter<TElement> elementFormatter)
            where TEnumerable : IEnumerable<TElement>
        {
            if (value == null)
                return DataLengths.Nil;

            var result = DataLengths.Array32Header;
            foreach (var element in value)
            {
                result += elementFormatter.GetBufferSize(element);
            }

            return result;
        }
    }
}
