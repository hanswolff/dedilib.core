using System;
using System.Diagnostics;
using System.Linq;
using DediLib.Collections;
using Xunit;

namespace Test.DediLib.Collections
{
    public class TestTimeSeriesLookupList
    {
        [Fact]
        public void Constructor_NullCollection_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new TimeSeriesLookupList<DateTime>(null, t => t));
        }

        [Fact]
        public void Constructor_NullTimestampFunc_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new TimeSeriesLookupList<DateTime>(new DateTime[0], null));
        }

        [Fact]
        public void Count()
        {
            var list = new DateTime[1];
            var sut = new TimeSeriesLookupList<DateTime>(list, t => t);
            Assert.Equal(1, sut.Count);
        }

        [Fact]
        public void Indexer_Ordered()
        {
            var value1 = DateTime.UtcNow;
            var value2 = DateTime.UtcNow.AddSeconds(-1);

            var sut = new TimeSeriesLookupList<DateTime>(new[] { value1, value2 }, t => t);
            Assert.Equal(value2, sut[0]);
            Assert.Equal(value1, sut[1]);
        }

        [Fact]
        public void GetEnumerator_Ordered()
        {
            var value1 = DateTime.UtcNow;
            var value2 = DateTime.UtcNow.AddSeconds(-1);

            var sut = new TimeSeriesLookupList<DateTime>(new[] { value1, value2 }, t => t);
            Assert.Equal(new[] { value2, value1 }, sut);
        }

        [Fact]
        public void GetBetween_RangeOutsideLeft_Empty()
        {
            var timestamp = DateTime.UtcNow;
            var values = Enumerable.Range(0, 2500).Select(x => timestamp.AddSeconds(x));

            var sut = new TimeSeriesLookupList<DateTime>(values, t => t);

            var result = sut.GetBetween(timestamp.AddSeconds(-2), timestamp.AddSeconds(-1));
            Assert.Empty(result);
        }

        [Fact]
        public void GetBetween_RangeOutsideRight_Empty()
        {
            var timestamp = DateTime.UtcNow;
            var values = Enumerable.Range(0, 2500).Select(x => timestamp.AddSeconds(-x));

            var sut = new TimeSeriesLookupList<DateTime>(values, t => t);

            var result = sut.GetBetween(timestamp.AddSeconds(1), timestamp.AddSeconds(2));
            Assert.Empty(result);
        }

        [Fact]
        public void GetBetween_RangeFromInclusive_Included()
        {
            var timestamp = DateTime.UtcNow;
            var values = Enumerable.Range(0, 2500).Select(x => timestamp.AddSeconds(-x));

            var sut = new TimeSeriesLookupList<DateTime>(values, t => t);

            var result = sut.GetBetween(timestamp, timestamp.AddMinutes(1));
            Assert.Equal(new[] { timestamp }, result.ToArray());
        }

        [Fact]
        public void GetBetween_RangeToExclusive_Excluded()
        {
            var timestamp = DateTime.UtcNow;
            var values = Enumerable.Range(0, 2500).Select(x => timestamp.AddSeconds(x));

            var sut = new TimeSeriesLookupList<DateTime>(values, t => t);

            var result = sut.GetBetween(timestamp.AddMinutes(-1), timestamp);
            Assert.Empty(result);
        }

        [Fact]
        public void GetBetween_RangeInside_Range()
        {
            var timestamp = DateTime.UtcNow;
            var values = Enumerable.Range(0, 2500).Select(x => timestamp.AddSeconds(x));

            var sut = new TimeSeriesLookupList<DateTime>(values, t => t);

            var result = sut.GetBetween(timestamp.AddMinutes(1), timestamp.AddMinutes(2));
            Assert.Equal(60, result.Count);
        }

        [Fact]
        public void GetBetween_RangeInsideWithDuplicates_IncludeDuplicates()
        {
            var timestamp = DateTime.UtcNow;
            var values = Enumerable.Range(0, 20).Select(x => timestamp.AddMinutes(x)).ToList();
            values.AddRange(Enumerable.Range(0, 20).Select(x => timestamp.AddMinutes(x)));

            var sut = new TimeSeriesLookupList<DateTime>(values.ToList(), t => t);

            var result = sut.GetBetween(timestamp, timestamp.AddSeconds(1));
            Assert.Equal(new[] { timestamp, timestamp }, result.ToArray());
        }

        [Fact]
        public void GetBetween_Dates_Empty()
        {
            var timestamp = DateTime.UtcNow;
            var values = Enumerable.Range(0, 200).Select(x => timestamp.AddDays(x));

            var sut = new TimeSeriesLookupList<DateTime>(values, t => t);

            var result = sut.GetBetween(timestamp.AddDays(1), timestamp.AddDays(2));
            Assert.Equal(1, result.Count);
        }

        [Trait("Category", "Benchmark")]
        [InlineData(1000000, 10, Skip = "benchmark")]
        [InlineData(100000, 100, Skip = "benchmark")]
        [InlineData(10000, 1000, Skip = "benchmark")]
        public void Benchmark(int count, int samplesPerWindow)
        {
            var timestamp = DateTime.UtcNow;
            var values = Enumerable.Range(0, count).Select(x => timestamp.AddSeconds(x));

            var sut = new TimeSeriesLookupList<DateTime>(values, t => t);

            var sw = Stopwatch.StartNew();
            for (var i = 0; i < count; i++)
            {
                var result = sut.GetBetween(timestamp, timestamp.AddSeconds(samplesPerWindow)).Count;
            }
            sw.Stop();

            var opsPerSec = count / (sw.ElapsedMilliseconds + 0.001m) * 1000m;
            Console.WriteLine($"{count} iterations with {samplesPerWindow} samples per window, {sw.Elapsed} ({opsPerSec:N0} ops/sec)");
        }
    }
}
