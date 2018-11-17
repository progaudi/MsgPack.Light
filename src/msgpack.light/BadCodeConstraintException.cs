using System;
using System.Collections.Generic;
using System.Linq;

namespace ProGaudi.MsgPack
{
    [Serializable]
    public class BadCodeConstraintException : ConstraintViolationException
    {
        public byte DataCode { get; }

        public HashSet<byte> AllowedCodes { get; }

        public BadCodeConstraintException(byte dataCode, HashSet<byte> allowedCodes, Exception innerException = null)
            : base($"{dataCode:x2} should be one of ({string.Join(", ", allowedCodes.Select(x => $"{x:x2}"))}), but it doesn't.", innerException)
        {
            DataCode = dataCode;
            AllowedCodes = allowedCodes;
        }
    }
}
