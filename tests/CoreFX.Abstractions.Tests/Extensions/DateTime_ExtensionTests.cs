using System;
using CoreFX.Abstractions.Extensions;
using Xunit;

namespace CoreFX.Abstractions.Tests.Extensions
{
    public class DateTime_ExtensionTests
    {
        [Fact]
        public void FromUnixTime_Zero_Returns1970Jan1()
        {
            var result = 0L.FromUnixTime();
            Assert.Equal(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc), result);
        }

        [Fact]
        public void FromUnixTime_KnownTimestamp_ReturnsExpectedDate()
        {
            // 1609459200 = 2021-01-01T00:00:00Z
            var result = 1609459200L.FromUnixTime();
            Assert.Equal(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc), result);
        }

        [Fact]
        public void ToUnixTime_KnownDate_ReturnsExpectedEpoch()
        {
            var date = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            Assert.Equal(1609459200L, date.ToUnixTime());
        }

        [Fact]
        public void RoundTrip_PreservesDate()
        {
            var original = new DateTime(2023, 6, 15, 12, 30, 0, DateTimeKind.Utc);
            var roundTripped = original.ToUnixTime().FromUnixTime();
            Assert.Equal(original, roundTripped);
        }
    }
}
