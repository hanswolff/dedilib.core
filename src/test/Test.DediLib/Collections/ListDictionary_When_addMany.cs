using DediLib.Collections;
using System.Linq;
using Xunit;

namespace Test.DediLib.Collections
{
    // ReSharper disable InconsistentNaming
    public class ListDictionary_When_addMany
    {
        private readonly ListDictionary<int, int> _sut;

        public ListDictionary_When_addMany()
        {
            _sut = new ListDictionary<int, int>();
        }

        [Fact]
        public void If_addMany_with_key_and_no_values_Then_count_and_values_match()
        {
            _sut.AddMany(1, new int[0]);

            Assert.Equal(0, _sut.Count);
            Assert.False(_sut.ContainsKey(1));
            Assert.Empty(_sut.GetValues(1));
            Assert.Empty(_sut.Keys);
            Assert.Empty(_sut.Values);
        }

        [Fact]
        public void If_addMany_with_same_key_and_same_values_Then_count_and_values_match()
        {
            _sut.AddMany(1, new[] { 1000, 1000 });

            Assert.Equal(2, _sut.Count);
            Assert.True(_sut.ContainsKey(1));
            Assert.Equal(new[] { 1000, 1000 }, _sut.GetValues(1).ToArray());
            Assert.Equal(new[] { 1 }, _sut.Keys.ToArray());
            Assert.Equal(new[] { 1000, 1000 }, _sut.Values.ToArray());
        }

        [Fact]
        public void If_addMany_with_same_key_and_different_values_Then_count_and_values_match()
        {
            _sut.AddMany(1, new[] { 1000, 2000 });

            Assert.Equal(2, _sut.Count);
            Assert.True(_sut.ContainsKey(1));
            Assert.Equal(new[] { 1000, 2000 }, _sut.GetValues(1).ToArray());
            Assert.Equal(new[] { 1 }, _sut.Keys.ToArray());
            Assert.Equal(new[] { 1000, 2000 }, _sut.Values.ToArray());
        }

        [Fact]
        public void If_addMany_twice_with_same_key_and_different_values_Then_count_and_values_match()
        {
            _sut.AddMany(1, new[] { 1000 });
            _sut.AddMany(1, new[] { 2000, 3000 });

            Assert.Equal(3, _sut.Count);
            Assert.True(_sut.ContainsKey(1));
            Assert.Equal(new[] { 1000, 2000, 3000 }, _sut.GetValues(1).ToArray());
            Assert.Equal(new[] { 1 }, _sut.Keys.ToArray());
            Assert.Equal(new[] { 1000, 2000, 3000 }, _sut.Values.ToArray());
        }
    }
}
