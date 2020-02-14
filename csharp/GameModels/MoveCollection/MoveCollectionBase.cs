using System;
using System.Text;

namespace Molytho.TowerOfHanoi
{
    public abstract class MoveCollectionBase
    {
        public MoveCollectionBase(int capacity, int baseAddress)
        {
            _capacity = capacity;
            _count = 0;
            _baseAddress = baseAddress;
        }

        protected internal Move[] moves;
        protected internal int _count;
        protected internal int _capacity;
        protected internal readonly int _baseAddress;

        protected abstract void Resize();

        public void Add(in Move item)
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
            if(collection._count + this._count > _capacity)
            {
                Array.Resize(ref moves, _capacity + collection._count);
            }

            Array.ConstrainedCopy(collection.moves, _baseAddress, this.moves, this._count + this._baseAddress, collection._count);

            _count += collection._count;
        }

        public ref Move this[int index]
        {
            get
            {
                if(index >= _count || index < 0)
                    throw new IndexOutOfRangeException();

                return ref moves[index + _baseAddress];
            }
        }
        public int Count => _count;

        public static explicit operator ReadOnlyMoveCollection(MoveCollectionBase moveCollectionBase)
        {
            return new ReadOnlyMoveCollection(moveCollectionBase.moves, moveCollectionBase._count);
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
}
