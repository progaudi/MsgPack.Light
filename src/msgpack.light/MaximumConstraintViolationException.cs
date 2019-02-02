using System;

namespace ProGaudi.MsgPack
{
    [Serializable]
    public sealed class MaximumConstraintViolationException : ConstraintViolationException
    {
        public int MaxSize { get; }

        public int ActualLength { get; }
        
        public MaximumConstraintViolationException(int maxSize, int actualLength, Exception innerException = null)
            : base($"Data has length {actualLength}, but expected to be at most {maxSize}.", innerException)
        {
            MaxSize = maxSize;
            ActualLength = actualLength;
        }
    }
}