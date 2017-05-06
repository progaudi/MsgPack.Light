using System;

namespace ProGaudi.MsgPack.Light.Converters
{
    internal class EnumConverter<T> : IMsgPackConverter<T>
    {
        private readonly Action<IConvertible, IMsgPackWriter> _writeMethod;
        private readonly Func<IMsgPackReader, T> _readMethod;

        public void Initialize(MsgPackContext context)
        {
        }
        
        public EnumConverter(Action<IConvertible, IMsgPackWriter> writeMethod, Func<IMsgPackReader, T> readMethod)
        {
            this._writeMethod = writeMethod;
            this._readMethod = readMethod;
        }

        public void Write(T value, IMsgPackWriter writer)
        {
            _writeMethod(value as IConvertible, writer);
        }

        public T Read(IMsgPackReader reader)
        {
            return _readMethod(reader);
        }
    }
}