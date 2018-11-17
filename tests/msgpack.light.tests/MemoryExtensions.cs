using System;

using Shouldly;

namespace ProGaudi.MsgPack.Light.Tests
{
    public static class MemoryExtensions
    {
        public static void ShouldBe<T>(this Memory<T> actual, Memory<T> expected) => actual.ToArray().ShouldBe(expected.ToArray());
    }
}
