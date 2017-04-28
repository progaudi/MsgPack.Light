// <copyright file="IImageInfo.cs" company="eVote">
//   Copyright © eVote
// </copyright>

namespace ProGaudi.MsgPack.Light.Tests.Generators
{
    public interface IImageInfo
    {
        [MsgPackMapElement("Width")]
        int Width { get; }

        [MsgPackMapElement("Height")]
        int Height { get; }

        [MsgPackMapElement("Link")]
        string Link { get; }

        [MsgPackMapElement("Credits")]
        string Credits { get; }
    }
}