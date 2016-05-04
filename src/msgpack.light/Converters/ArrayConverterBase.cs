namespace MsgPack.Light.Converters
{
    internal abstract class ArrayConverterBase<TArray, TElement> : IMsgPackConverter<TArray>
    {
        public abstract void Write(TArray value, IMsgPackWriter writer);

        public abstract TArray Read(IMsgPackReader reader);

        public void Initialize(MsgPackContext context)
        {
            var elementConverter = context.GetConverter<TElement>();
            if (elementConverter == null)
            {
                throw ExceptionUtils.NoConverterForCollectionElement(typeof(TElement), "element");
            }
            ElementConverter = elementConverter;
            Context = context;
        }

        protected IMsgPackConverter<TElement> ElementConverter { get; private set; }

        protected MsgPackContext Context { get; private set; }
    }
}