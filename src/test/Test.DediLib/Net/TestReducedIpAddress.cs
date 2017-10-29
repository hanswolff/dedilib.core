using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using DediLib.Net;
using Xunit;

namespace Test.DediLib.Net
{
    public class TestReducedIpAddress
    {
        [Fact]
        public void If_ip_is_null_Then_reduced_value_is_zero()
        {
            var reducedIp = new ReducedIpAddress(null);

            Assert.Equal(0UL, reducedIp.ReducedIpValue);
        }

        [Theory]
        [InlineData("127.0.0.1")]
        [InlineData("127.1.0.1")]
        [InlineData("::1")]
        public void If_ip_is_localhost_Then_reduced_values_are_same(string ipAddress)
        {
            var reducedIp = new ReducedIpAddress(IPAddress.Parse(ipAddress));

            Assert.Equal(1328304962299559429UL, reducedIp.ReducedIpValue);
        }

        [Fact]
        public void If_ipv4_is_mapped_to_ipv6_Then_reduced_values_are_same()
        {
            var ipv4 = IPAddress.Parse("142.52.77.134");
            var ipv6 = ipv4.MapToIPv6();

            var reducedIpv4 = new ReducedIpAddress(ipv4);
            var reducedIpv6 = new ReducedIpAddress(ipv6);

            Assert.Equal(reducedIpv4.ReducedIpValue, reducedIpv6.ReducedIpValue);
        }

        [Theory]
        [InlineData("2a02:8108:4740:780c:594f:4602:31ba:8e2e")]
        [InlineData("2a02:8108:4740:780c:594f:4602:31ba:9999")]
        [InlineData("2a02:8108:4740:780c:594f:4602:8888:9999")]
        [InlineData("2a02:8108:4740:780c:594f:7777:8888:9999")]
        [InlineData("2a02:8108:4740:780c:6666:7777:8888:9999")]
        [InlineData("2a02:8108:4740:780c::")]
        public void If_ipv6_has_same_64_bit_subnet_Then_reduced_values_are_same(string ipAddress)
        {
            var reducedIp = new ReducedIpAddress(IPAddress.Parse(ipAddress));

            Assert.Equal(9120338691296830918UL, reducedIp.ReducedIpValue);
        }

        [Trait("Category", "Benchmark")]
        [Fact]
        public void Benchmark_ReducedIpAddress()
        {
            const int count = 1000000;
            var ips = new[] { IPAddress.Parse("2a02:8108:4740:780c::"), IPAddress.Loopback };
            var tasks = Enumerable.Range(0, count).Select(x => new ReducedIpAddress(ips[x % ips.Length]));

            var sw = Stopwatch.StartNew();
            var list = tasks.ToList();
            sw.Stop();

            Assert.Equal(ips.Length, list.Select(x => x.ReducedIpValue).Distinct().Count());

            var opsPerSec = count / (sw.ElapsedMilliseconds + 0.0001m) * 1000m;
            Console.WriteLine($"{sw.Elapsed} ({opsPerSec:N0} ops/sec)");
        }
    }
}
