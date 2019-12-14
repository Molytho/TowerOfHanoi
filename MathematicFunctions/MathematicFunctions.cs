using System;

namespace Molytho.TowerOfHanoi
{
    public static class MathematicFunctions
    {
        public static ulong BinomialCoefficent(ulong n, ulong k)
        {
            if(k == 0)
                return 1;
            if(2 * k > n)
                k = n - k;
            ulong ergebnis = 1;
            for(ulong i = 1; i <= k; i++)
            {
                ergebnis = ergebnis * (n - k + i) / i;
            }
            return ergebnis;
        }

        public static ulong Increment(ulong pegCount, ulong diskCount)
        {
            if(diskCount == 0)
                return 0;
            ulong increment = 0, temp = 1;

            while(diskCount > temp)
            {
                increment++;
                temp = (ulong)(temp * (pegCount - 2d + increment) / increment);
            }

            return increment + 1;
        }

        private static ulong MoveCountIntern(ulong increment, ulong pegCount, ulong diskCount)
        {
            ulong temp = diskCount * HelperFunctions.Pow(2, increment - 1);

            for(ulong i = 0; i < increment; i++)
            {
                temp += HelperFunctions.Pow(2, i) * BinomialCoefficent(i + pegCount - 3, pegCount - 3);
            }
            for(ulong i = 0; i < increment; i++)
            {
                temp -= HelperFunctions.Pow(2, increment - 1) * BinomialCoefficent(i + pegCount - 3, pegCount - 3);
            }

            return temp;
        }

        public static ulong MoveCount(ulong pegCount, ulong diskCount)
        {
            if(pegCount == 3)
            {
                return HelperFunctions.Pow(2, diskCount) - 1;
            }

            ulong increment;

            increment = Increment(pegCount, diskCount);
            return MoveCountIntern(increment, pegCount, diskCount);
        }
    }
}
