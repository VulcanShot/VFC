using System;

namespace VulcanFlagCreator.Extensions
{
    public static class StringExtensions
    {
        public static bool CaseInsensitiveContains(this string source, string toCheck)
        {
            return source?.IndexOf(toCheck, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
