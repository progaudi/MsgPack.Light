using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;

using JetBrains.Annotations;

namespace MsgPack.Light
{
    //[DebuggerStepThrough]
    public static class MsgPackSerializer
    {
        public static byte[] Serialize<T>(T data)
        {
            return Serialize(data, new MsgPackContext());
        }

        public static byte[] Serialize<T>(T data, [NotNull]MsgPackContext context)
        {
            var memoryStream = new MemoryStream();
            using (var writer = new MsgPackStreamWriter(memoryStream))
            {
                var converter = GetConverter<T>(context);
                converter.Write(data, writer);
                return memoryStream.ToArray();
            }
        }

        public static void Serialize<T>(T data, Stream stream)
        {
            Serialize(data, stream, new MsgPackContext());
        }

        public static void Serialize<T>(T data, Stream stream, [NotNull]MsgPackContext context)
        {
            using (var writer = new MsgPackStreamWriter(stream, false))
            {
                var converter = GetConverter<T>(context);
                converter.Write(data, writer);
            }
        }

        public static T Deserialize<T>(byte[] data)
        {
            return Deserialize<T>(data, new MsgPackContext());
        }

        public static T Deserialize<T>(byte[] data, [NotNull]MsgPackContext context)
        {
            var reader = new MsgPackByteArrayReader(data);
            var converter = GetConverter<T>(context);
            return converter.Read(reader);
        }

        public static T Deserialize<T>(Stream stream)
        {
            return Deserialize<T>(stream, new MsgPackContext());
        }

        public static T Deserialize<T>(Stream stream, [NotNull]MsgPackContext context)
        {
            using (var reader = new MsgPackStreamReader(stream, false))
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