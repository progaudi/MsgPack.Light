// <copyright file="Enums.cs" company="eVote">
//   Copyright © eVote
// </copyright>

using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests.Writer
{
    public class Enums
    {
        [Theory]
        [InlineData(TestEnum.TestValue1, new byte[] { (byte)TestEnum.TestValue1 })]
        [InlineData(TestEnum.TestValue2, new byte[] { (byte)TestEnum.TestValue2 })]
        public void WriteEnum(TestEnum enumValue, byte[] expectedResult)
        {
            var result = MsgPackSerializer.Serialize(enumValue);
            result.ShouldNotBeNull();
        }
    }

    public enum TestEnum
    {
        TestValue1,
        TestValue2
    }
}