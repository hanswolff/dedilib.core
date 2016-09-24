using System;
using System.Diagnostics;
using System.Threading;
using DediLib;
using Xunit;
using Xunit.Abstractions;

namespace Test.DediLib
{
    public class TestCounterSignal
    {
        private readonly ITestOutputHelper _output;

        public TestCounterSignal(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void initial_value_greater_equal_one_IsSet_false()
        {
            var counterSignal = new CounterSignal(1);
            Assert.False(counterSignal.IsSet);
        }

        [Fact]
        public void initial_value_greater_equal_one_initial_value_one_IsSet_false()
        {
            var counterSignal = new CounterSignal(1, 1);
            Assert.True(counterSignal.IsSet);
        }

        [Fact]
        public void initial_value_signal_IsSet_true()
        {
            var counterSignal = new CounterSignal(1, 1);
            Assert.True(counterSignal.IsSet);
        }

        [Fact]
        public void initial_value_signal_set_Wait()
        {
            var counterSignal = new CounterSignal(1, 1);
            Assert.True(counterSignal.Wait(TimeSpan.Zero));
        }

        [Fact]
        public void initial_value_signal_set_Wait_TimeSpan()
        {
            var counterSignal = new CounterSignal(1, 1);
            Assert.True(counterSignal.Wait(TimeSpan.Zero));
        }

        [Fact]
        public void initial_value_signal_not_set_IsSet_false()
        {
            var counterSignal = new CounterSignal(2, 1);
            Assert.False(counterSignal.IsSet);
        }

        [Fact]
        public void initial_value_signal_not_set_Wait_CancellationToken()
        {
            var counterSignal = new CounterSignal(2, 1);
            var cancellationToken = new CancellationTokenSource(TimeSpan.FromMilliseconds(15)).Token;
            Assert.Throws<OperationCanceledException>(() => counterSignal.Wait(cancellationToken));
        }

        [Fact]
        public void initial_value_signal_not_set_Wait_TimeSpan_CancellationToken_timed_out()
        {
            var counterSignal = new CounterSignal(2, 1);
            var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(1)).Token;
            Assert.False(counterSignal.Wait(TimeSpan.FromMilliseconds(15), cancellationToken));
        }

        [Fact]
        public void initial_value_signal_not_set_Wait_TimeSpan_CancellationToken_cancelled()
        {
            var counterSignal = new CounterSignal(2, 1);
            var cancellationToken = new CancellationTokenSource(TimeSpan.FromMilliseconds(15)).Token;
            Assert.Throws<OperationCanceledException>(() => counterSignal.Wait(TimeSpan.FromSeconds(1), cancellationToken));
        }

        [Fact]
        public void initial_value_signal_not_set_Wait_TimeSpan_false()
        {
            var counterSignal = new CounterSignal(2, 1);
            Assert.False(counterSignal.Wait(TimeSpan.Zero));
        }

        [Fact]
        public void counter_increment_signal_IsSet_true()
        {
            var counterSignal = new CounterSignal(2, 1);
            counterSignal.Increment();
            Assert.True(counterSignal.IsSet);
        }

        [Fact]
        public void counter_add_signal_IsSet_true()
        {
            var counterSignal = new CounterSignal(10, 1);
            counterSignal.Add(100);
            Assert.True(counterSignal.IsSet);
            Assert.True(counterSignal.Wait(TimeSpan.Zero));
        }

        [Fact]
        public void counter_add_signal_IsSet_false()
        {
            var counterSignal = new CounterSignal(10, 0);
            counterSignal.Add(9);
            Assert.False(counterSignal.IsSet);
        }

        [Fact]
        public void counter_increment_then_decrement_signal_IsSet_false()
        {
            var counterSignal = new CounterSignal(2, 1);
            counterSignal.Increment();
            counterSignal.Decrement();
            Assert.False(counterSignal.IsSet);
        }

        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void CurrentValue_is_initial_value_same_after_ctor(int initialValue)
        {
            var counterSignal = new CounterSignal(0, initialValue);
            Assert.Equal(initialValue, counterSignal.CurrentValue);
        }

        [Fact]
        public void CurrentValue_set_value()
        {
            var counterSignal = new CounterSignal(0) { CurrentValue = 1234 };
            Assert.Equal(1234, counterSignal.CurrentValue);
        }

        [Fact]
        public void CurrentValue_incremented_on_Increment()
        {
            var counterSignal = new CounterSignal(0, 1);
            counterSignal.Increment();
            Assert.Equal(2, counterSignal.CurrentValue);
        }

        [Fact]
        public void CurrentValue_decremented_on_Decrement()
        {
            var counterSignal = new CounterSignal(0, 2);
            counterSignal.Decrement();
            Assert.Equal(1, counterSignal.CurrentValue);
        }

        [Trait("Category", "Benchmark")]
        [Fact]
        public void benchmark_get_value()
        {
            var counterSignal = new CounterSignal(1, 0);

            const int iterations = 10000000;
            var value = false;

            var sw = Stopwatch.StartNew();
            for (var i = 0; i < iterations; i++)
            {
                value |= counterSignal.Wait(TimeSpan.Zero);
            }
            sw.Stop();

            if (value) Console.WriteLine(); // prevent too aggressive optimization

            _output.WriteLine("{0} ({1:N0} ops/sec)", sw.Elapsed, iterations / sw.Elapsed.TotalMilliseconds * 1000);
        }

        [Trait("Category", "Benchmark")]
        [Fact]
        public void benchmark_increment_value()
        {
            var counterSignal = new CounterSignal(1, 0);

            const int iterations = 10000000;
            var sw = Stopwatch.StartNew();
            for (var i = 0; i < iterations; i++)
            {
                counterSignal.Increment();
            }
            sw.Stop();

            _output.WriteLine("{0} ({1:N0} ops/sec)", sw.Elapsed, iterations / sw.Elapsed.TotalMilliseconds * 1000);
        }

        [Trait("Category", "Benchmark")]
        [Fact]
        public void benchmark_add_value()
        {
            var counterSignal = new CounterSignal(1, 0);

            const int iterations = 10000000;
            var sw = Stopwatch.StartNew();
            for (var i = 0; i < iterations; i++)
            {
                counterSignal.Add(1);
            }
            sw.Stop();

            _output.WriteLine("{0} ({1:N0} ops/sec)", sw.Elapsed, iterations / sw.Elapsed.TotalMilliseconds * 1000);
        }
    }
}
