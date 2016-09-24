using System;
using System.Diagnostics;
using DediLib;
using Xunit;
using Xunit.Abstractions;

namespace Test.DediLib
{
    public class TestInterlockedBoolean
    {
        private readonly ITestOutputHelper _output;

        public TestInterlockedBoolean(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void initial_value_false()
        {
            var interlockedBoolean = new InterlockedBoolean();
            Assert.False(interlockedBoolean.Value);
        }

        [Fact]
        public void initial_value_true()
        {
            var interlockedBoolean = new InterlockedBoolean(true);
            Assert.True(interlockedBoolean.Value);
        }

        [Fact]
        public void set_value_true_was_false()
        {
            var interlockedBoolean = new InterlockedBoolean();
            var oldValue = interlockedBoolean.Set(true);

            Assert.True(interlockedBoolean.Value);
            Assert.False(oldValue);
        }

        [Fact]
        public void set_value_true_was_true()
        {
            var interlockedBoolean = new InterlockedBoolean(true);
            var oldValue = interlockedBoolean.Set(true);

            Assert.True(interlockedBoolean.Value);
            Assert.True(oldValue);
        }

        [Fact]
        public void CompareExchange_true_was_false_compare_with_false()
        {
            var interlockedBoolean = new InterlockedBoolean();
            var oldValue = interlockedBoolean.CompareExchange(true, false);

            Assert.True(interlockedBoolean.Value);
            Assert.False(oldValue);
        }

        [Fact]
        public void CompareExchange_true_was_true_compare_with_false()
        {
            var interlockedBoolean = new InterlockedBoolean(true);
            var oldValue = interlockedBoolean.CompareExchange(true, false);

            Assert.True(interlockedBoolean.Value);
            Assert.True(oldValue);
        }

        [Fact]
        public void CompareExchange_false_was_false_compare_with_true()
        {
            var interlockedBoolean = new InterlockedBoolean();
            var oldValue = interlockedBoolean.CompareExchange(false, true);

            Assert.False(interlockedBoolean.Value);
            Assert.False(oldValue);
        }

        [Fact]
        public void CompareExchange_false_was_true_compare_with_true()
        {
            var interlockedBoolean = new InterlockedBoolean(true);
            var oldValue = interlockedBoolean.CompareExchange(false, true);

            Assert.False(interlockedBoolean.Value);
            Assert.True(oldValue);
        }

        [Trait("Category", "Benchmark")]
        [Fact]
        public void benchmark_get_value()
        {
            var interlockedBoolean = new InterlockedBoolean();

            const int iterations = 100000000;
            var value = false;

            var sw = Stopwatch.StartNew();
            for (var i = 0; i < iterations; i++)
            {
                value |= interlockedBoolean.Value;
            }
            sw.Stop();

            if (value) Console.WriteLine(); // prevent too aggressive optimization

            _output.WriteLine("{0} ({1:N0} ops/sec)", sw.Elapsed, iterations / sw.Elapsed.TotalMilliseconds * 1000);
        }

        [Trait("Category", "Benchmark")]
        [Fact]
        public void benchmark_set_value()
        {
            var interlockedBoolean = new InterlockedBoolean();

            const int iterations = 100000000;
            var sw = Stopwatch.StartNew();
            for (var i = 0; i < iterations; i++)
            {
                interlockedBoolean.Set(true);
            }
            sw.Stop();

            _output.WriteLine("{0} ({1:N0} ops/sec)", sw.Elapsed, iterations / sw.Elapsed.TotalMilliseconds * 1000);
        }
    }
}
