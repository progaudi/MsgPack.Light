using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests.Writer
{
    public class Generic
    {
        [Fact]
        public void IntTest()
        {
            var context = new MsgPackContext();
            context.RegisterGenericFormatter(typeof(GenericFormatter<>));

            using (var bytes = MsgPackSerializer.Serialize(new A<int> {F = 10}, context, out var x))
            {
                x.ShouldBe(1);
                bytes.Memory.Slice(0, x).ShouldBe(new byte[] { 10 });
            }
        }

        [Fact]
        public void StringTest()
        {
            var context = new MsgPackContext();
            context.RegisterGenericFormatter(typeof(GenericFormatter<>));

            using (var bytes = MsgPackSerializer.Serialize(new A<string> {F = "abc"}, context, out var x))
            {
                x.ShouldBe(4);
                bytes.Memory.Slice(0, x).ShouldBe(new byte[] { 163, 97, 98, 99 });
            }
        }
    }
}
