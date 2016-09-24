using System;
using System.Diagnostics;
using DediLib.Collections;
using Xunit;
using Xunit.Abstractions;

namespace Test.DediLib.Collections
{
    // ReSharper disable InconsistentNaming
    [Trait("Category", "Benchmark")]
    public class ListDictionary_When_benchmarking
    {
        private const int Count = 10000000;

        private readonly ITestOutputHelper _output;

        public ListDictionary_When_benchmarking(ITestOutputHelper output)
        {
            _output = output;
        }

        // ReSharper disable CollectionNeverQueried.Local
        [Trait("Category", "Benchmark")]
        [Fact]
        public void Then_adding_multi_int_keys_are_benchmarked()
        {
            var sut = new ListDictionary<int, int>();

            var sw = Stopwatch.StartNew();
            for (var i = 0; i < Count; i++)
            {
                sut.Add(i, i);
            }
            sw.Stop();

            var opsPerSec = Count / (sw.ElapsedMilliseconds + 0.001m) * 1000m;
            _output.WriteLine($"{Count} iterations of Add multiple keys and value, {sw.Elapsed} ({opsPerSec:N0} ops/sec)");
        }

        [Trait("Category", "Benchmark")]
        [Fact]
        public void Then_adding_single_int_keys_are_benchmarked()
        {
            var sut = new ListDictionary<int, int>();

            var sw = Stopwatch.StartNew();
            for (var i = 0; i < Count; i++)
            {
                sut.Add(1, i);
            }
            sw.Stop();

            var opsPerSec = Count / (sw.ElapsedMilliseconds + 0.001m) * 1000m;
            _output.WriteLine($"{Count} iterations of Add multiple keys and value, {sw.Elapsed} ({opsPerSec:N0} ops/sec)");
        }
    }
}
