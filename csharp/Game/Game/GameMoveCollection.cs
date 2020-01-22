using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Molytho.TowerOfHanoi
{
    public partial class TowerOfHanoiGame
    {
        private readonly ulong _moveCount;
        private readonly MoveCollection _moveCollection;
        private readonly uint _diskCount;
        private readonly ushort _pegCount;

        public uint DiskCount => _diskCount;
        public ushort PegCount => _pegCount;
        public ulong MoveCount => _moveCount;
        public ReadOnlyMoveCollection MoveList => _moveCollection;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddMove(ushort startPeg, ushort endPeg)
        {
            _moveCollection.Add(startPeg, endPeg);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Move RemoveLastMove() => _moveCollection.RemoveLast();
    }
}
