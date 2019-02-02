using System.Runtime.Serialization;

using JetBrains.Annotations;

namespace ProGaudi.MsgPack
{
    public static class Extensions
    {
        [NotNull]
        public static IMsgPackFormatter<T> GetRequiredFormatter<T>(this MsgPackContext context)
        {
            var formatter = context.GetFormatter<T>();

            if (formatter == null)
            {
                throw new SerializationException($"Provide formatter for {typeof(T).Name}");
            }

            return formatter;
        }

        [NotNull]
        public static IMsgPackParser<T> GetRequiredParser<T>(this MsgPackContext context)
        {
            var parser = context.GetParser<T>();

            if (parser == null)
            {
                throw new SerializationException($"Provide parser for {typeof(T).Name}");
            }

            return parser;
        }

        [NotNull]
        public static IMsgPackSequenceParser<T> GetRequiredSequenceParser<T>(this MsgPackContext context)
        {
            var parser = context.GetSequenceParser<T>();

            if (parser == null)
            {
                throw new SerializationException($"Provide sequence parser for {typeof(T).Name}");
            }

            return parser;
        }
    }
}
