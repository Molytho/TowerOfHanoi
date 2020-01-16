namespace Molytho.TowerOfHanoi
{
    /// <summary>
    /// Provides static functions needed to calculate the Tower-of-Hanoi-problems
    /// </summary>
    public static class HelperFunctions
    {
        /// <summary>
        /// Calculate the potential of the given base and exponent.<br></br>
        /// Only able to use positive integer values
        /// </summary>
        /// <param name="_base">The base of the potential function</param>
        /// <param name="exponent">The exponent of potential function</param>
        /// <returns>The solution of the potential function</returns>
        public static ulong Pow(ulong _base, ulong exponent)
        {
            ulong ret = 1; //Set ret to 1 because of _base^0 = 1
            for(; exponent > 0; exponent--) //Multiply ret exponent times with _base
            {
                ret *= _base;
            }
            return ret; // return solution
        }
        
        /// <summary>
        /// Calculates the Binomial Coefficent
        /// </summary>
        /// <param name="n"></param>
        /// <param name="k"></param>
        /// <returns>n nCr k</returns>
        public static ulong BinomialCoefficent(ulong n, ulong k)
        {
            if(k > n) //When k bigger than n define BK as 0
                return 0;
            if(2 * k > n) //When k is over the symetrical axis of pascals triangle
                k = n - k; //Then set k to its symetrical opponent
            if(k == 0) //When k is 0
                return 1; //Then the solution is always 1
            ulong ergebnis = 1; //Set ergebnis to 1 because n nCr 0 = 1
            for(ulong i = 1; i <= k; i++) //Multiply through the diagonal of pascals triangle
            {
                ergebnis = ergebnis * (n - k + i) / i; //because (n + 1) nCr (k + 1) = (n nCr k) * (n + 1) / (k + 1)
            }
            return ergebnis;
        }
    }
}
