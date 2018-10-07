using System;

namespace ProGaudi.MsgPack
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct, Inherited = false)]
    public class MsgPackArrayAttribute : Attribute
    {
    }
}
