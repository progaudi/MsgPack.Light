using System.Collections.Generic;

namespace ProGaudi.MsgPack.Converters.List
{
    internal static class Extensions
    {
        public static int GetBufferSize<TList, TElement>(this TList value, IMsgPackFormatter<TElement> elementFormatter)
            where TList : IList<TElement>
        {
            if (value == null)
                return DataLengths.Nil;

            if (value.Count == 0)
                return DataLengths.FixArrayHeader;

            if (elementFormatter.HasConstantSize)
                return value.Count * elementFormatter.GetBufferSize(value[0]) + DataLengths.GetArrayHeaderLength(value.Count);

            var result = DataLengths.Array32Header;
            for (var i = 0; i < value.Count; i++)
            {
                result += elementFormatter.GetBufferSize(value[i]);
            }

            return result;
        }
    }
}
