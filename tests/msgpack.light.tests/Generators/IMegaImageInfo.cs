using System;

namespace ProGaudi.MsgPack.Light.Tests.Generators
{
    public interface IMegaImageInfo : IImageInfo
    {
        [MsgPackMapElement("CreationDate")]
        [MsgPackArrayElement(5)]
        DateTime SomeDate { get; }
    }
}