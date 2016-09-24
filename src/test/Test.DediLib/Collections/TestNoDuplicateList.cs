using System;
using System.Linq;
using DediLib.Collections;
using Xunit;

// ReSharper disable UseCollectionCountProperty
namespace Test.DediLib.Collections
{
    public class TestNoDuplicateList
    {
        [Fact]
        public void Add_single_item_count_one()
        {
            var list = new NoDuplicateList<string> { "test" };

            Assert.Equal(1, list.Count);
            Assert.Equal(1, list.Count());
        }

        [Fact]
        public void Add_same_item_twice_count_one()
        {
            var list = new NoDuplicateList<string> { "test", "test" };

            Assert.Equal(1, list.Count);
            Assert.Equal(1, list.Count());
        }

        [Fact]
        public void Add_same_item_twice_case_insensitive_count_one()
        {
            var list = new NoDuplicateList<string>(StringComparer.OrdinalIgnoreCase) { "test", "Test" };

            Assert.Equal(1, list.Count);
            Assert.Equal(1, list.Count());
        }

        [Fact]
        public void Add_two_different_items_count_two()
        {
            var list = new NoDuplicateList<string> { "test1", "test2" };

            Assert.Equal(2, list.Count);
            Assert.Equal(2, list.Count());
        }

        [Fact]
        public void Insert_single_item_count_one()
        {
            var list = new NoDuplicateList<string>();
            list.Insert(0, "test");

            Assert.Equal(1, list.Count);
            Assert.Equal(1, list.Count());
        }

        [Fact]
        public void Insert_same_item_twice_count_one()
        {
            var list = new NoDuplicateList<string>();
            list.Insert(0, "test");
            list.Insert(0, "test");

            Assert.Equal(1, list.Count);
            Assert.Equal(1, list.Count());
        }

        [Fact]
        public void Capacity()
        {
            var list = new NoDuplicateList<string>(1234);

            Assert.Equal(1234, list.Capacity);
            Assert.Equal(0, list.Count);
            Assert.Equal(0, list.Count());
        }

        [Fact]
        public void Contains_single_item_exists()
        {
            var list = new NoDuplicateList<string> { "test" };

            Assert.True(list.Contains("test"));
        }

        [Fact]
        public void Contains_single_item_doesnt_exist()
        {
            var list = new NoDuplicateList<string> { "test" };

            Assert.False(list.Contains("other"));
        }

        [Fact]
        public void IndexOf_existing_item()
        {
            var list = new NoDuplicateList<string> { "test" };

            Assert.Equal(0, list.IndexOf("test"));
        }

        [Fact]
        public void IndexOf_non_existing_item()
        {
            var list = new NoDuplicateList<string> { "test" };

            Assert.Equal(-1, list.IndexOf("other"));
        }

        [Fact]
        public void Indexer_get_item()
        {
            var list = new NoDuplicateList<string> { "test" };

            Assert.Equal("test", list[0]);
        }

        [Fact]
        public void Indexer_set_item()
        {
            var list = new NoDuplicateList<string> { "test" };
            list[0] = "new";

            Assert.False(list.Contains("test"));
            Assert.True(list.Contains("new"));
            Assert.Equal("new", list[0]);
            Assert.Equal(1, list.Count);
            Assert.Equal(1, list.Count());
        }

        [Fact]
        public void preserves_order()
        {
            var list = new NoDuplicateList<int>();
            list.AddRange(Enumerable.Range(1, 100));

            var actual = list.ToList();
            var expected = Enumerable.Range(1, 100).ToList();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Clear()
        {
            var list = new NoDuplicateList<string> { "test" };
            list.Clear();

            Assert.Equal(0, list.Count);
            Assert.Equal(0, list.Count());
            Assert.False(list.Contains("test"));
        }

        [Fact]
        public void Remove_non_existing_item()
        {
            var list = new NoDuplicateList<string> { "test" };

            Assert.False(list.Remove("other"));
            Assert.True(list.Contains("test"));
            Assert.Equal(1, list.Count);
            Assert.Equal(1, list.Count());
        }

        [Fact]
        public void Remove_single_item()
        {
            var list = new NoDuplicateList<string> { "test" };

            Assert.True(list.Remove("test"));
            Assert.False(list.Contains("test"));
            Assert.Equal(0, list.Count);
            Assert.Equal(0, list.Count());
        }

        [Fact]
        public void Remove_single_item_at_position()
        {
            var list = new NoDuplicateList<string> { "test" };

            list.RemoveAt(0);
            Assert.False(list.Contains("test"));
            Assert.Equal(0, list.Count);
            Assert.Equal(0, list.Count());
        }
    }
}
