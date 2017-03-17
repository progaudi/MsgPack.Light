using System;
using System.Text;

using Shouldly;

namespace ProGaudi.MsgPack.Light.Tests.Reader
{
    public class Helpers
    {
        public static string GenerateString(int size)
        {
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            var data = GenerateBytesArray(size);
            var result = new StringBuilder(size);
            foreach (var b in data)
            {
                result.Append(chars[b % chars.Length]);
            }
            return result.ToString();
        }

        public static byte[] GenerateBytesArray(int size)
        {
            var data = new byte[size];
            var crypto = new Random();
            crypto.NextBytes(data);
            return data;
        }

        public static MsgPackToken CheckTokenDeserialization(byte[] data)
        {
            var token = MsgPackSerializer.Deserialize<MsgPackToken>(data);
            token.RawBytes.ShouldBe(data);
            return token;
        }
    }
}
