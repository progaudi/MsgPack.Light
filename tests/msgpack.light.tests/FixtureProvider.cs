using System.Collections;
using System.Collections.Generic;

namespace ProGaudi.MsgPack.Light.Tests
{
    public class FixtureProvider<T1, T2> : IEnumerable<object[]>
        where T1 : new()
        where T2 : new()
    {
        private static readonly IEnumerable<object[]> Fixtures = new List<object[]>
        {
            new object[] { new T1() },
            new object[] { new T2() }
        };

        public IEnumerator<object[]> GetEnumerator()
        {
            return Fixtures.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}