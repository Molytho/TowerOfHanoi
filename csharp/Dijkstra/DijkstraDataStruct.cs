using System;
using System.Collections.Generic;
using System.Text;

namespace Molytho.TowerOfHanoi
{
    public readonly struct DijkstraData
    {
        internal DijkstraData(ushort[] previousPoint, uint depth)
        {
            PreviousPoint = previousPoint;
            Depth = depth;
        }
        public ushort[] PreviousPoint { get; }
        public uint Depth { get; }
    }
}
