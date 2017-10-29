using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Test.DediLib
{
    public class LinqExtensionsTests
    {
        [Fact]
        public void AsList_InputIsNull_EmptyList()
        {
            Assert.Equal(new List<int>(), ((IEnumerable<int>)null).AsList());
        }

        [Fact]
        public void AsList_InputIsEmptyList_SameReference()
        {
            var list = new List<int>();
            Assert.True(ReferenceEquals(list, list.AsList()));
        }

        [Fact]
        public void AsList_InputIsEnumerable_List()
        {
            var list = new Dictionary<int, string> { { 1, "1" } }.AsList();
            Assert.True(list is List<KeyValuePair<int, string>>);
            Assert.Contains(new KeyValuePair<int, string>(1, "1"), list);
        }

        [Fact]
        public void Split_InputIsNull_EmptyListOfList()
        {
            Assert.Equal(new List<List<int>>(), ((IEnumerable<int>)null).Split(1).ToList());
        }

        [Fact]
        public void Split_EnumerableHasLessItemsThanBatchSize_OneListWithOneListOfAllElements()
        {
            Assert.Equal(new List<List<int>> { new List<int> { 1, 2 } }, new List<int> { 1, 2 }.Split(3).ToList());
        }

        [Fact]
        public void Split_EnumerableHasSameNumberOfItemsAsBatchSize_OneListWithOneListOfAllElements()
        {
            Assert.Equal(new List<List<int>> { new List<int> { 1, 2 } }, new List<int> { 1, 2 }.Split(2).ToList());
        }

        [Fact]
        public void Split_EnumerableHasOneMoreItemThanBatchSize_SplitIntoMultipleLists()
        {
            Assert.Equal(new List<int> { 1, 2 }, new List<int> { 1, 2, 3 }.Split(2).ElementAt(0));
            Assert.Equal(new List<int> { 3 }, new List<int> { 1, 2, 3 }.Split(2).ElementAt(1));
        }
    }
}
