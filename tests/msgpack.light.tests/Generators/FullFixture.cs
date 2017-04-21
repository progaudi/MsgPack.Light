namespace ProGaudi.MsgPack.Light.Tests.Generators
{
    public class FullFixture : ContextFixture
    {
        public FullFixture()
        {
            NewContext.GenerateAndRegisterConverter<IImageInfo, ImageInfo>();
        }
    }
}