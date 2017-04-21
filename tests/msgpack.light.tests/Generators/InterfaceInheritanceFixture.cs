namespace ProGaudi.MsgPack.Light.Tests.Generators
{
    public class InterfaceInheritanceFixture : ContextFixture
    {
        public InterfaceInheritanceFixture()
        {
            NewContext.GenerateAndRegisterConverter<IImageInfo>();
            NewContext.GenerateAndRegisterConverter<IMegaImageInfo>();
        }
    }
}