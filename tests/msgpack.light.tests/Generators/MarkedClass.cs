namespace ProGaudi.MsgPack.Light.Tests.Generators
{
    [MsgPackMap]
    public class MarkedClass
    {
        [MsgPackMapElement("Asdf")]
        public int A { get; set; }
    }
}
