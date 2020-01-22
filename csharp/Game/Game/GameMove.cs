using System;
using System.Collections.Generic;
using System.Text;

namespace Molytho.TowerOfHanoi
{
    public partial class TowerOfHanoiGame
    {
        private readonly Game.TowerOfHanoiConfiguration _configuration;
        private int _currentMove = 0;

        public void AddMove(ushort startPeg, ushort endPeg)
        {
            AddMove(startPeg, endPeg);
            MoveAdded?.Invoke(_moveCollection[_moveCollection.Count]);
        }
        public void TakeBackMove()
        {
            Move move = RemoveLastMoveFromCollection();
            MoveTakenBack?.Invoke(move);
        }

        public void NextMove()
        {
            if(_currentMove < _moveCollection.Count)
            {
                Move move = _moveCollection[_currentMove++];
                _configuration.ApplyMove(move);
                DiskMoved?.Invoke(move);
            }
        }
        public void LastMove()
        {
            if(_currentMove > 0 && _currentMove < _moveCollection.Count)
            {
                Move move = _moveCollection[_currentMove--];
                _configuration.ApplyMoveBackward(move);
                DiskMoveReversed?.Invoke(move);
            }
        }

        public uint[] DisksOnPeg(ushort peg) => _configuration[peg];
    }
}
