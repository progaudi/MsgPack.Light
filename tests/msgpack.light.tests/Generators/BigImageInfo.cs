namespace ProGaudi.MsgPack.Light.Tests.Generators
{
    public class BigImageInfo : ImageInfo
    {
        [MsgPackMapElement("Size2")]
        [MsgPackArrayElement(4)]
        public int Size { get; set; }
    }
}