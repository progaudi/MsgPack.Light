using System;

using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests.Generators
{
    public class InterfaceInheritanceTest : MapGeneratorTestBase
    {
        [Theory]
        [ClassData(typeof(FixtureProvider<MapFixture, ArrayFixture>))]
        public void DescendantConverterCanUseAscendant(ContextFixtureBase fixture)
        {
            IImageInfo expected = CreateTestObject();

            var actual = MsgPackSerializer.Deserialize<IMegaImageInfo>(MsgPackSerializer.Serialize(expected, fixture.OldContext), fixture.NewContext);
            actual.ShouldBeAssignableTo<IMegaImageInfo>();
            AssertEqual(actual, expected);
        }

        [Theory]
        [ClassData(typeof(FixtureProvider<MapFixture, ArrayFixture>))]
        public void InterfaceInheritance(ContextFixtureBase fixture)
        {
            IMegaImageInfo expected = new MegaImageInfo
            {
                Credits = Guid.NewGuid().ToString("N"),
                Height = 123,
                Link = Guid.NewGuid().ToString("N"),
                SomeDate = DateTime.UtcNow,
                Width = 345
            };

            var actual = MsgPackSerializer.Deserialize<IMegaImageInfo>(MsgPackSerializer.Serialize(expected, fixture.OldContext), fixture.NewContext);
            actual.ShouldBeAssignableTo<IMegaImageInfo>();
            AssertEqual(actual, expected);
        }

        public class MapFixture : MapContextFixture
        {
            public MapFixture()
            {
                NewContext.GenerateAndRegisterMapConverter<IImageInfo>();
                NewContext.GenerateAndRegisterMapConverter<IMegaImageInfo>();
            }
        }

        public class ArrayFixture : ArrayContextFixture
        {
            public ArrayFixture()
            {
                NewContext.GenerateAndRegisterArrayConverter<IImageInfo>();
                NewContext.GenerateAndRegisterArrayConverter<IMegaImageInfo>();
            }
        }
    }
}