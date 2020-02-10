using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

            _weights = new ushort[diskCount];
            _weights[0] = 1;
            for(ushort i = 1; i < diskCount; i++)
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
                int index = GetPointIndex(coords);
                return _pointData[index];
            }
            set
            {
                int index = GetPointIndex(coords);
                _pointData[index] = value;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int GetPointIndex(ushort[] coords)
        {
            if(coords.Length != _weights.Length)
                throw new ArgumentException();
            int ret = 0;
            for(int i = 0; i < coords.Length; i++)
            {
                ret += coords[i] * _weights[i];
            }
            return ret;
        }
    }
}
