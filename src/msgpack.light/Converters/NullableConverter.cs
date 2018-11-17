using System;

namespace ProGaudi.MsgPack.Converters
{
    public class NullableConverter<T> : IMsgPackFormatter<T?>, IMsgPackParser<T?>
        where T : struct
    {
        private readonly IMsgPackFormatter<T> _formatter;

        private readonly IMsgPackParser<T> _parser;

        public NullableConverter(MsgPackContext context)
        {
            _formatter = context.GetRequiredFormatter<T>();
            _parser = context.GetRequiredParser<T>();
        }

        public int GetBufferSize(T? value) => value.HasValue ? _formatter.GetBufferSize(value.Value) : DataLengths.Nil;

        public bool HasConstantSize => _formatter.HasConstantSize && _formatter.GetBufferSize(default) == DataLengths.Nil;

        public int Format(Span<byte> destination, T? value) => value.HasValue
            ? _formatter.Format(destination, value.Value)
            : MsgPackSpec.WriteNil(destination);

        public T? Parse(ReadOnlySpan<byte> source, out int readSize) => MsgPackSpec.TryReadNil(source, out readSize)
            ? default
            : _parser.Parse(source, out readSize);
    }
}
