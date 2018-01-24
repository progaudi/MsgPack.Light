namespace ProGaudi.MsgPack.Light.Converters
{
    public abstract class ArrayConverterBase<TArray, TElement> : IMsgPackConverter<TArray>
    {
        public abstract void Write(TArray value, IMsgPackWriter writer);

        public abstract TArray Read(IMsgPackReader reader);

        public abstract int GuessByteArrayLength(TArray value);

        public bool HasFixedLength => false;

        public void Initialize(MsgPackContext context)
        {
            ElementConverter = context.GetConverter<TElement>() ?? throw ExceptionUtils.NoConverterForCollectionElement(typeof(TElement), "element");
            Context = context;
        }

        protected IMsgPackConverter<TElement> ElementConverter { get; private set; }

        protected MsgPackContext Context { get; private set; }

        protected int GetHeaderLength(int valueCount) => valueCount > ushort.MaxValue
            ? 5
            : valueCount > 15
                ? 3
                : 1;
    }
}