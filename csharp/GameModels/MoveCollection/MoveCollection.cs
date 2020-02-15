using System;

namespace Molytho.TowerOfHanoi
{
    /// <summary>
    /// Represents the master class of a MoveCollection
    /// </summary>
    public class MoveCollection : MoveCollectionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MoveCollection"/> class with a capacity of 4
        /// </summary>
        public MoveCollection() : this(4) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="MoveCollection"/> class
        /// </summary>
        /// <param name="capacity">The capacity of the collection</param>
        public MoveCollection(int capacity) : base(capacity, 0)
        {
            moves = new Move[capacity];
        }

        /// <summary>
        /// Provide a way to resize the collection
        /// </summary>
        protected override void Resize()
        {
            _capacity *= 2; //Double the capacity
            Array.Resize(ref moves, _capacity); //And resize the array
        }

        /// <summary>
        /// Pops the last element in the array
        /// </summary>
        /// <returns>The last element</returns>
        public Move RemoveLast()
        {
            //Return the move and decrease the element count
            return moves[_count--];
        }
    }
}
