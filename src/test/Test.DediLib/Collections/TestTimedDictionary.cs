using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using DediLib.Collections;
using Xunit;
using System.Linq;

namespace Test.DediLib.Collections
{
    public class TestTimedDictionary
    {
        [Fact]
        public void Constructor_DefaultExpiry()
        {
            using (var timedDictionary = new TimedDictionary<object, object>(TimeSpan.FromSeconds(123)))
            {
                Assert.Equal(TimeSpan.FromSeconds(123), timedDictionary.DefaultExpiry);
            }
        }

        [Fact]
        public void Constructor_DefaultExpiry_CleanUpPeriod()
        {
            var defaultExpiry = TimeSpan.FromSeconds(123);
            var cleanUpPeriod = TimeSpan.FromSeconds(234);

            using (var timedDictionary = new TimedDictionary<object, object>(defaultExpiry, cleanUpPeriod))
            {
                Assert.Equal(defaultExpiry, timedDictionary.DefaultExpiry);
                Assert.Equal(cleanUpPeriod, timedDictionary.CleanUpPeriod);
            }
        }

        [Fact]
        public void Constructor_DefaultExpiry_ConcurrencyLevel_Capacity()
        {
            using (var timedDictionary = new TimedDictionary<object, object>(TimeSpan.FromSeconds(123), 5, 6))
            {
                Assert.Equal(TimeSpan.FromSeconds(123), timedDictionary.DefaultExpiry);
            }
        }

        [Fact]
        public void AddOrUpdate_value_called_first_creates_value()
        {
            using (var timedDictionary = new TimedDictionary<string, object>())
            {
                var value = timedDictionary.AddOrUpdate("test", "value", (k, v) => "value2");

                Assert.Equal("value", value);
                Assert.Equal("value", timedDictionary["test"]);
            }
        }

        [Fact]
        public void AddOrUpdate_value_called_twice_updates_value()
        {
            using (var timedDictionary = new TimedDictionary<string, object>())
            {
                timedDictionary.AddOrUpdate("test", "value", (k, v) => "value2");
                var value = timedDictionary.AddOrUpdate("test", "value", (k, v) => "value2");

                Assert.Equal("value2", value);
                Assert.Equal("value2", timedDictionary["test"]);
            }
        }

        [Fact]
        public void AddOrUpdate_value_UpdateAccessTime_true()
        {
            using (var timedDictionary = new TimedDictionary<string, object>())
            {
                timedDictionary.AddOrUpdate("test", "value", (k, v) => v);

                TimedValue<object> timedValue;
                Assert.True(timedDictionary.TryGetValue("test", out timedValue));

                var lastAccessed = timedValue.LastAccessUtc;
                Thread.Sleep(15);

                timedDictionary.AddOrUpdate("test", "", (k, v) => v);
                Assert.True(timedValue.LastAccessUtc > lastAccessed);
            }
        }

        [Fact]
        public void AddOrUpdate_value_UpdateAccessTime_false()
        {
            using (var timedDictionary = new TimedDictionary<string, object>())
            {
                timedDictionary.AddOrUpdate("test", "value", (k, v) => v);

                TimedValue<object> timedValue;
                Assert.True(timedDictionary.TryGetValue("test", out timedValue));

                var lastAccessed = timedValue.LastAccessUtc;
                Thread.Sleep(15);

                timedDictionary.AddOrUpdate("test", "", (k, v) => v, false);
                Assert.Equal(timedValue.LastAccessUtc, lastAccessed);
            }
        }

        [Fact]
        public void AddOrUpdate_factory_UpdateAccessTime_true()
        {
            using (var timedDictionary = new TimedDictionary<string, object>())
            {
                timedDictionary.AddOrUpdate("test", k => "value", (k, v) => v);

                TimedValue<object> timedValue;
                Assert.True(timedDictionary.TryGetValue("test", out timedValue));

                var lastAccessed = timedValue.LastAccessUtc;
                Thread.Sleep(15);

                timedDictionary.AddOrUpdate("test", k => "value", (k, v) => v);
                Assert.True(timedValue.LastAccessUtc > lastAccessed);
            }
        }

        [Fact]
        public void AddOrUpdate_factory_UpdateAccessTime_false()
        {
            using (var timedDictionary = new TimedDictionary<string, object>())
            {
                timedDictionary.AddOrUpdate("test", k => "value", (k, v) => v);

                TimedValue<object> timedValue;
                Assert.True(timedDictionary.TryGetValue("test", out timedValue));

                var lastAccessed = timedValue.LastAccessUtc;
                Thread.Sleep(15);

                timedDictionary.AddOrUpdate("test", k => "value", (k, v) => v, false);
                Assert.Equal(timedValue.LastAccessUtc, lastAccessed);
            }
        }

        [Fact]
        public void GetOrAdd_value_called_first_creates_value()
        {
            using (var timedDictionary = new TimedDictionary<string, object>())
            {
                var value = timedDictionary.GetOrAdd("test", "value");

                Assert.Equal("value", value);
                Assert.Equal("value", timedDictionary["test"]);
            }
        }

        [Fact]
        public void GetOrAdd_value_called_twice_creates_value_only_once()
        {
            using (var timedDictionary = new TimedDictionary<string, object>())
            {
                timedDictionary.GetOrAdd("test", "value");
                var value = timedDictionary.GetOrAdd("test", "value2");

                Assert.Equal("value", value);
                Assert.Equal("value", timedDictionary["test"]);
            }
        }

        [Fact]
        public void GetOrAdd_value_UpdateAccessTime_true()
        {
            using (var timedDictionary = new TimedDictionary<string, object>())
            {
                timedDictionary.GetOrAdd("test", "value");

                TimedValue<object> timedValue;
                Assert.True(timedDictionary.TryGetValue("test", out timedValue));

                var lastAccessed = timedValue.LastAccessUtc;
                Thread.Sleep(15);

                timedDictionary.GetOrAdd("test", "");
                Assert.True(timedValue.LastAccessUtc > lastAccessed);
            }
        }

        [Fact]
        public void GetOrAdd_value_UpdateAccessTime_false()
        {
            using (var timedDictionary = new TimedDictionary<string, object>())
            {
                timedDictionary.GetOrAdd("test", "value");

                TimedValue<object> timedValue;
                Assert.True(timedDictionary.TryGetValue("test", out timedValue));

                var lastAccessed = timedValue.LastAccessUtc;
                Thread.Sleep(15);

                timedDictionary.GetOrAdd("test", "", false);
                Assert.Equal(timedValue.LastAccessUtc, lastAccessed);
            }
        }

        [Fact]
        public void GetOrAdd_factory_UpdateAccessTime_true()
        {
            using (var timedDictionary = new TimedDictionary<string, object>())
            {
                timedDictionary.GetOrAdd("test", k => "value");

                TimedValue<object> timedValue;
                Assert.True(timedDictionary.TryGetValue("test", out timedValue));

                var lastAccessed = timedValue.LastAccessUtc;
                Thread.Sleep(15);

                timedDictionary.GetOrAdd("test", k => "value");
                Assert.True(timedValue.LastAccessUtc > lastAccessed);
            }
        }

        [Fact]
        public void GetOrAdd_factory_UpdateAccessTime_false()
        {
            using (var timedDictionary = new TimedDictionary<string, object>())
            {
                timedDictionary.GetOrAdd("test", k => "value");

                TimedValue<object> timedValue;
                Assert.True(timedDictionary.TryGetValue("test", out timedValue));

                var lastAccessed = timedValue.LastAccessUtc;
                Thread.Sleep(15);

                timedDictionary.GetOrAdd("test", k => "value", false);
                Assert.Equal(timedValue.LastAccessUtc, lastAccessed);
            }
        }

        [Fact]
        public void Indexer_TryGetValue()
        {
            using (var timedDictionary = new TimedDictionary<string, object>())
            {
                timedDictionary["test"] = 1;

                object value;
                Assert.True(timedDictionary.TryGetValue("test", out value));
            }
        }

        [Fact]
        public void Indexer_TryRemove_TryGetValue()
        {
            using (var timedDictionary = new TimedDictionary<string, object>())
            {
                timedDictionary["test"] = 1;

                Assert.True(timedDictionary.TryRemove("test"));

                object value;
                Assert.False(timedDictionary.TryGetValue("test", out value));
            }
        }

        [Fact]
        public void Values_enumeration()
        {
            var timedDictionary = new TimedDictionary<string, object> {{"key", "value"}};

            Assert.Equal(new object[] { "value" }, timedDictionary.Values.ToArray());
        }

        [Fact]
        public void TryRemove_not_existing_returns_false()
        {
            using (var timedDictionary = new TimedDictionary<string, object>())
            {
                Assert.False(timedDictionary.TryRemove("test"));
            }
        }

        [Fact]
        public void Indexer_TryGetValue_StringComparer_InvariantCultureIgnoreCase()
        {
            using (var timedDictionary = new TimedDictionary<string, object>(1, 1, StringComparer.OrdinalIgnoreCase))
            {
                timedDictionary["Test"] = 1;

                object value;
                Assert.True(timedDictionary.TryGetValue("test", out value));
                Assert.Equal(1, timedDictionary["test"]);
            }
        }

        [Fact]
        public void DefaultExpiry_Indexer_StringComparer_InvariantCultureIgnoreCase()
        {
            var defaultExpiry = TimeSpan.FromSeconds(123);

            using (var timedDictionary = new TimedDictionary<string, object>(defaultExpiry, 1, 1, StringComparer.OrdinalIgnoreCase))
            {
                timedDictionary["Test"] = 1;

                Assert.Equal(defaultExpiry, timedDictionary.DefaultExpiry);
                Assert.Equal(1, timedDictionary["test"]);
            }
        }

        [Fact]
        public void CleanUp_HasOneExpiredItem_CountIsOne()
        {
            using (var timedDictionary = new TimedDictionary<int, int>(TimeSpan.MaxValue))
            {
                timedDictionary.TryAdd(1, 1);
                timedDictionary.CleanUp();
                Assert.Equal(1, timedDictionary.Count);
            }
        }

        [Fact]
        public void CleanUp_HasOneExpiredItem_CountIsZero()
        {
            using (var timedDictionary = new TimedDictionary<int, int>(TimeSpan.FromMilliseconds(0)))
            {
                timedDictionary.TryAdd(1, 1);
                Thread.Sleep(1);
                timedDictionary.CleanUp();
                Assert.Equal(0, timedDictionary.Count);
            }
        }

        [Fact]
        public void CleanUp_HasOneExpiredItemWaitForAutomaticCleanUp_CountIsZero()
        {
            using (var timedDictionary = new TimedDictionary<int, int>(TimeSpan.FromMilliseconds(0)))
            {
                timedDictionary.TryAdd(1, 1);

                var cancelled = false;
                var task = Task.Factory.StartNew(() =>
                {
                    while (timedDictionary.Count > 0 && !cancelled) Thread.Sleep(0);
                }, TaskCreationOptions.LongRunning);
                {
                    task.Wait(1100);
                    cancelled = true;
                    task.Wait();
                }

                Assert.Equal(0, timedDictionary.Count);
            }
        }

        [Fact]
        public void ConcurrencyTest()
        {
            var exceptions = new List<Exception>();
            // TODO: TimedDictionaryWorker.OnCleanUpException += (td, ex) => exceptions.Add(ex);

            var sw = Stopwatch.StartNew();
            using (var timedDictionary = new TimedDictionary<int, byte>(TimeSpan.Zero))
            {
                var added = 0;
                while (sw.ElapsedMilliseconds < 2000)
                {
                    if (timedDictionary.TryAdd(added, 0))
                        added++;
                }
                Assert.True(timedDictionary.Count < added);
                Assert.Equal(0, exceptions.Count);
            }
        }
    }
}
