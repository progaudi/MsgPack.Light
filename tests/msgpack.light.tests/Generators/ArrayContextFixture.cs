namespace ProGaudi.MsgPack.Light.Tests.Generators
{
    public abstract class ArrayContextFixture : ContextFixtureBase
    {
        protected ArrayContextFixture()
        {
            OldContext = new MsgPackContext();
            OldContext.RegisterConverter<IImageInfo>(new ImageInfoArrayConverter());
            OldContext.RegisterConverter<ImageInfo>(new ImageInfoArrayConverter());
            OldContext.RegisterConverter<BigImageInfo>(new ImageInfoArrayConverter());
            OldContext.RegisterConverter<IMegaImageInfo>(new ImageInfoArrayConverter());

            NewContext = new MsgPackContext();
        }
    }
}
