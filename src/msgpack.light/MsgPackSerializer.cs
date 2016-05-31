using System.IO;
using System.Runtime.Serialization;

using JetBrains.Annotations;

namespace MsgPack.Light
{
    public static class MsgPackSerializer
    {
        public static byte[] Serialize<T>(T data)
        {
            var context = new MsgPackContext();
            context.Initialize();

            return Serialize(data, context);
        }

        public static byte[] Serialize<T>(T data, [NotNull]MsgPackContext context)
        {
            var memoryStream = new MemoryStream();
            using (var writer = new MsgPackMemoryStreamWriter(memoryStream))
            {
                var converter = GetConverter<T>(context);
                converter.Write(data, writer);
                return memoryStream.ToArray();
            }
        }

        public static void Serialize<T>(T data, MemoryStream stream)
        {
            var context = new MsgPackContext();
            context.Initialize();

            Serialize(data, stream, context);
        }

        public static void Serialize<T>(T data, MemoryStream stream, [NotNull]MsgPackContext context)
        {
            using (var writer = new MsgPackMemoryStreamWriter(stream, false))
            {
                var converter = GetConverter<T>(context);
                converter.Write(data, writer);
            }
        }

        public static T Deserialize<T>(byte[] data)
        {
            var context  = new MsgPackContext();
            context.Initialize();

            return Deserialize<T>(data, context);
        }

        public static T Deserialize<T>(byte[] data, [NotNull]MsgPackContext context)
        {
            var reader = new MsgPackByteArrayReader(data);
            var converter = GetConverter<T>(context);
            return converter.Read(reader);
        }

        public static T Deserialize<T>(MemoryStream stream)
        {
            var context = new MsgPackContext();
            context.Initialize();

            return Deserialize<T>(stream, context);
        }

        public static T Deserialize<T>(MemoryStream stream, [NotNull]MsgPackContext context)
        {
            using (var reader = new MsgPackMemoryStreamReader(stream, false))
            {
                var converter = GetConverter<T>(context);
                return converter.Read(reader);
            }
        }

        [NotNull]
        private static IMsgPackConverter<T> GetConverter<T>(MsgPackContext context)
        {
            var converter = context.GetConverter<T>();

            if (converter == null)
            {
                throw new SerializationException($"Provide converter for {typeof(T).Name}");
            }

            return converter;
        }
    }
}