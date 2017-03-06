namespace ProGaudi.MsgPack.Light.Converters
{
    public abstract class ArrayConverterBase<TArray, TElement> : IMsgPackConverter<TArray>
    {
        public void Initialize(MsgPackContext context)
        {
            var elementConverter = context.GetConverter<TElement>();
            ElementConverter = elementConverter ?? throw ExceptionUtils.NoConverterForCollectionElement(typeof(TElement), "element");
            Context = context;
        }

        protected IMsgPackConverter<TElement> ElementConverter { get; private set; }

        protected MsgPackContext Context { get; private set; }

        public abstract MsgPackToken Write(TArray value);

        public abstract TArray Read(MsgPackToken token);
    }
}