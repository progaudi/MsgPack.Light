namespace ProGaudi.MsgPack.Light.Tests.Generators
{
    public interface IImageInfo
    {
        [MsgPackMapElement("Width")]
        [MsgPackArrayElement(0)]
        int Width { get; }

        [MsgPackMapElement("Height")]
        [MsgPackArrayElement(1)]
        int Height { get; }

        [MsgPackMapElement("Link")]
        [MsgPackArrayElement(2)]
        string Link { get; }

        [MsgPackMapElement("Credits")]
        [MsgPackArrayElement(4)]
        string Credits { get; }

        string NotSerializedProperty { get; }
    }
}