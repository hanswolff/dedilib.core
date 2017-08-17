using DediLib.Logging;
using Xunit;

namespace Test.DediLib.Logging
{
    public class TestLogger
    {
        [Fact]
        public void GetCurrentClassLogger()
        {
            var logger = Logger.GetCurrentClassLogger();

            Assert.Equal(typeof(TestLogger).FullName, logger.Name);
        }
    }
}
