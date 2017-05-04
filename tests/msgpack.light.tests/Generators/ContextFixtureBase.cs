namespace ProGaudi.MsgPack.Light.Tests.Generators
{
    public abstract class ContextFixtureBase
    {
        public MsgPackContext NewContext { get; protected set; }

        public MsgPackContext OldContext { get; protected set; }
    }
}