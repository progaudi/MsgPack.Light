using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests.Generators
{
    public class InterfaceInheritanceTest : MapGeneratorTestBase, IClassFixture<InterfaceInheritanceFixture>
    {
        private readonly InterfaceInheritanceFixture _fixture;

        public InterfaceInheritanceTest(InterfaceInheritanceFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void InterfaceInheritance()
        {
            IImageInfo expected = CreateTestObject();

            var actual = MsgPackSerializer.Deserialize<IMegaImageInfo>(MsgPackSerializer.Serialize(expected, _fixture.OldContext), _fixture.NewContext);
            actual.ShouldBeAssignableTo<IMegaImageInfo>();
            AssertEqual(actual, expected);
        }
    }
}