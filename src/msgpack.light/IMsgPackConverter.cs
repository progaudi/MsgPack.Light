using JetBrains.Annotations;
using System;

namespace MsgPack.Light
{
    public interface IMsgPackConverter
    {
        void Initialize([NotNull] MsgPackContext context);
    }

    public interface IMsgPackConverter<T> : IMsgPackConverter
    {
        void Write([CanBeNull] T value, [NotNull] IMsgPackWriter writer);

        T Read([NotNull] IMsgPackReader reader);
    }
}