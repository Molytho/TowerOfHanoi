using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Molytho.TowerOfHanoi
{
    public readonly struct Move : IEquatable<Move>
    {
        public Move(int startPeg, int endPeg)
        {
            StartPeg = startPeg;
            EndPeg = endPeg;
        }
        public readonly int StartPeg { get; }
        public readonly int EndPeg { get; }

        #region object
        public override bool Equals(object obj)
        {
            return obj is Move move && Equals(move);
        }
        public bool Equals(Move other)
        {
            return StartPeg == other.StartPeg &&
                   EndPeg == other.EndPeg;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(StartPeg, EndPeg);
        }
        public override string ToString()
        {
            return string.Format("[{0},{1}]", StartPeg, EndPeg);
        }
        public static bool operator ==(Move a, Move b)
        {
            return a.StartPeg == b.StartPeg && a.EndPeg == b.EndPeg;
        }
        public static bool operator !=(Move a, Move b) => !(a == b);
        #endregion
    }

    public abstract class MoveCollectionBase
    {
        public MoveCollectionBase(int capacity, int baseAddress)
        {
            _capacity = capacity;
            _count = 0;
            _baseAddress = baseAddress;
        }

        protected abstract void Resize();

        public void Add(ref Move item)
        {
            if(_count == _capacity)
                Resize();

            moves[_count++ + _baseAddress] = item;
        }
        public void Add(int startPeg, int endPeg)
        {
            if(_count == _capacity)
                Resize();

            moves[_count++ + _baseAddress] = new Move(startPeg, endPeg);
        }
        public void AddRange(MoveCollectionBase collection)
        {
            if(collection.Count + this._count > _capacity)
            {
                Array.Resize(ref moves, _capacity + collection.Count);
            }

            Array.ConstrainedCopy(collection.moves, _baseAddress, this.moves, this._count + this._baseAddress, collection._count);

            _count += collection.Count;
        }


        protected internal Move[] moves;
        protected internal int _count;
        protected internal int _capacity;
        protected internal readonly int _baseAddress;

        public int Count => _count;

        public Move this[int index]
        {
            get
            {
                if(index >= _count || index < 0)
                    throw new IndexOutOfRangeException();

                return moves[index + _baseAddress];
            }
        }

        public MoveCollectionBase InverseMoves(int endPeg)
        {
            MoveCollection ret = new MoveCollection(_count);
            for(int i = _baseAddress + _count - 1; i >= _baseAddress; i--)
            {
                int end =
                    moves[i].StartPeg != 0 && moves[i].StartPeg != endPeg
                        ? moves[i].StartPeg
                        : moves[i].StartPeg == 0
                            ? endPeg
                            : 0;
                int start =
                   moves[i].EndPeg != 0 && moves[i].EndPeg != endPeg
                       ? moves[i].EndPeg
                       : moves[i].EndPeg == 0
                           ? endPeg
                           : 0;
                ret.Add(start, end);
            }
            return ret;
        }
        public void InverseMoves(int endPeg, MoveCollectionBase collection, int noCopy)
        {
            if(_count > collection._capacity - collection._count + noCopy)
                collection.Resize();
            for(int i = _baseAddress + _count - 1 - noCopy; i >= _baseAddress; i--)
            {
                int end =
                    moves[i].StartPeg != 0 && moves[i].StartPeg != endPeg
                        ? moves[i].StartPeg
                        : moves[i].StartPeg == 0
                            ? endPeg
                            : 0;
                int start =
                   moves[i].EndPeg != 0 && moves[i].EndPeg != endPeg
                       ? moves[i].EndPeg
                       : moves[i].EndPeg == 0
                           ? endPeg
                           : 0;
                collection.Add(start, end);
            }
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
    }
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
