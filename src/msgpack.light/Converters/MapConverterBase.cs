namespace ProGaudi.MsgPack.Light.Converters
{
    public abstract class MapConverterBase<TMap, TKey, TValue> : IMsgPackConverter<TMap>
    {
        public void Initialize(MsgPackContext context)
        {
            var keyConverter = context.GetConverter<TKey>();
            var valueConverter = context.GetConverter<TValue>();

            KeyConverter = keyConverter ?? throw ExceptionUtils.NoConverterForCollectionElement(typeof(TKey), "key");
            ValueConverter = valueConverter ?? throw ExceptionUtils.NoConverterForCollectionElement(typeof(TValue), "value");
            Context = context;
        }

        public abstract void Write(TMap value, IMsgPackWriter writer);

        public abstract TMap Read(IMsgPackReader reader);

        public abstract int GuessByteArrayLength(TMap value);

        public bool HasFixedLength => ValueConverter.HasFixedLength && KeyConverter.HasFixedLength;

        protected MsgPackContext Context { get; private set; }

        protected IMsgPackConverter<TValue> ValueConverter { get; private set; }

        protected IMsgPackConverter<TKey> KeyConverter { get; private set; }

        protected int GetHeaderLength(int valueCount) => valueCount > ushort.MaxValue
            ? 5
            : valueCount > 15
                ? 3
                : 1;
    }
}