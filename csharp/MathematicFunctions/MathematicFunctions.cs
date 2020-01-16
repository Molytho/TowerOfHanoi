namespace Molytho.TowerOfHanoi
{
    /// <summary>
    /// Provides static functions for the Tower-of-Hanoi-MoveCount-problem
    /// </summary>
    public static class MathematicFunctions
    {
        /// <summary>
        /// Calculates the Increment of a <paramref name="pegCount"/>, <paramref name="diskCount"/> combination
        /// </summary>
        /// <param name="pegCount">Count of pegs</param>
        /// <param name="diskCount">Count of disks</param>
        /// <returns>Increment of the combination</returns>
        public static ulong Increment(ulong pegCount, ulong diskCount)
        {
            if(pegCount < 3)
                throw new System.ArgumentException("pegCounts smaller than 3 are senseless", "pegCount");
            if(diskCount == 0)
                return 0;
            ulong increment = 0, temp = 1;

            while(diskCount > temp)
            {
                increment++;
                temp = temp * (pegCount - 2 + increment) / increment;
            }

            return increment + 1;
        }
        /// <summary>
        /// Calculates minimum moves needed for solving the Tower-of-Hanoi with 3+ pegs
        /// </summary>
        /// <param name="increment">The increment of the <paramref name="pegCount"/>, <paramref name="diskCount"/> combination</param>
        /// <param name="pegCount">The peg count of the Tower-of-Hanoi game</param>
        /// <param name="diskCount">The disk count of the Tower-of-Hanoi game</param>
        /// <returns>The move count minimaly needed</returns>
        private static ulong MoveCountIntern(ulong increment, ulong pegCount, ulong diskCount)
        {
            ulong temp = diskCount * HelperFunctions.Pow(2, increment - 1);

            for(ulong i = 0; i < increment; i++)
            {
                temp += HelperFunctions.Pow(2, i) * HelperFunctions.BinomialCoefficent(i + pegCount - 3, pegCount - 3);
            }
            for(ulong i = 0; i < increment; i++)
            {
                temp -= HelperFunctions.Pow(2, increment - 1) * HelperFunctions.BinomialCoefficent(i + pegCount - 3, pegCount - 3);
            }

            return temp;
        } //Implementation for the formular

        /// <summary>
        /// Calculates minimum moves needed for solving the Tower-of-Hanoi with 3+ pegs
        /// </summary>
        /// <param name="pegCount">The peg count of the Tower-of-Hanoi game</param>
        /// <param name="diskCount">The disk count of the Tower-of-Hanoi game</param>
        /// <returns>The move count minimaly needed</returns>
        public static ulong MoveCount(ulong pegCount, ulong diskCount)
        {
            if(pegCount < 3) //Less than 3 pegs is nonsense
                throw new System.ArgumentException("pegCounts smaller than 3 are senseless", "pegCount");
            if(pegCount == 3) //3 pegs is simply 2^diskCount - 1
            {
                return HelperFunctions.Pow(2, diskCount) - 1;
            }

            ulong increment;

            increment = Increment(pegCount, diskCount); //Calculate the increment
            return MoveCountIntern(increment, pegCount, diskCount); //and pass it to the intern function
        }
    }
}
