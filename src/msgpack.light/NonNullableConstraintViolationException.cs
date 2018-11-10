using System;

namespace ProGaudi.MsgPack
{
    [Serializable]
    public sealed class NonNullableConstraintViolationException : ConstraintViolationException
    {
        public NonNullableConstraintViolationException(Exception innerException = null)
            : base("Data is null, but expected to be not null.", innerException)
        {
        }
    }
}