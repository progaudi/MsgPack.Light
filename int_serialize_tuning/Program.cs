using System;
using System.Collections.Generic;
using System.Linq;

using ProGaudi.MsgPack.Light;

namespace int_serialize_tuning
{
    class Program
    {
        static void Main(string[] args)
        {
            var r = new Random();
            var a = Enumerable.Range(1, 200000).Select(_ => r.Next()).ToArray();
            var c = new List<byte[]>(2000)
            {
                MsgPackSerializer.Serialize(a)
            };

            for (var i = 0; i < 1000; i++)
            {
                c.Add(MsgPackSerializer.Serialize(a));
            }

            Console.WriteLine(c.Count);
        }
    }
}
