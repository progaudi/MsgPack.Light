using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests.Generators
{
    public class FullTest : MapGeneratorTestBase
    {
        [Theory]
        [ClassData(typeof(FixtureProvider<MapFixture, ArrayFixture>))]
        public void WriteSmoke(ContextFixtureBase fixture)
        {
            IImageInfo testObject = CreateTestObject();

            MsgPackSerializer.Serialize(testObject, fixture.NewContext).ShouldBe(MsgPackSerializer.Serialize(testObject, fixture.OldContext));
        }

        [Theory]
        [ClassData(typeof(FixtureProvider<MapFixture, ArrayFixture>))]
        public void ReadSmoke(ContextFixtureBase fixture)
        {
            IImageInfo expected = CreateTestObject();

            var actual = MsgPackSerializer.Deserialize<IImageInfo>(MsgPackSerializer.Serialize(expected, fixture.NewContext), fixture.NewContext);
            AssertEqual(actual, expected);
        }

        [Theory]
        [ClassData(typeof(FixtureProvider<MapFixture, ArrayFixture>))]
        public void WriteNewReadOld(ContextFixtureBase fixture)
        {
            IImageInfo expected = CreateTestObject();

            var actual = MsgPackSerializer.Deserialize<IImageInfo>(MsgPackSerializer.Serialize(expected, fixture.NewContext), fixture.OldContext);

            AssertEqual(actual, expected);
        }

        [Theory]
        [ClassData(typeof(FixtureProvider<MapFixture, ArrayFixture>))]
        public void WriteOldReadNew(ContextFixtureBase fixture)
        {
            IImageInfo expected = CreateTestObject();

            var actual = MsgPackSerializer.Deserialize<IImageInfo>(MsgPackSerializer.Serialize(expected, fixture.OldContext), fixture.NewContext);
            AssertEqual(actual, expected);
        }
        
        public class MapFixture : MapContextFixture
        {
            public MapFixture()
            {
                NewContext.GenerateAndRegisterMapConverter<IImageInfo, ImageInfo>();
            }
        }

        public class ArrayFixture : ArrayContextFixture
        {
            public ArrayFixture()
            {
                NewContext.GenerateAndRegisterArrayConverter<IImageInfo, ImageInfo>();
            }
        }
    }
}
