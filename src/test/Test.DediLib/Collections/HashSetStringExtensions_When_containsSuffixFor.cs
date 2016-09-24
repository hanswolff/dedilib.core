using System.Collections.Generic;
using DediLib.Collections;
using Xunit;

namespace Test.DediLib.Collections
{
    // ReSharper disable InconsistentNaming
    public class HashSetStringExtensions_When_containsSuffixFor
    {
        [Fact]
        public void If_text_is_null_Then_return_false()
        {
            var hashSet = new HashSet<string> { "FirstSection_is_extracted_from_RawUrl.it" };
            Assert.False(hashSet.ContainsSuffixFor(null, '.'));
        }

        [Fact]
        public void If_text_is_blank_Then_return_false()
        {
            var hashSet = new HashSet<string> { "FirstSection_is_extracted_from_RawUrl.it" };
            Assert.False(hashSet.ContainsSuffixFor("", '.'));
        }

        [Fact]
        public void If_text_is_delimiter_Then_return_false()
        {
            var hashSet = new HashSet<string> { "FirstSection_is_extracted_from_RawUrl.it" };
            Assert.False(hashSet.ContainsSuffixFor(".", '.'));
        }

        [Fact]
        public void If_hashSet_empty_and_long_text_Then_return_false()
        {
            var hashSet = new HashSet<string>();
            Assert.False(hashSet.ContainsSuffixFor("FirstSection_is_extracted_from_RawUrl.it", '.'));
        }

        [Fact]
        public void If_hashSet_contains_blank_text_and_text_is_blank_Then_return_false()
        {
            var hashSet = new HashSet<string> { "" };
            Assert.False(hashSet.ContainsSuffixFor("", '.'));
        }

        [Fact]
        public void If_hashSet_contains_text_and_text_is_exactly_the_same_Then_return_true()
        {
            var hashSet = new HashSet<string> { "FirstSection_is_extracted_from_RawUrl.it" };
            Assert.True(hashSet.ContainsSuffixFor("FirstSection_is_extracted_from_RawUrl.it", '.'));
        }

        [Fact]
        public void If_hashSet_contains_text_and_text_is_shorter_than_hashSet_text_Then_return_false()
        {
            var hashSet = new HashSet<string> { "FirstSection_is_extracted_from_RawUrl.it" };
            Assert.False(hashSet.ContainsSuffixFor("it", '.'));
        }

        [Fact]
        public void If_hashSet_contains_text_and_text_is_suffix_for_containing_text_Then_return_true()
        {
            var hashSet = new HashSet<string> { "FirstSection_is_extracted_from_RawUrl.it" };
            Assert.True(hashSet.ContainsSuffixFor("www.FirstSection_is_extracted_from_RawUrl.it", '.'));
        }

        [Fact]
        public void If_delimiter_does_not_match_Then_return_false()
        {
            var hashSet = new HashSet<string> { "wwwtest.it" };
            Assert.False(hashSet.ContainsSuffixFor("bla.wwwtest.it", ','));
        }
    }
}
