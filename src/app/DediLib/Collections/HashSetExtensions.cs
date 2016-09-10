using System.Collections.Generic;
using System.Text;

namespace DediLib.Collections
{
    public static class HashSetExtensions
    {
        /// <summary>
        /// Checks if a delimited part is in the HashSet
        /// </summary>
        /// <remarks>
        /// Example:<br/>
        /// <br/>
        /// var hashSet = new HashSet&lt;string&gt; { "test.it" };<br/>
        /// hashSet.ContainsSuffixFor("test", '.'); // false<br/>
        /// hashSet.ContainsSuffixFor("test.it", '.'); // true<br/>
        /// hashSet.ContainsSuffixFor("it", '.'); // false<br/>
        /// hashSet.ContainsSuffixFor("www.test.it", '.'); // true<br/>
        /// hashSet.ContainsSuffixFor("www.sub.test.it", '.'); // true<br/>
        /// hashSet.ContainsSuffixFor("wwwtest.it", '.'); // false<br/>
        /// </remarks>
        /// <param name="hashSet"></param>
        /// <param name="text"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static bool ContainsSuffixFor(this HashSet<string> hashSet, string text, char delimiter)
        {
            if (string.IsNullOrEmpty(text)) return false;

            var sb = new StringBuilder();
            var parts = text.Split(delimiter);
            for (var i = parts.Length - 1; i >= 0; i--)
            {
                if (sb.Length > 0) sb.Insert(0, delimiter);
                sb.Insert(0, parts[i]);
                if (hashSet.Contains(sb.ToString())) return true;
            }
            return false;
        }
    }
}
