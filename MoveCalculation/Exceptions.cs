using System;
using System.Collections.Generic;
using System.Text;

namespace Molytho.TowerOfHanoi
{
    public class TooManyMovesException : Exception
    {
        public TooManyMovesException(ulong count) => MoveCount = count;
        public ulong MoveCount { get; private set; }
    }
    public class OutOfArraySpaceException : Exception { }
}
