using System;
using DediLib.Collections;
using Xunit;

namespace Test.DediLib
{
    public class DistinctConcurrentQueueTests
    {
        private readonly DistinctConcurrentQueue<string> _queue = new DistinctConcurrentQueue<string>(StringComparer.OrdinalIgnoreCase);

        [Fact]
        public void IsEmpty_Empty_True()
        {
            var isEmpty = _queue.IsEmpty;

            Assert.True(isEmpty);
        }

        [Fact]
        public void IsEmpty_NotEmpty_False()
        {
            _queue.Enqueue("");

            var isEmpty = _queue.IsEmpty;

            Assert.False(isEmpty);
        }

        [Fact]
        public void Enqueue_Null_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => _queue.Enqueue(null));
        }

        [Fact]
        public void Enqueue_Value_True()
        {
            var enqueued = _queue.Enqueue("value");

            Assert.True(enqueued);
        }

        [Fact]
        public void Enqueue_EqualValueTwice_False()
        {
            _queue.Enqueue("Value");

            var enqueued = _queue.Enqueue("value");

            Assert.False(enqueued);
        }

        [Fact]
        public void Dequeue_NotExisting_False()
        {
            var dequeued = _queue.TryDequeue(out var _);

            Assert.False(dequeued);
        }

        [Fact]
        public void Dequeue_Existing_True()
        {
            _queue.Enqueue("value");

            var dequeued = _queue.TryDequeue(out var value);

            Assert.True(dequeued);
            Assert.Equal("value", value);
        }

        [Fact]
        public void EnqueueDequeue_Multiple_Works()
        {
            _queue.Enqueue("value1");
            _queue.Enqueue("Value1");
            _queue.Enqueue("VALUE1");
            _queue.Enqueue("Value2");
            _queue.Enqueue("value2");

            _queue.TryDequeue(out var value);
            Assert.Equal("value1", value);
            _queue.TryDequeue(out value);
            Assert.Equal("Value2", value);
            Assert.True(_queue.IsEmpty);

            _queue.Enqueue("value3");
            _queue.TryDequeue(out value);
            Assert.Equal("value3", value);
            Assert.True(_queue.IsEmpty);
        }
    }
}
