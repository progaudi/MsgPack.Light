using System;

namespace ProGaudi.MsgPack
{
    [Serializable]
    public class UnexpectedCodeException : ConstraintViolationException
    {
        public byte DataCode { get; }

        public UnexpectedCodeException(byte dataCode, Exception innerException = null)
            : base($"We don't expect code '{dataCode}' here.", innerException)
        {
            DataCode = dataCode;
        }
    }
}