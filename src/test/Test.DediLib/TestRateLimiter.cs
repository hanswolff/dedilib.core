using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DediLib;
using Xunit;

namespace Test.DediLib
{
    public class TestRateLimiter
    {
        private bool _taskHasRun;
        private readonly Func<Task> _func;

        public TestRateLimiter()
        {
            _taskHasRun = false;
            _func = () =>
            {
                _taskHasRun = true;
                return Task.FromResult(0);
            };
        }

        [Fact]
        public void If_arguments_are_invalid_Then_throw()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new RateLimiter(0, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new RateLimiter(1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new RateLimiter(1, 1, TimeSpan.Zero));
            Assert.Throws<ArgumentException>(() => new RateLimiter(2, 1));
        }

        [Fact]
        public void If_soft_limit_is_not_exhausted_Then_do_not_rate_limit_and_run()
        {
            var rateLimiter = new RateLimiter(1, 1);

            var sw = Stopwatch.StartNew();
            var result = rateLimiter.RateLimit(_func).Result;
            sw.Stop();

            Assert.True(sw.ElapsedMilliseconds < 15);
            Assert.True(result);
            Assert.True(_taskHasRun);
        }

        [Fact]
        public void If_soft_limit_is_exhausted_Then_rate_limit_and_run()
        {
            var rateLimiter = new RateLimiter(1, 2);

            rateLimiter.RateLimit(() => Task.Delay(20));

            var sw = Stopwatch.StartNew();
            var result = rateLimiter.RateLimit(_func).Result;
            sw.Stop();

            Assert.True(sw.ElapsedMilliseconds > 10);
            Assert.True(result);
            Assert.True(_taskHasRun);
        }

        [Fact]
        public void If_hard_limit_is_exhausted_Then_do_not_rate_limit_and_do_not_run()
        {
            var rateLimiter = new RateLimiter(1, 2);

            rateLimiter.RateLimit(() => Task.Delay(20));
            rateLimiter.RateLimit(() => Task.Delay(0));

            var sw = Stopwatch.StartNew();
            var result = rateLimiter.RateLimit(_func).Result;
            sw.Stop();

            Assert.True(sw.ElapsedMilliseconds < 20);
            Assert.False(result);
            Assert.False(_taskHasRun);
        }

        [Fact]
        public void If_hard_limit_is_exceeded_Then_do_not_rate_limit_and_do_not_run()
        {
            var rateLimiter = new RateLimiter(1, 2);

            rateLimiter.RateLimit(() => Task.Delay(100));
            rateLimiter.RateLimit(() => Task.Delay(0));

            var sw = Stopwatch.StartNew();
            Task.WaitAll(Enumerable.Range(0, 100).Select(x => rateLimiter.RateLimit(_func)).ToArray());
            sw.Stop();

            Assert.True(sw.ElapsedMilliseconds < 120);
            Assert.False(_taskHasRun);
        }
    }
}
