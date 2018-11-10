using System;

namespace ProGaudi.MsgPack
{
    [Serializable]
    public class MinimumShouldBeLessThanMaximumException : ConstraintViolationException
    {
        public int MinSize { get; }

        public int MaxSize { get; }

        public MinimumShouldBeLessThanMaximumException(int minSize, int maxSize, Exception innerException = null)
            : base($"Minimum should be less, than maximum, but it isn't. {minSize} >= {maxSize}", innerException)
        {
            MinSize = minSize;
            MaxSize = maxSize;
        }
    }
}