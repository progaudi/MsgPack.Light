using System.IO;
using System.Runtime.Serialization;

using JetBrains.Annotations;

namespace ProGaudi.MsgPack.Light
{
    public static class MsgPackSerializer
    {
        public static byte[] Serialize<T>(T data)
        {
            return Serialize(data, new MsgPackContext());
        }

        public static byte[] Serialize<T>(T data, [NotNull]MsgPackContext context)
        {
            var memoryStream = new MemoryStream();
            using (var writer = new MsgPackMemoryStreamWriter(memoryStream))
            {
                var tokenWriter = new TokenWriter(writer);
                var converter = GetConverter<T>(context);
                var token = converter.ConvertFrom(data);
                tokenWriter.Write(token);
                return memoryStream.ToArray();
            }
        }

        public static void Serialize<T>(T data, MemoryStream stream)
        {
            Serialize(data, stream, new MsgPackContext());
        }

        public static void Serialize<T>(T data, MemoryStream stream, [NotNull]MsgPackContext context)
        {
            using (var writer = new MsgPackMemoryStreamWriter(stream, false))
            {
                var tokenWriter = new TokenWriter(writer);
                var converter = GetConverter<T>(context);
                tokenWriter.Write(converter.ConvertFrom(data));
            }
        }

        public static T Deserialize<T>(byte[] data)
        {
            return Deserialize<T>(data, new MsgPackContext());
        }

        public static T Deserialize<T>(byte[] data, [NotNull]MsgPackContext context)
        {
            var reader = new MsgPackByteArrayReader(data);
            return DeserializeInternal<T>(context, reader);
        }

        public static T Deserialize<T>(MemoryStream stream)
        {
            return Deserialize<T>(stream, new MsgPackContext());
        }

        public static T Deserialize<T>(MemoryStream stream, [NotNull]MsgPackContext context)
        {
            using (var reader = new MsgPackMemoryStreamReader(stream, false))
            {
                return DeserializeInternal<T>(context, reader);
            }
        }

        [NotNull]
        private static IMsgPackTokenConverter<T> GetConverter<T>(MsgPackContext context)
        {
            var converter = context.GetConverter<T>();

            if (converter == null)
            {
                throw new SerializationException($"Provide tokenConverter for {typeof(T).Name}");
            }

            return converter;
        }


        private static T DeserializeInternal<T>(MsgPackContext context, IMsgPackReader reader)
        {
            var tokenReader = new TokenReader(reader);
            var converter = GetConverter<T>(context);
            return converter.ConvertTo(tokenReader.Read());
        }

    }
}