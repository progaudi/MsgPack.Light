namespace ProGaudi.MsgPack.Light.Tests.Generators
{
    public abstract class ContextFixture
    {
        protected ContextFixture()
        {
            OldContext = new MsgPackContext();
            OldContext.RegisterConverter<IImageInfo>(new ImageInfoConverter());
            OldContext.RegisterConverter<ImageInfo>(new ImageInfoConverter());

            NewContext = new MsgPackContext();
        }

        public MsgPackContext NewContext { get; }

        public MsgPackContext OldContext { get; }
    }
}
