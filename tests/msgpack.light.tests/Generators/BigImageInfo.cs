namespace ProGaudi.MsgPack.Light.Tests.Generators
{
    public class BigImageInfo : ImageInfo
    {
        [MsgPackMapElement("Size2")]
        public int Size { get; set; }
    }
}