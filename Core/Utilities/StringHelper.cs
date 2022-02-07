using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSystem.Core.Utilities
{
    public static class StringHelper
    {
        public static string TrimWhiteSpace (this string value)
        {
            return string.Join(" ", value.Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries).ToList().Select(x => x.Trim()));
        }
    }
}
