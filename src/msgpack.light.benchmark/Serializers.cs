using System.IO;

using MsgPack.Serialization;

using ProGaudi.MsgPack.Light.Benchmark.Data;

using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace ProGaudi.MsgPack.Light.Benchmark
{
    public static class Serializers
    {
        public static readonly JsonSerializer Newtonsoft = new JsonSerializer();
        public static readonly SerializationContext MsgPack = new SerializationContext();
        public static readonly SerializationContext MsgPackHardcore = new SerializationContext();
        public static readonly MsgPackContext MsgPackLight = new MsgPackContext();
        public static readonly MsgPackContext MsgPackLightHardcore = new MsgPackContext();

        static Serializers()
        {
            MsgPackLight.RegisterFormatter(x => new BeerConverter(x));
            MsgPackLight.RegisterParser(x => new BeerConverter(x));

            MsgPackLightHardcore.RegisterFormatter(x => new BeerConverterHardCore(x));
            MsgPackLightHardcore.RegisterParser(x => new BeerConverterHardCore(x));

            MsgPack.Serializers.Register(new BeerSerializer(MsgPack));
            MsgPackHardcore.Serializers.Register(new BeerSerializer(MsgPackHardcore));
        }

        public static MemoryStream CreateStream() => new MemoryStream(1_000_000);
    }
}
