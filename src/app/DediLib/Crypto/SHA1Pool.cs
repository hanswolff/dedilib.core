using System.Collections.Concurrent;
using System.Linq;
using System.Security.Cryptography;

namespace DediLib.Crypto
{
    public static class SHA1Pool
    {
        private static readonly ConcurrentStack<SHA1> Stack =
            new ConcurrentStack<SHA1>(Enumerable.Range(0, 16).Select(x => SHA1.Create()));

        public static SHA1 Aquire()
        {
            SHA1 sha1;
            return Stack.TryPop(out sha1) ? sha1 : SHA1.Create();
        }

        public static void Release(SHA1 sha1)
        {
            Stack.Push(sha1);
        }
    }
}
