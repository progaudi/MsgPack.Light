using JetBrains.Annotations;

namespace ProGaudi.MsgPack.Light
{
    public interface IMsgPackTokenConverter
    {
        void Initialize([NotNull] MsgPackContext context);
    }

    public interface IMsgPackTokenConverter<T> : IMsgPackTokenConverter
    {
        MsgPackToken ConvertFrom([CanBeNull] T value);

        T ConvertTo([NotNull] MsgPackToken token);
    }
}