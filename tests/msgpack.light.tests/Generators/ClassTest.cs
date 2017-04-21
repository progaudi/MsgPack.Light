using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests.Generators
{
    public class ClassTest : MapGeneratorTestBase, IClassFixture<ClassTest.Fixture>
    {
        private readonly Fixture _fixture;

        public ClassTest(Fixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void WriteSmoke()
        {
            var testObject = CreateTestObject();

            MsgPackSerializer.Serialize(testObject, _fixture.NewContext).ShouldBe(MsgPackSerializer.Serialize(testObject, _fixture.OldContext));
        }

        [Fact]
        public void ReadSmoke()
        {
            var expected = CreateTestObject();

            var actual = MsgPackSerializer.Deserialize<ImageInfo>(MsgPackSerializer.Serialize(expected, _fixture.NewContext), _fixture.NewContext);
            AssertEqual(actual, expected);
        }

        [Fact]
        public void WriteNewReadOld()
        {
            var expected = CreateTestObject();

            var actual = MsgPackSerializer.Deserialize<ImageInfo>(MsgPackSerializer.Serialize(expected, _fixture.NewContext), _fixture.OldContext);

            AssertEqual(actual, expected);
        }

        [Fact]
        public void WriteOldReadNew()
        {
            var expected = CreateTestObject();

            var actual = MsgPackSerializer.Deserialize<ImageInfo>(MsgPackSerializer.Serialize(expected, _fixture.OldContext), _fixture.NewContext);
            AssertEqual(actual, expected);
        }

        public class Fixture : ContextFixture
        {
            public Fixture()
            {
                NewContext.GenerateAndRegisterConverter<ImageInfo>();
            }
        }
    }
}
