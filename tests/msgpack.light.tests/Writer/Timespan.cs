using System;
using System.Collections.Generic;

using Shouldly;

using Xunit;

namespace ProGaudi.MsgPack.Light.Tests.Writer
{
    public class Timespan
    {
        [Fact]
        public void TestTimeSpan()
        {
            var tests = new List<KeyValuePair<System.TimeSpan, byte[]>>()
            {
                new KeyValuePair<TimeSpan, byte[]>(TimeSpan.MinValue, new byte[] {211, 128, 0, 0, 0, 0, 0, 0, 0,}),
                new KeyValuePair<TimeSpan, byte[]>(TimeSpan.MaxValue, new byte[] {207, 127, 255, 255, 255, 255, 255, 255, 255}),
                new KeyValuePair<TimeSpan, byte[]>(new TimeSpan(1, 2, 3, 4, 5), new byte[] {207, 0, 0, 0, 218, 91, 159, 127, 80}),
                new KeyValuePair<TimeSpan, byte[]>(TimeSpan.FromTicks(-100), new byte[] {208, 156}),
            };

            foreach (var test in tests)
            {
                MsgPackSerializer.Serialize(test.Key).ShouldBe(test.Value);
                ((MsgPackToken)test.Key).RawBytes.ShouldBe(test.Value);
            }
        }
    }
}