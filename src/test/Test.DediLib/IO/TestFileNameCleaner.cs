using DediLib.IO;
using Xunit;

namespace Test.DediLib.IO
{
    public class TestFileNameCleaner
    {
        [Fact]
        public void ReplaceInvalidPathChars()
        {
            var fileNameCleaner = new FileNameCleaner();
            Assert.Equal("abc_", fileNameCleaner.ReplaceInvalidPathChars("abc|", "_"));
        }

        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("abc", "abc")]
        [InlineData("abc|", "abc")]
        [InlineData("abc>", "abc")]
        [InlineData("abc?", "abc?")]
        [InlineData("abc*", "abc*")]
        [InlineData(@"C:\Temp", @"C:\Temp")]
        public void Remove_InvalidPathChars(string fileName, string expectedOutput)
        {
            var fileNameCleaner = new FileNameCleaner();
            Assert.Equal(expectedOutput, fileNameCleaner.ReplaceInvalidPathChars(fileName));
        }

        [Fact]
        public void ReplaceInvalidFileChars()
        {
            var fileNameCleaner = new FileNameCleaner();
            Assert.Equal("abc_", fileNameCleaner.ReplaceInvalidFileChars("abc*", "_"));
        }

        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("abc", "abc")]
        [InlineData("abc|", "abc")]
        [InlineData("abc>", "abc")]
        [InlineData("abc?", "abc")]
        [InlineData("abc*", "abc")]
        [InlineData(@"C:\Temp", "CTemp")]
        public void Remove_InvalidFileChars(string fileName, string expectedOutput)
        {
            var fileNameCleaner = new FileNameCleaner();
            Assert.Equal(expectedOutput, fileNameCleaner.ReplaceInvalidFileChars(fileName));
        }

        [Fact]
        public void ReplaceAllInvalidChars()
        {
            var fileNameCleaner = new FileNameCleaner();
            Assert.Equal("abc_", fileNameCleaner.ReplaceAllInvalidChars("abc*", "_"));
        }

        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("abc", "abc")]
        [InlineData("abc|", "abc")]
        [InlineData("abc>", "abc")]
        [InlineData("abc?", "abc")]
        [InlineData("abc*", "abc")]
        [InlineData(@"C:\Temp", "CTemp")]
        public void Remove_AllInvalidChars(string fileName, string expectedOutput)
        {
            var fileNameCleaner = new FileNameCleaner();
            Assert.Equal(expectedOutput, fileNameCleaner.ReplaceAllInvalidChars(fileName));
        }
    }
}
