using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests
{
    public class MsgPackContextTests
    {
        [Fact]
        public void GetConverterShouldNotThrow()
        {
            var context = new MsgPackContext();

            var ex = Should.Throw<ConverterNotFoundException>(() => context.GetConverter<MsgPackContextTests>());
            ex.ObjectType.ShouldBe(typeof(MsgPackContextTests));
        }
    }
}