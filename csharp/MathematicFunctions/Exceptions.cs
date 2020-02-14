using System;
using System.Collections.Generic;
using System.Text;

namespace Molytho.TowerOfHanoi
{
    /// <summary>
    /// Exception indicating that there is not enough memory available for calculation
    /// </summary>
    public class TooManyMovesException : Exception
    {
        public TooManyMovesException(ulong count) => MoveCount = count;
        public ulong MoveCount { get; private set; } //The count of move being needed
    }

    /// <summary>
    /// Exception indicating that the MoveCollectionSegment is full and isn't able to resize
    /// </summary>
    public class OutOfArraySpaceException : Exception { }
}
