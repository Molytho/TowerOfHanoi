﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Molytho.TowerOfHanoi
{
    public partial class TowerOfHanoiGame
    {
        public event Action<Move> DiskMoved;
        public event Action<Move> DiskMoveReversed;
    }
}
