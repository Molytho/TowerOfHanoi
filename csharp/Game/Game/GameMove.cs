using System;
using System.Collections.Generic;
using System.Text;

namespace Molytho.TowerOfHanoi
{
    public partial class TowerOfHanoiGame
    {
        private readonly Game.TowerOfHanoiConfiguration _configuration;

        public void MoveDisk(ushort startPeg, ushort endPeg)
        {
            AddMove(startPeg, endPeg);
            _configuration.ApplyMove(startPeg, endPeg);
            DiskMoved?.Invoke(_moveCollection[_moveCollection.Count]);
        }
        public void MoveDiskBackward()
        {
            Move move = RemoveLastMove();
            _configuration.ApplyMoveBackward(move);
            DiskMoveReversed?.Invoke(move);
        }

        public uint[] DisksOnPeg(ushort peg) => _configuration[peg];
    }
}
