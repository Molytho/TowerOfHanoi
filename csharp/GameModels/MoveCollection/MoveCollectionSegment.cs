namespace Molytho.TowerOfHanoi
{
    /// <summary>
    /// Represents a subsegment of a MoveCollection
    /// </summary>
    public class MoveCollectionSegment : MoveCollectionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MoveCollectionSegment"/> class
        /// </summary>
        /// <param name="collectionBase">The parent MoveCollection</param>
        /// <param name="baseIndex">The base index in the MoveCollection</param>
        /// <param name="count">The count of elements the <see cref="MoveCollectionSegment"/> should contain</param>
        public MoveCollectionSegment(MoveCollectionBase collectionBase, int baseIndex, int count) : base(count, baseIndex + collectionBase._baseAddress)
        {
            moves = collectionBase.moves; //Take the array from the parent
            collectionBase._count += count; // Increase the count of contained elements in the parent collection
        }

        /// <summary>
        /// Provide a way to resize the collection
        /// </summary>
        protected override void Resize()
        {
            //Can't resize the slave
            throw new OutOfArraySpaceException();
        }
    }
}
