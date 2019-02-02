using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests.SequenceReader
{
    public class String
    {
        [Theory]
        [InlineData("asdf", new byte[] { 164, 97, 115, 100, 102 })]
        [InlineData("12345678901234567890", new byte[] { 180, 49, 50, 51, 52, 53, 54, 55, 56, 57, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 48 })]
        [InlineData("1234567890123456789012345678901234567890", new byte[] { 217, 40, 49, 50, 51, 52, 53, 54, 55, 56, 57, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 48 })]
        [InlineData("", new byte[] { 160 })]
        [InlineData("08612364123065712639gr1h2j3grtk1h23kgfrt1hj2g3fjrgf1j2hg08612364123065712639gr1h2j3grtk1h23kgfrt1hj2g3fjrgf1j2hg08612364123065712639gr1h2j3grtk1h23kgfrt1hj2g3fjrgf1j2hg08612364123065712639gr1h2j3grtk1h23kgfrt1hj2g3fjrgf1j2hg08612364123065712639gr1h2j3grtk1h23kgfrt1hj2g3fjrgf1j2hg08612364123065712639gr1h2j3grtk1h23kgfrt1hj2g3fjrgf1j2hg", new byte[] {
                    218, 1, 80, 48, 56, 54, 49, 50, 51, 54, 52, 49, 50, 51, 48, 54, 53, 55, 49, 50, 54, 51, 57, 103, 114,
                    49, 104, 50, 106, 51, 103, 114, 116, 107, 49, 104, 50, 51, 107, 103, 102, 114, 116, 49, 104, 106, 50,
                    103, 51, 102, 106, 114, 103, 102, 49, 106, 50, 104, 103, 48, 56, 54, 49, 50, 51, 54, 52, 49, 50, 51,
                    48, 54, 53, 55, 49, 50, 54, 51, 57, 103, 114, 49, 104, 50, 106, 51, 103, 114, 116, 107, 49, 104, 50,
                    51, 107, 103, 102, 114, 116, 49, 104, 106, 50, 103, 51, 102, 106, 114, 103, 102, 49, 106, 50, 104,
                    103, 48, 56, 54, 49, 50, 51, 54, 52, 49, 50, 51, 48, 54, 53, 55, 49, 50, 54, 51, 57, 103, 114, 49,
                    104, 50, 106, 51, 103, 114, 116, 107, 49, 104, 50, 51, 107, 103, 102, 114, 116, 49, 104, 106, 50,
                    103, 51, 102, 106, 114, 103, 102, 49, 106, 50, 104, 103, 48, 56, 54, 49, 50, 51, 54, 52, 49, 50, 51,
                    48, 54, 53, 55, 49, 50, 54, 51, 57, 103, 114, 49, 104, 50, 106, 51, 103, 114, 116, 107, 49, 104, 50,
                    51, 107, 103, 102, 114, 116, 49, 104, 106, 50, 103, 51, 102, 106, 114, 103, 102, 49, 106, 50, 104,
                    103, 48, 56, 54, 49, 50, 51, 54, 52, 49, 50, 51, 48, 54, 53, 55, 49, 50, 54, 51, 57, 103, 114, 49,
                    104, 50, 106, 51, 103, 114, 116, 107, 49, 104, 50, 51, 107, 103, 102, 114, 116, 49, 104, 106, 50,
                    103, 51, 102, 106, 114, 103, 102, 49, 106, 50, 104, 103, 48, 56, 54, 49, 50, 51, 54, 52, 49, 50, 51,
                    48, 54, 53, 55, 49, 50, 54, 51, 57, 103, 114, 49, 104, 50, 106, 51, 103, 114, 116, 107, 49, 104, 50,
                    51, 107, 103, 102, 114, 116, 49, 104, 106, 50, 103, 51, 102, 106, 114, 103, 102, 49, 106, 50, 104,
                    103
                })]
        [InlineData("Мама мыла раму", new byte[] { 186, 208, 156, 208, 176, 208, 188, 208, 176, 32, 208, 188, 209, 139, 208, 187, 208, 176, 32, 209, 128, 208, 176, 208, 188, 209, 131 })]
        [InlineData("Шла Саша по шоссе и сосала сушку", new byte[] { 217, 58, 208, 168, 208, 187, 208, 176, 32, 208, 161, 208, 176, 209, 136, 208, 176, 32, 208, 191, 208, 190, 32, 209, 136, 208, 190, 209, 129, 209, 129, 208, 181, 32, 208, 184, 32, 209, 129, 208, 190, 209, 129, 208, 176, 208, 187, 208, 176, 32, 209, 129, 209, 131, 209, 136, 208, 186, 209, 131 })]
        public void TestStringPack(string s, byte[] data)
        {
            MsgPackSerializer.Deserialize<string>(data.ToMultipleSegments(), out var readSize).ShouldBe(s);
            readSize.ShouldBe(data.Length);
        }
    }
}
