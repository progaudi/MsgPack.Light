using System.Collections.Generic;

using MessagePack;

namespace ProGaudi.MsgPack.Light.Benchmark.Data
{
    [MessagePackObject(true)]
    public class Beer
    {
        [MsgPackMapElement(nameof(Brand))]
        [MsgPackArrayElement(0)]
        public string Brand { get; set; }

        [MsgPackMapElement(nameof(Sort))]
        [MsgPackArrayElement(1)]
        public List<string> Sort { get; set; }

        [MsgPackMapElement(nameof(Alcohol))]
        [MsgPackArrayElement(2)]
        public float Alcohol { get; set; }

        [MsgPackMapElement(nameof(Brewery))]
        [MsgPackArrayElement(3)]
        public string Brewery { get; set; }
    }
}