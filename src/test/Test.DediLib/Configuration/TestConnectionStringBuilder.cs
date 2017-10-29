using System;
using DediLib.Configuration;
using Xunit;

namespace Test.DediLib.Configuration
{
    public class TestConnectionStringBuilder
    {
        [Fact]
        public void With_NameHasIllegalCharacters_Throws()
        {
            var builder = ConnectionStringBuilder.New();
            Assert.Throws<ArgumentException>(() => builder.With("name with illegal chars #!@", ""));
        }

        [Fact]
        public void With_NameDoesNotStartWithLetter_Throws()
        {
            var builder = ConnectionStringBuilder.New();
            Assert.Throws<ArgumentException>(() => builder.With("1name", ""));
        }

        [Fact]
        public void With_ValueHasIllegalCharacters_Throws()
        {
            var builder = ConnectionStringBuilder.New();
            Assert.Throws<ArgumentException>(() => builder.With("name", "value with illegal chars \r\n"));
        }

        [Theory]
        [InlineData("1+1=2")]
        [InlineData(" precedingspace")]
        [InlineData("spaceattheend ")]
        [InlineData("semicolon;value")]
        public void With_ValueThatNeedsToBeEscaped_ConnectionString(string value)
        {
            var builder = ConnectionStringBuilder.New();
            builder.With("name", value);

            Assert.Equal($"name='{value}'", builder.Build());
        }

        [Fact]
        public void Build_SingleName_ConnectionString()
        {
            var builder = ConnectionStringBuilder.New();
            builder.With("name", null);

            Assert.Equal("name", builder.Build());
        }

        [Fact]
        public void Build_SingleNameAndStringValue_ConnectionString()
        {
            var builder = ConnectionStringBuilder.New();
            builder.With("name", "value");

            Assert.Equal("name=value", builder.Build());
        }

        [Fact]
        public void Build_SingleNameAndBoolValue_ConnectionString()
        {
            var builder = ConnectionStringBuilder.New();
            builder.With("name", true);

            Assert.Equal("name=True", builder.Build());
        }

        [Fact]
        public void Build_OverrideExistingNameAndValue_ConnectionString()
        {
            var builder =
                ConnectionStringBuilder.New()
                .With("name", "value")
                .With("Name", "VALUE");

            Assert.Equal("name=VALUE", builder.Build());
        }

        [Fact]
        public void Build_MultipleNamesAndValues_ConnectionString()
        {
            var builder =
                ConnectionStringBuilder.New()
                .With("name1", "value1")
                .With("name2", "value2");

            Assert.Equal("name1=value1;name2=value2", builder.Build());
        }

        [Theory]
        [InlineData("name1", "name1")]
        [InlineData("name1;", "name1")]
        [InlineData("name1;;", "name1")]
        [InlineData("name1; ;", "name1")]
        [InlineData("name1; name2;", "name1;name2")]
        [InlineData("name1=value1; name2=value2;", "name1=value1;name2=value2")]
        [InlineData("name1 = value1; name2 = value2", "name1=value1;name2=value2")]
        [InlineData("name1 = 'value;1'; name2 = value2", "name1='value;1';name2=value2")]
        public void Parse_Build_ConnectionString(string connectionString, string buildConnectionString)
        {
            var builder =
                ConnectionStringBuilder.Parse(connectionString);

            Assert.Equal(buildConnectionString, builder.Build());
        }
    }
}
