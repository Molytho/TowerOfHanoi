using System;

namespace Molytho.TowerOfHanoi
{
    public abstract class ReadOnlyMoveCollection
    {
        public ReadOnlyMoveCollection(int capacity, int baseAddress)
        {
            _capacity = capacity;
            _count = 0;
            _baseAddress = baseAddress;
        }

        protected internal Move[] moves;
        protected internal int _count;
        protected internal int _capacity;
        protected internal readonly int _baseAddress;

        public Move this[int index]
        {
            get
            {
                if(index >= _count || index < 0)
                    throw new IndexOutOfRangeException();

                return moves[index + _baseAddress];
            }
        }

        public int Count => _count;
    }
}
