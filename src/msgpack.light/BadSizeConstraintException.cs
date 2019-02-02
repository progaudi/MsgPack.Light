using System;

namespace ProGaudi.MsgPack
{
    [Serializable]
    public class BadSizeConstraintException : ConstraintViolationException
    {
        public DataFamily DataFamily { get; }

        public byte DataCode { get; }

        public int RequestedSize { get; }

        public BadSizeConstraintException(DataFamily dataFamily, byte dataCode, int requestedSize, Exception innerException = null)
            : base(M(dataFamily, dataCode, requestedSize), innerException)
        {
            DataFamily = dataFamily;
            DataCode = dataCode;
            RequestedSize = requestedSize;
        }

        private static string M(DataFamily dataFamily, byte dataCode, int requestedSize)
        {
            var (min, max) = DataLengths.GetMinAndMaxLength(dataCode);
            return $"Length of {dataCode} of {dataFamily} should belong to [{min}, {max}]. {requestedSize} doesn't";
        }
    }
}