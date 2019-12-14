using System;
using System.Collections.Generic;
using System.Text;

namespace Molytho.TowerOfHanoi
{
    public static class HelperFunctions
    {
        public static ulong Pow(ulong _base, ulong exponent)
        {
            ulong ret = 1;
            for(;exponent > 0; exponent--)
            {
                ret *= _base;
            }
            return ret;
        }
    }
}
