using System.Collections.Generic;

namespace ProGaudi.MsgPack.Light.Benchmark.Data
{
    public class Beer
    {
        [MsgPackMapElement(nameof(Brand))]
        public string Brand { get; set; }

        [MsgPackMapElement(nameof(Sort))]
        public List<string> Sort { get; set; }

        [MsgPackMapElement(nameof(Alcohol))]
        public float Alcohol { get; set; }

        [MsgPackMapElement(nameof(Brewery))]
        public string Brewery { get; set; }
    }
}