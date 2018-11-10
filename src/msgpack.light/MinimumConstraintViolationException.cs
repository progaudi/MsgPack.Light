using System;

namespace ProGaudi.MsgPack
{
    [Serializable]
    public sealed class MinimumConstraintViolationException : ConstraintViolationException
    {
        public int MinSize { get; }

        public int ActualLength { get; }

        public MinimumConstraintViolationException(int minSize, int actualLength, Exception innerException = null)
            : base($"Data has length {actualLength}, but expected to be at least {minSize}.", innerException)
        {
            MinSize = minSize;
            ActualLength = actualLength;
        }
    }
}