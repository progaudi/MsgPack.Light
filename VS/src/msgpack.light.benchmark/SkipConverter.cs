using MsgPack.Light;

namespace msgpack.light.benchmark
{
    public class SkipConverter<T> :IMsgPackConverter<T>
    {
        private readonly int _objectsCount;

        public SkipConverter(int objectsCount = 1)
        {
            _objectsCount = objectsCount;
        }

        public void Initialize(MsgPackContext context)
        {
            
        }

        public void Write(T value, IMsgPackWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public T Read(IMsgPackReader reader)
        {
            for (var i = 0; i < _objectsCount; i++)
            {
                reader.SkipToken();
            }

            return default(T);
        }
    }
}