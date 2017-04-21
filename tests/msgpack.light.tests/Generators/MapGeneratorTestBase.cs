using Shouldly;

namespace ProGaudi.MsgPack.Light.Tests.Generators
{
    public class MapGeneratorTestBase
    {
        protected static ImageInfo CreateTestObject()
        {
            return new ImageInfo
            {
                Credits = "123",
                Height = 234,
                Link = "345ыва",
                Width = 456
            };
        }

        protected static void AssertEqual(IImageInfo actual, IImageInfo expected)
        {
            actual.Credits.ShouldBe(expected.Credits);
            actual.Height.ShouldBe(expected.Height);
            actual.Width.ShouldBe(expected.Width);
            actual.Link.ShouldBe(expected.Link);
        }
    }
}