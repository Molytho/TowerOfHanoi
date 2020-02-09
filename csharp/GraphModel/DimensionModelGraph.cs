using System;
using System.Collections.Generic;
using System.Text;

namespace Molytho.TowerOfHanoi
{
    class DimensionModelGraph<T>
    {
        private readonly T[] _pointData;
        private readonly ushort[] _weights;
        public ushort Dimension { get; }
        public ushort Lenght { get; }
        public int Count { get; }

        public DimensionModelGraph(ushort pegCount, ushort diskCount)
        {
            Count = CalculateSize(pegCount, diskCount);
            _pointData = new T[Count];

            _weights = new ushort[pegCount];
            _weights[0] = 1;
            for(ushort i = 1; i < pegCount; i++)
                _weights[i] = (ushort)(_weights[i - 1] * pegCount);

            Dimension = diskCount;
            Lenght = pegCount;
        }

        private int CalculateSize(ushort a, ushort b)
        {
            int ret = 1;
            for(; b > 0; b--)
                ret *= a;
            return ret;
        }

        public T this[ushort[] coords]
        {
            get
            {
                int index = DimensionModelPoint.GetPointIndex(coords, _weights);
                return _pointData[index];
            }
            set
            {
                int index = DimensionModelPoint.GetPointIndex(coords, _weights);
                _pointData[index] = value;
            }
        }
    }
    static class DimensionModelPoint
    {
        internal static int GetPointIndex(ushort[] coords, ushort[] weights)
        {
            if(coords.Length != weights.Length)
                throw new ArgumentException();
            int ret = 0;
            for(int i = 0; i < coords.Length; i++)
            {
                ret += coords[i] * weights[i];
            }
            return ret;
        }
    }
}
