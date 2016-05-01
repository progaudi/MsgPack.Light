using JetBrains.Annotations;
using System;

namespace MsgPack.Light
{
    public interface IMsgPackConverter
    {
    }

    public interface IMsgPackConverter<T> : IMsgPackConverter
    {
        void Write([CanBeNull] T value, [NotNull] IMsgPackWriter writer, [NotNull] MsgPackContext context);

        T Read([NotNull] IMsgPackReader reader, [NotNull] MsgPackContext context, Func<T> creator);
    }
}