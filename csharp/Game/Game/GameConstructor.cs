using System;
using System.Collections.Generic;
using System.Text;

namespace Molytho.TowerOfHanoi
{
    public partial class TowerOfHanoiGame
    {
        public TowerOfHanoiGame(uint diskCount, ushort pegCount)
        {
            _moveCount = MathematicFunctions.MoveCount(pegCount, diskCount);
            _moveCollection = new MoveCollection((int)(_moveCount * 1.5));
            _diskCount = diskCount;
            _pegCount = pegCount;

            _configuration = new Game.TowerOfHanoiConfiguration(diskCount, pegCount);
        }
    }
}
