using JetBrains.Annotations;

namespace ProGaudi.MsgPack.Light
{
    public interface IMsgPackConverter
    {
        void Initialize([NotNull] MsgPackContext context);
    }

    public interface IMsgPackConverter<T> : IMsgPackConverter
    {
        MsgPackToken Write([CanBeNull] T value);

        T Read([NotNull] MsgPackToken token);
    }
}