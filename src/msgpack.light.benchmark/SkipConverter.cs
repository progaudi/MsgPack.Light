using ProGaudi.MsgPack.Light;

namespace ProGaudi.MsgPack.Light.benchmark
{
    public class SkipConverter<T> :IMsgPackConverter<T>
    {
        public void Initialize(MsgPackContext context)
        {

        }

        public void Write(T value, IMsgPackWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public T Read(IMsgPackReader reader)
        {
            reader.SkipToken();
            return default(T);
        }
    }
}