using System;
using System.Collections.Generic;
using System.Linq;
using DediLib.Collections;
using Xunit;

namespace Test.DediLib.Collections
{
    // ReSharper disable InconsistentNaming
    public class HashSetDictionary_When_remove
    {
        private readonly HashSetDictionary<string, string> _sut;

        private readonly ICollection<string> _keys;

        private readonly ICollection<string> _values;

        public HashSetDictionary_When_remove()
        {
            _sut = new HashSetDictionary<string, string>(StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase);
            _sut.AddMany("key1", new[] { "value" });
            _sut.AddMany("key2", new[] { "value1", "value2" });

            _keys = _sut.Keys.ToArray();
            _values = _sut.Values.ToArray();
        }

        [Fact]
        public void If_remove_non_existing_key_Then_collection_is_unchanged()
        {
            var result = _sut.Remove("non-existing-key");

            AssertCollectionUnchanged(result);
        }

        [Fact]
        public void If_remove_single_key_Then_item_is_removed()
        {
            var result = _sut.Remove("Key1");

            Assert.True(result);
            Assert.False(_sut.ContainsKey("key1"));
        }

        [Fact]
        public void If_remove_single_key_with_existing_value_Then_item_is_removed()
        {
            var result = _sut.Remove("Key1", "Value");

            Assert.True(result);
            Assert.False(_sut.ContainsKey("key1"));
        }

        [Fact]
        public void If_remove_single_key_with_non_existing_value_Then_item_is_not_removed()
        {
            var result = _sut.Remove("Key1", "Value2");

            AssertCollectionUnchanged(result);
        }

        [Fact]
        public void If_remove_multi_key_Then_item_is_removed()
        {
            var result = _sut.Remove("Key2");

            Assert.True(result);
            Assert.False(_sut.ContainsKey("key2"));
        }

        [Fact]
        public void If_remove_multi_key_with_existing_value_Then_item_is_removed()
        {
            var result = _sut.Remove("Key2", "Value1");

            Assert.True(result);
            Assert.True(_sut.ContainsKey("key2"));
            Assert.Equal(new[] { "value2" }, _sut.GetValues("key2"));
        }

        [Fact]
        public void If_remove_multi_key_with_non_existing_value_Then_item_is_not_removed()
        {
            var result = _sut.Remove("Key2", "non-existing-value");

            AssertCollectionUnchanged(result);
        }

        private void AssertCollectionUnchanged(bool result)
        {
            Assert.False(result);
            Assert.Equal(_keys, _sut.Keys.ToArray());
            Assert.Equal(_values, _sut.Values.ToArray());
        }
    }
}
