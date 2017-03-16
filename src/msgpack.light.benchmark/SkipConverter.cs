using ProGaudi.MsgPack.Light;

namespace ProGaudi.MsgPack.Light.benchmark
{
    public class SkipConverter<T> : IMsgPackTokenConverter<T>
    {
        public void Initialize(MsgPackContext context)
        {

        }

        public T ConvertTo(MsgPackToken token)
        {
            return default(T);
        }

        public MsgPackToken ConvertFrom(T value)
        {
            throw new System.NotImplementedException();
        }
    }
}