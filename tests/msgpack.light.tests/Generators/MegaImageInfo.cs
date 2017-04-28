// <copyright file="IMegaImageInfo.cs" company="eVote">
//   Copyright © eVote
// </copyright>

using System;

namespace ProGaudi.MsgPack.Light.Tests.Generators
{
    public class MegaImageInfo : ImageInfo, IMegaImageInfo
    {
        public DateTime SomeDate { get; set; }
    }
}