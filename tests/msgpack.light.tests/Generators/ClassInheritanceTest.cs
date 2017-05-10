using System;

using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests.Generators
{
    public class ClassInheritanceTest : MapGeneratorTestBase
    {
        [Theory]
        [ClassData(typeof(FixtureProvider<MapFixture, ArrayFixture>))]
        public void WriteSmoke(ContextFixtureBase fixture)
        {
            var testObject = new BigImageInfo
            {
                Credits = Guid.NewGuid().ToString("N"),
                Height = 123,
                Link = Guid.NewGuid().ToString("N"),
                Size = 234,
                Width = 345
            };

            MsgPackSerializer.Serialize(testObject, fixture.NewContext).ShouldBe(MsgPackSerializer.Serialize(testObject, fixture.OldContext));
        }

       

        public class MapFixture : MapContextFixture
        {
            public MapFixture()
            {
                NewContext.GenerateAndRegisterMapConverter<BigImageInfo>();
            }
        }

        public class ArrayFixture : ArrayContextFixture
        {
            public ArrayFixture()
            {
                NewContext.GenerateAndRegisterArrayConverter<BigImageInfo>();
            }
        }
    }
}
