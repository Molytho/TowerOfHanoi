using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Molytho.TowerOfHanoi
{
    class DimensionModelGraph<T>
    {
        private readonly DimensionModelPointTranslator _pointTranslator;
        private readonly T[] _pointData;
        public ushort Dimension { get; }
        public ushort Lenght { get; }
        public uint Count { get; }

        public DimensionModelGraph(ushort diskCount, ushort pegCount, DimensionModelPointTranslator pointTranslator)
        {
            Count = CalculateSize(pegCount, diskCount);
            _pointData = new T[Count];

            _pointTranslator = pointTranslator;

            Dimension = diskCount;
            Lenght = pegCount;
        }

        private uint CalculateSize(ushort a, ushort b)
        {
            uint ret = 1;
            for(; b > 0; b--)
                ret *= a;
            return ret;
        }

        public T this[ushort[] coords]
        {
            get
            {
                uint index = _pointTranslator.GetPointIndex(coords);
                return _pointData[index];
            }
            set
            {
                uint index = _pointTranslator.GetPointIndex(coords);
                _pointData[index] = value;
            }
        }
    }
}
