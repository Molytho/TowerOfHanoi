using System;
using System.Collections.Generic;
using System.Text;

namespace Molytho.TowerOfHanoi
{
    public partial class TowerOfHanoiGame
    {
        private readonly Game.TowerOfHanoiConfiguration _configuration;
        private readonly bool _autoMove;
        private int _currentMove = 0;

        public void AddMove(ushort startPeg, ushort endPeg)
        {
            AddMove(startPeg, endPeg);
            MoveAdded?.Invoke(_moveCollection[_moveCollection.Count]);

            if(_autoMove && _currentMove + 1 == _moveCollection.Count)
                NextMove();
        }
        public void TakeBackMove()
        {
            Move move = RemoveLastMoveFromCollection();
            MoveTakenBack?.Invoke(move);

            if(_currentMove - 1 == _moveCollection.Count)
            {
                LastMove(move);
                _currentMove--;
            }
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
        public void LastMove(Move? move = null)
        {
            if(_currentMove > 0 && _currentMove < _moveCollection.Count)
            {
                Move moveTemp = move ?? _moveCollection[_currentMove--];
                _configuration.ApplyMoveBackward(moveTemp);
                DiskMoveReversed?.Invoke(moveTemp);
            }
        }

        public uint[] DisksOnPeg(ushort peg) => _configuration[peg];
    }
}
