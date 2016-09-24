using DediLib.Collections;
using Xunit;

namespace Test.DediLib.Collections
{
    // ReSharper disable InconsistentNaming
    public class ListDictionary_When_empty
    {
        private readonly ListDictionary<int, int> _sut;

        public ListDictionary_When_empty()
        {
            _sut = new ListDictionary<int, int>();
        }

        [Fact]
        public void Then_count_and_values_match()
        {
            Assert.False(_sut.IsReadOnly);

            Assert.Equal(0, _sut.Count);
            Assert.False(_sut.ContainsKey(1));

            Assert.Empty(_sut.GetValues(1));

            Assert.Empty(_sut.Keys);
            Assert.True(_sut.Keys.IsReadOnly);

            Assert.Empty(_sut.Values);
            Assert.True(_sut.Values.IsReadOnly);
        }
    }
}
