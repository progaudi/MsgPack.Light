using System;

namespace ProGaudi.MsgPack.Light
{
    public class ConverterNotFoundException : Exception
    {
        public ConverterNotFoundException(Type type)
            : base($"Converter not found for type {type.FullName}")
        {
            ObjectType = type;
        }

        public Type ObjectType { get; }
    }
}