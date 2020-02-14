using System;

namespace Molytho.TowerOfHanoi
{
    public class ReadOnlyMoveCollection
    {
        internal ReadOnlyMoveCollection(Move[] moves, int count)
        {
            this.moves = moves;
            _count = count;
        }

        protected internal Move[] moves;
        protected internal int _count;

        public Move this[int index]
        {
            get
            {
                if(index >= _count || index < 0)
                    throw new IndexOutOfRangeException();

                return moves[index];
            }
        }

        public int Count => _count;
    }
}
