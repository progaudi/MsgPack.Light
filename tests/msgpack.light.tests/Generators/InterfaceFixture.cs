namespace ProGaudi.MsgPack.Light.Tests.Generators
{
    public class InterfaceFixture : ContextFixture
    {
        public InterfaceFixture()
        {
            NewContext.GenerateAndRegisterConverter<IImageInfo>();
        }
    }
}