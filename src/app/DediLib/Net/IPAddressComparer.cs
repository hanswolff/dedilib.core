﻿using System.Collections.Generic;
using System.Net;

namespace DediLib.Net
{
    public class IPAddressComparer : IComparer<IPAddress>
    {
        public static readonly IPAddressComparer Static = new IPAddressComparer();

        public int Compare(IPAddress ip1, IPAddress ip2)
        {
            if (ip1 == null && ip2 == null) return 0;
            if (ip1 != null && ip2 == null) return 1;
            if (ip1 == null) return -1;

            var bytes1 = ip1.GetAddressBytes();
            var bytes2 = ip2.GetAddressBytes();

            var lengthCompare = bytes1.Length.CompareTo(bytes2.Length);
            if (lengthCompare != 0) return lengthCompare;

            for (var i = 0; i < bytes1.Length; i++)
            {
                var compare = bytes1[i].CompareTo(bytes2[i]);
                if (compare != 0) return compare;
            }
            return 0;
        }
    }
}
