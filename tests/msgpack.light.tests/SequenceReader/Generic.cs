using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests.SequenceReader
{
    public class Generic
    {
        [Fact]
        public void IntTest()
        {
            var context = new MsgPackContext();
            context.RegisterGenericSequenceParser(typeof(GenericSequenceParser<>));

            var a = MsgPackSerializer.Deserialize<A<int>>(new byte[] {10}.ToMultipleSegments(), context, out var x);
            x.ShouldBe(1);
            a.F.ShouldBe(10);
        }

        [Fact]
        public void StringTest()
        {
            var context = new MsgPackContext();
            context.RegisterGenericSequenceParser(typeof(GenericSequenceParser<>));

            var a = MsgPackSerializer.Deserialize<A<string>>(new byte[] { 163, 97, 98, 99 }.ToMultipleSegments(), context, out var x);
            x.ShouldBe(4);
            a.F.ShouldBe("abc");
        }
    }
}
