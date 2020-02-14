using System;

namespace Molytho.TowerOfHanoi
{
    public class MoveCollection : MoveCollectionBase
    {
        public MoveCollection() : this(4) { }
        public MoveCollection(int capacity) : base(capacity, 0)
        {
            moves = new Move[capacity];
        }

        protected override void Resize()
        {
            _capacity *= 2;
            Array.Resize(ref moves, _capacity);
        }
        public Move RemoveLast()
        {
            _count--;
            return moves[_count + 1];
        }
    }
}
