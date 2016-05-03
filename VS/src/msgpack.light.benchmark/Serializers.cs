using MsgPack.Light;
using MsgPack.Serialization;

using ServiceStack.Text;

using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace msgpack.light.benchmark
{
    public static class Serializers<T>
    {
        public static readonly JsonSerializer Newtonsoft = new JsonSerializer();
        public static readonly TypeSerializer<T> ServiceStack = new TypeSerializer<T>();
        public static readonly SerializationContext MsgPack = new SerializationContext();
        public static readonly SerializationContext MsgPackHardcore = new SerializationContext();
        public static readonly MsgPackContext MsgPackLight = new MsgPackContext();
        public static readonly MsgPackContext MsgPackLightHardcore = new MsgPackContext();

        static Serializers()
        {
            MsgPackLight.RegisterConverter(new BeerConverter());
            MsgPackLightHardcore.RegisterConverter(new BeerConverterHardCore());
            MsgPack.Serializers.Register(new BeerSerializer(MsgPack));
            MsgPackHardcore.Serializers.Register(new BeerSerializer(MsgPackHardcore));
        }
    }
}