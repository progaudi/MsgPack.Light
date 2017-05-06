using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ProGaudi.MsgPack.Light.Tests
{
    public class EnumValuesProvider<T> : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            return Enum
                .GetValues(typeof(T))
                .Cast<T>()
                .Select(v => new object[] { v })
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}