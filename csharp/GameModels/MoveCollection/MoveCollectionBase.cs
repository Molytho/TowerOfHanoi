using System;
using System.Text;

namespace Molytho.TowerOfHanoi
{
    /// <summary>
    /// Represents the base class of a MoveCollection
    /// </summary>
    public abstract class MoveCollectionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MoveCollectionBase"/> class
        /// </summary>
        /// <param name="capacity">Capacity of the collection</param>
        /// <param name="baseAddress">Index start point in the <see cref="Move"/> array</param>
        public MoveCollectionBase(int capacity, int baseAddress)
        {
            // Set the local variables
            _capacity = capacity;
            _count = 0;
            _baseAddress = baseAddress;
        }

        protected internal Move[] moves; //The array the elements are saved in
        protected internal int _count; //The count of elements contained by the collection
        protected internal int _capacity; //The maximal count of elements the collection can contain
        protected internal readonly int _baseAddress; //The base index the Indexer uses to index in moves

        /// <summary>
        /// Provides the resize logic íf the capacity is reached
        /// </summary>
        protected abstract void Resize();

        /// <summary>
        /// Adds <paramref name="item"/> to the collection
        /// </summary>
        /// <param name="item">Element being added</param>
        public void Add(in Move item)
        {
            //If the collection is full
            if(_count == _capacity)
                Resize(); //then resize it

            //Insert the element and increase the count of contained elements
            moves[_count++ + _baseAddress] = item;
        }
        /// <summary>
        /// Adds a new instance of <see cref="Move"/> to the collection
        /// </summary>
        /// <param name="startPeg">StartPeg of the move</param>
        /// <param name="endPeg">EndPeg of the move</param>
        public void Add(int startPeg, int endPeg)
        {
            //If the collection is full
            if(_count == _capacity)
                Resize(); //then resize it

            //Insert the element and increase the count of contained elements
            moves[_count++ + _baseAddress] = new Move(startPeg, endPeg);
        }
        /// <summary>
        /// Adds the elements of the given <see cref="MoveCollectionBase"/>
        /// </summary>
        /// <param name="collection">Collection with elements being added</param>
        public void AddRange(MoveCollectionBase collection)
        {
            //If the capacity of the collection is to small
            if(collection._count + this._count > _capacity)
            {
                //then resize the array
                Array.Resize(ref moves, _capacity += collection._count);
            }

            //Copy the array content to the current collection
            Array.ConstrainedCopy(collection.moves, _baseAddress, this.moves, this._count + this._baseAddress, collection._count);

            //Increase the count of cotained elements
            _count += collection._count;
        }

        /// <summary>
        /// Returns the element at the given <paramref name="index"/>
        /// </summary>
        /// <param name="index">The index in the collection</param>
        /// <returns>A reference to the element at the given <paramref name="index"/></returns>
        public ref Move this[int index]
        {
            get
            {
                //Test íf the index exceeds the array borders
                if(index >= _count || index < 0)
                    throw new IndexOutOfRangeException();

                //Return a reference to the elemént at the given index
                return ref moves[index + _baseAddress];
            }
        }
        /// <summary>
        /// Returns the count of elements contained by the collection
        /// </summary>
        public int Count => _count; //Encapsulation of the local variable _count

        public static explicit operator ReadOnlyMoveCollection(MoveCollectionBase moveCollectionBase)
        {
            return new ReadOnlyMoveCollection(moveCollectionBase.moves, moveCollectionBase._count);
        }

        /// <summary>
        /// Applíes the contained moves backwards in relation to the given <paramref name="endPeg"/>
        /// </summary>
        /// <param name="endPeg">The peg, the tower should be built on<</param>
        /// <returns>A <see cref="MoveCollectionBase"/> containing the reversed moves</returns>
        public MoveCollectionBase InverseMoves(int endPeg)
        {
            //Create the MoveCollection being returned
            MoveCollection ret = new MoveCollection(_count);

            //Iterate through the full array space from top to bottom
            for(int i = _baseAddress + _count - 1; i >= _baseAddress; i--)
            {
                int end = //Calculate the endPeg of the inverse Move
                    moves[i].StartPeg != 0 && moves[i].StartPeg != endPeg //if the startPeg of the move isn't the global startPeg (0) or the given endPeg
                        ? moves[i].StartPeg //then it's the startPeg of the move
                        : moves[i].StartPeg == 0 //but if it's one of them then test which one it is
                            ? endPeg //if it's the global startPeg then it's the given endPeg
                            : 0; //or if it's the given endPeg then it's the global startPeg
                int start = //Calculate the startPeg of the inverse Move
                   moves[i].EndPeg != 0 && moves[i].EndPeg != endPeg //if the endPeg of the move isn't the global startPeg (0) or the given endPeg
                       ? moves[i].EndPeg //then it's the endPeg of the move
                       : moves[i].EndPeg == 0 //but if it's one of them then test which one it is
                           ? endPeg //if it's the global startPeg then it's the endPeg
                           : 0; //or if it's the given endPeg then it's the global startPeg
                //Add the move to the return collection
                ret.Add(start, end);
            }
            //And return it
            return ret;
        }
        /// <summary>
        /// Applies the contained moves backwards in relation the the given endPeg
        /// </summary>
        /// <param name="endPeg">´The peg, the tower should be built on</param>
        /// <param name="collection">The <see cref="MoveCollectionBase"/> the moves shuold be saved in</param>
        /// <param name="noCopy">The count of elements at the end which shouldn't be applied reversely </param>
        public void InverseMoves(int endPeg, MoveCollectionBase collection, int noCopy)
        {
            //While there isn't enough space available in the collection
            while(_count > collection._capacity - collection._count + noCopy)
                collection.Resize(); //Resize it

            //Iterate through the array space without the given count of element in noCopy from top to bottom
            for(int i = _baseAddress + _count - 1 - noCopy; i >= _baseAddress; i--)
            {
                int end = //Calculate the endPeg of the inverse Move
                    moves[i].StartPeg != 0 && moves[i].StartPeg != endPeg //if the startPeg of the move isn't the global startPeg (0) or the given endPeg
                        ? moves[i].StartPeg //then it's the startPeg of the move
                        : moves[i].StartPeg == 0 //but if it's one of them then test which one it is
                            ? endPeg //if it's the global startPeg then it's the given endPeg
                            : 0; //or if it's the given endPeg then it's the global startPeg
                int start = //Calculate the startPeg of the inverse Move
                   moves[i].EndPeg != 0 && moves[i].EndPeg != endPeg //if the endPeg of the move isn't the global startPeg (0) or the given endPeg
                       ? moves[i].EndPeg //then it's the endPeg of the move
                       : moves[i].EndPeg == 0 //but if it's one of them then test which one it is
                           ? endPeg //if it's the global startPeg then it's the endPeg
                           : 0; //or if it's the given endPeg then it's the global startPeg
                //Add the move to the collection
                collection.Add(start, end);
            }
            //Don't return anything
        }

        #region object
        public static bool operator ==(MoveCollectionBase a, MoveCollectionBase b) => a.Equals(b);
        public static bool operator !=(MoveCollectionBase a, MoveCollectionBase b) => !a.Equals(b);
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            for(int i = 0; i < _count; i++)
            {
                builder.Append(moves[i + _baseAddress].ToString());
                builder.Append(",");
            }
            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }
        public override bool Equals(object obj)
        {
            if(obj is MoveCollectionBase a)
            {
                try
                {
                    for(int i = 0; i < _count; i++)
                    {
                        if(a[i] != this[i])
                            return false;
                    }
                    return true;
                }
                catch(Exception)
                {
                    return false;
                }
            }
            return false;
        }
        public override int GetHashCode()
        {
            return moves.GetHashCode();
        }
        #endregion
    }
}