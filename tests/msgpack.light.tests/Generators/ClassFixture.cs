namespace ProGaudi.MsgPack.Light.Tests.Generators
{
    public class ClassFixture : ContextFixture
    {
        public ClassFixture()
        {
            NewContext.GenerateAndRegisterConverter<ImageInfo>();
        }
    }
}