using System.Collections.Generic;
using System.Linq;

namespace ProGaudi.MsgPack.Converters.Collection
{
    internal static class Extensions
    {
        public static int GetBufferSize<TCollection, TElement>(this TCollection value, IMsgPackFormatter<TElement> elementFormatter)
            where TCollection : ICollection<TElement>
        {
            if (value == null)
                return DataLengths.Nil;

            if (value.Count == 0)
                return DataLengths.FixArrayHeader;

            if (elementFormatter.HasConstantSize)
                return value.Count * elementFormatter.GetBufferSize(value.First()) + DataLengths.GetArrayHeaderLength(value.Count);

            var result = DataLengths.Array32Header;
            foreach (var element in value)
            {
                result += elementFormatter.GetBufferSize(element);
            }

            return result;
        }
    }
}
