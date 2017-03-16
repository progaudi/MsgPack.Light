namespace ProGaudi.MsgPack.Light.Converters
{
    public abstract class ArrayTokenConverterBase<TArray, TElement> : IMsgPackTokenConverter<TArray>
    {
        public void Initialize(MsgPackContext context)
        {
            var elementConverter = context.GetConverter<TElement>();
            ElementTokenConverter = elementConverter ?? throw ExceptionUtils.NoConverterForCollectionElement(typeof(TElement), "element");
            Context = context;
        }

        protected IMsgPackTokenConverter<TElement> ElementTokenConverter { get; private set; }

        protected MsgPackContext Context { get; private set; }

        public abstract MsgPackToken ConvertFrom(TArray value);

        public abstract TArray ConvertTo(MsgPackToken token);
    }
}