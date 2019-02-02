using System;

namespace ProGaudi.MsgPack
{
    [Serializable]
    public class CodeDoesntBelongFamilyException : ConstraintViolationException
    {
        public byte DataCode { get; }

        public DataFamily Binary { get; }

        public CodeDoesntBelongFamilyException(byte dataCode, DataFamily binary, Exception innerException = null)
            : base($"We expect {binary} data family, but we have {dataCode}, it belongs to {MsgPackSpec.GetDataFamily(dataCode)}.", innerException)
        {
            DataCode = dataCode;
            Binary = binary;
        }
    }
}