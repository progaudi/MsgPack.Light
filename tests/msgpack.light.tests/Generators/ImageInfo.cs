// <copyright file="ImageInfo.cs" company="eVote">
//   Copyright © eVote
// </copyright>

namespace ProGaudi.MsgPack.Light.Tests.Generators
{
    public class ImageInfo : IImageInfo
    {
        [MsgPackMapElement("Width")]
        public int Width { get; set; }

        [MsgPackMapElement("Height")]
        public int Height { get; set; }

        [MsgPackMapElement("Link")]
        public string Link { get; set; }

        [MsgPackMapElement("Credits")]
        public string Credits { get; set; }
    }
}