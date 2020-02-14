using System;

namespace Molytho.TowerOfHanoi
{
    /// <summary>
    /// Represents a readonly <see cref="MoveCollection"/>
    /// </summary>
    public class ReadOnlyMoveCollection
    {
        /// <summary>
        /// Initializese a new instance of <see cref="ReadOnlyMoveCollection"/> class
        /// </summary>
        /// <param name="moves">Move array of <see cref="MoveCollection"/> object</param>
        /// <param name="count">Count of objects in the <see cref="ReadOnlyMoveCollection"/> instance</param>
        internal ReadOnlyMoveCollection(Move[] moves, int count)
        {
            this.moves = moves;
            _count = count;
        }

        protected internal Move[] moves;
        protected internal int _count;

        /// <summary>
        /// Gets an Element of the <see cref="ReadOnlyMoveCollection"/>
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Move this[int index]
        {
            get
            {
                if(index >= _count || index < 0)
                    throw new IndexOutOfRangeException();

                return moves[index];
            }
        }

        /// <summary>
        /// Gets the number of Elements in the <see cref="ReadOnlyMoveCollection"/>
        /// </summary>
        public int Count => _count;
    }
}
