using System;
using System.Buffers;

using JetBrains.Annotations;

using ProGaudi.Buffers;

namespace ProGaudi.MsgPack
{
    public static class MsgPackSerializer
    {
        public static IMemoryOwner<byte> Serialize<T>(T data, out int wroteSize)
        {
            return Serialize(data, new MsgPackContext(), out wroteSize);
        }

        public static IMemoryOwner<byte> Serialize<T>(T data, [NotNull]MsgPackContext context, out int wroteSize)
        {
            var formatter = context.GetRequiredFormatter<T>();
            var pool = formatter.HasConstantSize ? FixedLengthMemoryPool<byte>.Shared : MemoryPool<byte>.Shared;
            var memory = pool.Rent(formatter.GetBufferSize(data));
            wroteSize = formatter.Format(memory.Memory.Span, data);
            return memory;
        }

        public static int Serialize<T>(T data, Span<byte> destination)
        {
            return Serialize(data, destination, new MsgPackContext());
        }

        public static int Serialize<T>(T data, Span<byte> destination, [NotNull]MsgPackContext context)
        {
            var converter = context.GetRequiredFormatter<T>();
            return converter.Format(destination, data);
        }

        public static T Deserialize<T>(ReadOnlySpan<byte> data, out int readSize)
        {
            return Deserialize<T>(data, new MsgPackContext(), out readSize);
        }

        public static T Deserialize<T>(ReadOnlySpan<byte> data, [NotNull] MsgPackContext context)
        {
            return Deserialize<T>(data, context, out _);
        }

        public static T Deserialize<T>(ReadOnlySpan<byte> data, [NotNull] MsgPackContext context, out int readSize)
        {
            var converter = context.GetRequiredParser<T>();
            return converter.Parse(data, out readSize);
        }
    }
}
