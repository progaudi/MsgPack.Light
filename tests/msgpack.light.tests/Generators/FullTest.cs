using ProGaudi.MsgPack.Light.Converters.Generation;

using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests.Generators
{
    [Collection(nameof(MapConverterGenerator))]
    public class FullTest : MapGeneratorTestBase, IClassFixture<FullFixture>
    {
        private readonly FullFixture _fixture;

        public FullTest(FullFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void WriteSmoke()
        {
            IImageInfo testObject = CreateTestObject();

            MsgPackSerializer.Serialize(testObject, _fixture.NewContext).ShouldBe(MsgPackSerializer.Serialize(testObject, _fixture.OldContext));
        }

        [Fact]
        public void ReadSmoke()
        {
            IImageInfo expected = CreateTestObject();

            var actual = MsgPackSerializer.Deserialize<IImageInfo>(MsgPackSerializer.Serialize(expected, _fixture.NewContext), _fixture.NewContext);
            AssertEqual(actual, expected);
        }

        [Fact]
        public void WriteNewReadOld()
        {
            IImageInfo expected = CreateTestObject();

            var actual = MsgPackSerializer.Deserialize<IImageInfo>(MsgPackSerializer.Serialize(expected, _fixture.NewContext), _fixture.OldContext);

            AssertEqual(actual, expected);
        }

        [Fact]
        public void WriteOldReadNew()
        {
            IImageInfo expected = CreateTestObject();

            var actual = MsgPackSerializer.Deserialize<IImageInfo>(MsgPackSerializer.Serialize(expected, _fixture.OldContext), _fixture.NewContext);
            AssertEqual(actual, expected);
        }
    }
}
