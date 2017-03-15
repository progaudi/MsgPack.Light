namespace ProGaudi.MsgPack.Light.Converters
{
    public abstract class MapTokenConverterBase<TMap, TKey, TValue> : IMsgPackTokenConverter<TMap>
    {
        public void Initialize(MsgPackContext context)
        {
            var keyConverter = context.GetConverter<TKey>();
            var valueConverter = context.GetConverter<TValue>();
            if (keyConverter == null)
            {
                throw ExceptionUtils.NoConverterForCollectionElement(typeof(TKey), "key");
            }

            if (valueConverter == null)
            {
                throw ExceptionUtils.NoConverterForCollectionElement(typeof(TValue), "value");
            }

            KeyTokenConverter = keyConverter;
            ValueTokenConverter = valueConverter;
            Context = context;
        }


        protected MsgPackContext Context { get; private set; }

        protected IMsgPackTokenConverter<TValue> ValueTokenConverter { get; private set; }

        protected IMsgPackTokenConverter<TKey> KeyTokenConverter { get; private set; }

        public abstract MsgPackToken ConvertFrom(TMap value);

        public abstract TMap ConvertTo(MsgPackToken token);
    }
}