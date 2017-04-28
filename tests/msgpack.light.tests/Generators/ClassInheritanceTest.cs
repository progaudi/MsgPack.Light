using System;

using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests.Generators
{
    public class ClassInheritanceTest : MapGeneratorTestBase, IClassFixture<ClassInheritanceTest.Fixture>
    {
        private readonly Fixture _fixture;

        public ClassInheritanceTest(Fixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void WriteSmoke()
        {
            var testObject = new BigImageInfo
            {
                Credits = Guid.NewGuid().ToString("N"),
                Height = 123,
                Link = Guid.NewGuid().ToString("N"),
                Size = 234,
                Width = 345
            };

            MsgPackSerializer.Serialize(testObject, _fixture.NewContext).ShouldBe(MsgPackSerializer.Serialize(testObject, _fixture.OldContext));
        }

        public class Fixture : ContextFixture
        {
            public Fixture()
            {
                NewContext.GenerateAndRegisterConverter<BigImageInfo>();
            }
        }
    }
}
