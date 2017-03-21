using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DediLib.Collections;
using Xunit;

namespace Test.DediLib.Collections
{
    public class TestBucketQueue
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Constructor_InvalidBucketSize_Throws(int bucketSize)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new BucketQueue<object>(bucketSize));
        }

        [Fact]
        public void Dequeue_NonExistingItem_Works()
        {
            // Arrange
            var bucket = new BucketQueue<string>(1);

            // Act
            var result = bucket.TryDequeue(out string value);

            // Assert
            Assert.False(result);
            Assert.Null(value);
        }

        [Fact]
        public void Dequeue_SingleItem_Works()
        {
            // Arrange
            var bucket = new BucketQueue<string>(1);
            bucket.Enqueue(0, "test");

            // Act
            var result = bucket.TryDequeue(out string value);

            // Assert
            Assert.True(result);
            Assert.Equal("test", value);
        }

        [Fact]
        public void Dequeue_MultipleItems_Works()
        {
            // Arrange
            var bucket = new BucketQueue<string>(2);
            bucket.Enqueue(0, "0");
            bucket.Enqueue(1, "1");
            bucket.Enqueue(2, "2");

            // Act / Assert
            string value;
            Assert.True(bucket.TryDequeue(out value));
            Assert.Equal("1", value);
            Assert.True(bucket.TryDequeue(out value));
            Assert.Equal("0", value);
            Assert.True(bucket.TryDequeue(out value));
            Assert.Equal("2", value);
            Assert.False(bucket.TryDequeue(out value));
        }

        [Fact]
        public void Dequeue_Concurrently_Works()
        {
            // Arrange
            var bucket = new BucketQueue<string>(2);
            var count = 10000;
            var range = Enumerable.Range(0, count).ToList();
            range.ForEach(i => bucket.Enqueue(i, i.ToString()));

            var dequeuedItems = new ConcurrentBag<string>();

            // Act
            Enumerable.Range(0, 2 * count).AsParallel().ForAll(i =>
            {
                if (bucket.TryDequeue(out string value))
                {
                    dequeuedItems.Add(value);
                }
            });

            // Assert
            var rangeAsString = range.Select(x => x.ToString()).ToList();
            Assert.True(new HashSet<string>(dequeuedItems).SetEquals(rangeAsString));
        }

        [Trait("Category", "Benchmark")]
        [Theory(Skip = "Benchmark")]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(10000)]
        public void Benchmark(int buckets)
        {
            var count = 2000000;
            var bucket = new BucketQueue<string>(buckets);
            var range = Enumerable.Range(0, count).ToList();
            range.ForEach(i => bucket.Enqueue(i, i.ToString()));

            var sw = Stopwatch.StartNew();
            for (var i = 0; i < count; i++)
            {
                string value;
                bucket.TryDequeue(out value);
            };
            sw.Stop();

            Console.WriteLine("{0} ops/sec", count / sw.Elapsed.Add(TimeSpan.FromTicks(1)).TotalSeconds);
        }
    }
}