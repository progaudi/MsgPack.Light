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
        public static readonly MsgPackContext MsgPackLightMapAutoGeneration = new MsgPackContext();
        public static readonly MsgPackContext MsgPackLightArrayAutoGeneration = new MsgPackContext();

        static Serializers()
        {
            MsgPackLight.RegisterConverter(new BeerConverter());
            MsgPackLightHardcore.RegisterConverter(new BeerConverterHardCore());
            MsgPack.Serializers.Register(new BeerSerializer(MsgPack));
            MsgPackHardcore.Serializers.Register(new BeerSerializer(MsgPackHardcore));
            MsgPackLightMapAutoGeneration.GenerateAndRegisterMapConverter<Beer>();
            MsgPackLightArrayAutoGeneration.GenerateAndRegisterArrayConverter<Beer>();
        }
    }
}