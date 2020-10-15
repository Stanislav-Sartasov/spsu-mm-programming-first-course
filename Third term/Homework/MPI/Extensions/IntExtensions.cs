using System;
using System.Collections.Generic;
using System.Text;

namespace Extensions
{
    public static class IntExtensions
    {
        public static bool IsPowerOfTwo(this int x)
        {
            return (int)Math.Ceiling(Math.Log(x) / Math.Log(2))
             == (int)Math.Floor((Math.Log(x) / Math.Log(2)));
        }
    }
}
