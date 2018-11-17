using System;
using System.Collections.Generic;

using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests.Reader
{
    public class Timespan
    {
        [Fact]
        public void TestTimeSpan()
        {
            var tests = new List<(TimeSpan span, byte[] blob)>
            {
                ValueTuple.Create(TimeSpan.MinValue, new byte[] {211, 128, 0, 0, 0, 0, 0, 0, 0}),
                ValueTuple.Create(TimeSpan.MaxValue, new byte[] {207, 127, 255, 255, 255, 255, 255, 255, 255}),
                ValueTuple.Create(new TimeSpan(1, 2, 3, 4, 5), new byte[] {207, 0, 0, 0, 218, 91, 159, 127, 80}),
                ValueTuple.Create(TimeSpan.FromTicks(-100), new byte[] {208, 156})
            };

            foreach (var test in tests)
            {
                MsgPackSerializer.Deserialize<TimeSpan>(test.blob, out var readSize).ShouldBe(test.span);
                readSize.ShouldBe(test.blob.Length);
            }
        }
    }
}
