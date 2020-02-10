using System;
using System.Collections.Generic;
using System.Text;

namespace Molytho.TowerOfHanoi
{
    public struct DijkstraData
    {
        internal DijkstraData(ushort[] previousPoint, uint depth)
        {
            PreviousPoint = previousPoint;
            Depth = depth;
        }
        public ushort[] PreviousPoint { readonly get; set; }
        public uint Depth { readonly get; set; }
    }
}
