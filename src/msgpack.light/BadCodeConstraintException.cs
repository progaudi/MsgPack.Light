using System;
using System.Collections.Generic;

namespace ProGaudi.MsgPack
{
    [Serializable]
    public class BadCodeConstraintException : ConstraintViolationException
    {
        public byte DataCode { get; }

        public HashSet<byte> AllowedCodes { get; }

        public BadCodeConstraintException(byte dataCode, HashSet<byte> allowedCodes, Exception innerException = null)
            : base($"{dataCode} should be one of ({string.Join(", ", allowedCodes)}), but it doesn't.", innerException)
        {
            DataCode = dataCode;
            AllowedCodes = allowedCodes;
        }
    }
}