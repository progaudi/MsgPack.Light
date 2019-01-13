using System;
using System.Buffers;
using System.Diagnostics;

namespace ProGaudi.MsgPack.Light.Tests.SequenceReader
{
    [DebuggerStepThrough]
    public static class Extensions
    {
        public static ReadOnlySequence<T> ToMultipleSegments<T>(this T[] buffer)
        {
            return buffer.AsMemory().ToMultipleSegments();
        }

        public static ReadOnlySequence<T> ToMultipleSegments<T>(this Memory<T> memory)
        {
            return ((ReadOnlyMemory<T>) memory).ToMultipleSegments();
        }

        public static ReadOnlySequence<T> ToMultipleSegments<T>(this ReadOnlyMemory<T> memory)
        {
            var length = memory.Length;
            if (length <= 1) return ZeroOrOneElementSequence();

            var last = new Segment<T>(memory.Slice(length - 1), null, length - 1);
            var lastEmpty = new Segment<T>(ReadOnlyMemory<T>.Empty, last, length - 1);
            var medium = new Segment<T>(memory.Slice(1, length - 2), lastEmpty, 1);
            var firstEmpty = new Segment<T>(ReadOnlyMemory<T>.Empty, medium, 1);
            var first = new Segment<T>(memory.Slice(0, 1), firstEmpty, 0);
            return new ReadOnlySequence<T>(first, 0, last, last.Memory.Length);

            ReadOnlySequence<T> ZeroOrOneElementSequence()
            {
                var endSegment = new Segment<T>(ReadOnlyMemory<T>.Empty, null, length);
                var element = new Segment<T>(memory, endSegment, 0);
                var startSegment = new Segment<T>(ReadOnlyMemory<T>.Empty, element, 0);

                return new ReadOnlySequence<T>(startSegment, 0, endSegment, 0);
            }
        }

        private sealed class Segment<T> : ReadOnlySequenceSegment<T>
        {
            public Segment(ReadOnlyMemory<T> memory, ReadOnlySequenceSegment<T> next, long runningIndex)
            {
                Memory = memory;
                Next = next;
                RunningIndex = runningIndex;
            }
        }
    }
}
