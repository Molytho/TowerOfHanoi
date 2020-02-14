namespace Molytho.TowerOfHanoi
{
    public class MoveCollectionSegment : MoveCollectionBase
    {
        public MoveCollectionSegment(MoveCollectionBase collectionBase, int baseIndex, int count) : base(count, baseIndex + collectionBase._baseAddress)
        {
            moves = collectionBase.moves;
            collectionBase._count += count;
        }
        protected override void Resize()
        {
            throw new OutOfArraySpaceException();
        }
    }
}
