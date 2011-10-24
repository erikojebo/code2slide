using System.Collections.Generic;

namespace code2slide.core.Extensions
{
    public static class ObjectExtensions
    {
        public static List<T> AsList<T>(this T obj)
        {
            return new List<T> { obj };
        }
    }
}