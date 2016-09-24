using DediLib.Collections;
using System.Linq;
using Xunit;

namespace Test.DediLib.Collections
{
    // ReSharper disable InconsistentNaming
    public class ListDictionary_When_add
    {
        private readonly ListDictionary<int, int> _sut;

        public ListDictionary_When_add()
        {
            _sut = new ListDictionary<int, int>();
        }

        [Fact]
        public void If_adding_single_item_Then_expected_count_and_values_match()
        {
            _sut.Add(1, 1000);

            Assert.Equal(1, _sut.Count);
            Assert.True(_sut.ContainsKey(1));
            Assert.Equal(new[] { 1000 }, _sut.GetValues(1).ToArray());
            Assert.Equal(new[] { 1 }, _sut.Keys.ToArray());
            Assert.Equal(new[] { 1000 }, _sut.Values.ToArray());
        }

        [Fact]
        public void If_adding_two_items_with_same_key_and_value_Then_expected_count_and_values_match()
        {
            _sut.Add(1, 1000);
            _sut.Add(1, 1000);

            Assert.Equal(2, _sut.Count);
            Assert.True(_sut.ContainsKey(1));
            Assert.Equal(new[] { 1000, 1000 }, _sut.GetValues(1).ToArray());
            Assert.Equal(new[] { 1 }, _sut.Keys.ToArray());
            Assert.Equal(new[] { 1000, 1000 }, _sut.Values.ToArray());
        }

        [Fact]
        public void If_adding_two_items_with_same_key_Then_expected_count_and_values_match()
        {
            _sut.Add(1, 1000);
            _sut.Add(1, 2000);

            Assert.Equal(2, _sut.Count);
            Assert.True(_sut.ContainsKey(1));
            Assert.Equal(new[] { 1000, 2000 }, _sut.GetValues(1).ToArray());
            Assert.Equal(new[] { 1 }, _sut.Keys.ToArray());
            Assert.Equal(new[] { 1000, 2000 }, _sut.Values.ToArray());
        }

        [Fact]
        public void If_adding_two_items_with_different_keys_and_different_values_Then_expected_count_and_values_match()
        {
            _sut.Add(1, 1000);
            _sut.Add(2, 2000);

            Assert.Equal(2, _sut.Count);
            Assert.True(_sut.ContainsKey(1));
            Assert.True(_sut.ContainsKey(2));
            Assert.Equal(new[] { 1000 }, _sut.GetValues(1).ToArray());
            Assert.Equal(new[] { 2000 }, _sut.GetValues(2).ToArray());
            Assert.Equal(new[] { 1, 2 }, _sut.Keys.ToArray());
            Assert.Equal(new[] { 1000, 2000 }, _sut.Values.ToArray());
        }
    }
}
