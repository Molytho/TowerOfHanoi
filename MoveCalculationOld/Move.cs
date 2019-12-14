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
    public class MoveCollection
    {
        public MoveCollection() : this(4) { }
        public MoveCollection(int capacity)
        {
            moves = new Move[capacity];

            _capacity = capacity;
        }

        protected void Resize()
        {
            _capacity *= 2;
            Array.Resize(ref moves, _capacity);
        }

        public void Add(ref Move item)
        {
            if(_count == _capacity)
                Resize();

            moves[_count++] = item;
        }
        public void Add(int startPeg, int endPeg)
        {
            if(_count == _capacity)
                Resize();

            moves[_count++] = new Move(startPeg, endPeg);
        }
        public void AddRange(MoveCollection collection)
        {
            if(collection._count + this._count > _capacity)
            {
                Array.Resize(ref moves, _capacity + collection.Count);
            }

            Array.ConstrainedCopy(collection.moves, 0, this.moves, this._count, collection._count);

            _count += collection.Count;
        }


        private Move[] moves;
        private int _count;
        private int _capacity;

        public int Count => _count;

        public Move this[int index]
        {
            get
            {
                if(index >= _count || index < 0)
                    throw new IndexOutOfRangeException();

                return moves[index];
            }
        }

        public MoveCollection InverseMoves(int endPeg)
        {
            MoveCollection ret = new MoveCollection(_count);
            for(int i = _count - 1; i >= 0; i--)
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

        #region object
        public static bool operator ==(MoveCollection a, MoveCollection b) => a.Equals(b);
        public static bool operator !=(MoveCollection a, MoveCollection b) => !a.Equals(b);
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            for(int i = 0; i < _count; i++)
            {
                builder.Append(moves[i].ToString());
                builder.Append(",");
            }
            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }
        public override bool Equals(object obj)
        {
            if(obj is MoveCollection a)
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
            return HashCode.Combine(moves.GetHashCode(),_capacity,_count);
        }
        #endregion
    }
}
