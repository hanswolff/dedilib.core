using DediLib.Collections;
using Xunit;

namespace Test.DediLib.Collections
{
    // ReSharper disable InconsistentNaming
    public class HashSetDictionary_When_empty
    {
        private readonly HashSetDictionary<int, int> _sut;

        public HashSetDictionary_When_empty()
        {
            _sut = new HashSetDictionary<int, int>();
        }

        [Fact]
        public void Then_count_and_values_match()
        {
            Assert.False(_sut.IsReadOnly);

            Assert.Equal(0, _sut.Count);
            Assert.False(_sut.ContainsKey(1));

            Assert.Empty(_sut.GetValues(1));
            Assert.Empty(_sut.GetValuesAsHashSet(1));

            Assert.Empty(_sut.Keys);
            Assert.True(_sut.Keys.IsReadOnly);

            Assert.Empty(_sut.Values);
            Assert.True(_sut.Values.IsReadOnly);
        }
    }
}
