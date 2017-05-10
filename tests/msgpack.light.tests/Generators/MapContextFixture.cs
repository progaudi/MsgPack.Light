namespace ProGaudi.MsgPack.Light.Tests.Generators
{
    public abstract class MapContextFixture : ContextFixtureBase
    {
        protected MapContextFixture()
        {
            OldContext = new MsgPackContext();
            OldContext.RegisterConverter<IImageInfo>(new ImageInfoMapConverter());
            OldContext.RegisterConverter<ImageInfo>(new ImageInfoMapConverter());
            OldContext.RegisterConverter<BigImageInfo>(new ImageInfoMapConverter());
            OldContext.RegisterConverter<IMegaImageInfo>(new ImageInfoMapConverter());

            NewContext = new MsgPackContext();
        }
    }
}
