using System;

namespace ProGaudi.MsgPack.Light
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MsgPackMapElementAttribute : Attribute
    {
        public MsgPackMapElementAttribute(string name) => Name = name;

        public string Name { get; }
    }
}