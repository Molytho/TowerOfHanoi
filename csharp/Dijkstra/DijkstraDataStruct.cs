using System;
using System.Collections.Generic;
using System.Text;

namespace Molytho.TowerOfHanoi
{
    struct DijkstraData
    {
        public DijkstraData(ushort[] previousPoint, uint depth)
        {
            PreviousPoint = previousPoint;
            Depth = depth;
        }
        public ushort[] PreviousPoint { readonly get; set; }
        public uint Depth { readonly get; set; }
    }
}
