using System;

namespace Molytho.TowerOfHanoi
{
    /// <summary>
    /// Represents a move in the towers of hanoi game
    /// </summary>
    public readonly struct Move : IEquatable<Move>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Move"/> struct
        /// </summary>
        /// <param name="startPeg">The StartPeg of the move</param>
        /// <param name="endPeg">The EndPeg of the move</param>
        public Move(int startPeg, int endPeg)
        {
            StartPeg = startPeg;
            EndPeg = endPeg;
        }

        /// <summary>
        /// Indicates the StartPeg of the move
        /// </summary>
        public readonly int StartPeg { get; }
        /// <summary>
        /// Indicates the EndPeg of the move
        /// </summary>
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
}
