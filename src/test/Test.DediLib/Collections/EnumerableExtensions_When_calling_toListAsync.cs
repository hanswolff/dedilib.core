using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DediLib.Collections;
using Xunit;

namespace Test.DediLib.Collections
{
    // ReSharper disable InconsistentNaming
    public class EnumerableExtensions_When_calling_toListAsync
    {
        private const int NumberOfTasksToRun = 1000;

        [Fact]
        public async Task Then_selector_is_called_for_each_item()
        {
            // Arrange
            var selectorCalledCount = 0;

            // Act
            await Enumerable
                .Range(0, NumberOfTasksToRun)
                .ToListAsync(
                    i =>
                    {
                        Interlocked.Increment(ref selectorCalledCount);
                        return Task.FromResult(true);
                    })
                .ConfigureAwait(false);

            // Assert
            Assert.Equal(NumberOfTasksToRun, selectorCalledCount);
        }

        [Fact]
        public async Task Then_selector_is_called_asynchronously_for_all_items()
        {
            // Arrange
            const int taskDelayTime = 200;
            var sw = Stopwatch.StartNew();

            // Act
            await Enumerable
                .Range(0, NumberOfTasksToRun)
                .ToListAsync(
                    async x =>
                    {
                        await Task.Delay(taskDelayTime).ConfigureAwait(false);
                        return true;
                    })
                .ConfigureAwait(false);

            // Assert
            sw.Stop();
            Assert.True(sw.ElapsedMilliseconds < 2 * taskDelayTime);
        }

        [Fact]
        public async Task If_some_items_are_filtered_Then_filtered_items_are_returned()
        {
            // Act
            var list = await Enumerable
                .Range(0, NumberOfTasksToRun)
                .ToListAsync(i => Task.FromResult(i % 2 != 0))
                .ConfigureAwait(false);

            // Assert
            Assert.Equal(NumberOfTasksToRun / 2, list.Count);
            Assert.True(list.All(i => i % 2 != 0));
        }

        [Fact]
        public void If_selector_is_null_Then_exception_is_thrown()
        {
            // Act
            Func<Task<List<int>>> act = () => Enumerable
                .Range(0, NumberOfTasksToRun)
                .ToListAsync(null);

            // Assert
            Assert.Throws<AggregateException>(() => act().Wait());
        }

        [Fact]
        public void If_source_is_null_Then_exception_is_thrown()
        {
            // Act
            Func<Task<List<object>>> act = () => ((List<object>)null).ToListAsync(x => Task.FromResult(true));

            // Assert
            Assert.Throws<AggregateException>(() => act().Wait());
        }

        [Fact]
        public void If_task_throws_exception_Then_aggregateException_is_thrown()
        {
            // Act
            Func<Task<List<int>>> act = () => Enumerable
                .Range(0, NumberOfTasksToRun)
                .ToListAsync(
                    i =>
                    {
                        if (i == NumberOfTasksToRun / 2)
                            throw new InvalidOperationException();
                        return Task.FromResult(true);
                    });

            // Assert
            Assert.Throws<AggregateException>(() => act().Wait());
        }
    }
}