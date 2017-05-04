// <copyright file="ImageInfo.cs" company="eVote">
//   Copyright © eVote
// </copyright>

namespace ProGaudi.MsgPack.Light.Tests.Generators
{
    public class ImageInfo : IImageInfo
    {
        [MsgPackMapElement("Width")]
        [MsgPackArrayElement(0)]
        public int Width { get; set; }

        [MsgPackMapElement("Height")]
        [MsgPackArrayElement(1)]
        public int Height { get; set; }

        [MsgPackMapElement("Link")]
        [MsgPackArrayElement(2)]
        public string Link { get; set; }

        [MsgPackMapElement("Credits")]
        [MsgPackArrayElement(4)]
        public string Credits { get; set; }

        public string NotSerializedProperty { get; set; }
    }
}