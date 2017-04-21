using System;

using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests.Generators
{
    public class InterfaceInheritanceTest : MapGeneratorTestBase, IClassFixture<InterfaceInheritanceTest.Fixture>
    {
        private readonly Fixture _testFixture;

        public InterfaceInheritanceTest(Fixture testFixture)
        {
            _testFixture = testFixture;
        }

        [Fact]
        public void DescendantConverterCanUseAscendant()
        {
            IImageInfo expected = CreateTestObject();

            var actual = MsgPackSerializer.Deserialize<IMegaImageInfo>(MsgPackSerializer.Serialize(expected, _testFixture.OldContext), _testFixture.NewContext);
            actual.ShouldBeAssignableTo<IMegaImageInfo>();
            AssertEqual(actual, expected);
        }

        [Fact]
        public void InterfaceInheritance()
        {
            IMegaImageInfo expected = new MegaImageInfo
            {
                Credits = Guid.NewGuid().ToString("N"),
                Height = 123,
                Link = Guid.NewGuid().ToString("N"),
                SomeDate = DateTime.UtcNow,
                Width = 345
            };

            var actual = MsgPackSerializer.Deserialize<IMegaImageInfo>(MsgPackSerializer.Serialize(expected, _testFixture.OldContext), _testFixture.NewContext);
            actual.ShouldBeAssignableTo<IMegaImageInfo>();
            AssertEqual(actual, expected);
        }

        public class Fixture : ContextFixture
        {
            public Fixture()
            {
                NewContext.GenerateAndRegisterConverter<IImageInfo>();
                NewContext.GenerateAndRegisterConverter<IMegaImageInfo>();
            }
        }
    }
}