using System;
using System.Runtime.Serialization;

namespace ProGaudi.MsgPack
{
    [Serializable]
    public abstract class ConstraintViolationException : SerializationException
    {
        protected ConstraintViolationException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}