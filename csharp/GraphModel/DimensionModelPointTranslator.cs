using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Molytho.TowerOfHanoi
{
    class DimensionModelPointTranslator
    {
        private readonly ushort[] _weights;

        public DimensionModelPointTranslator(ushort pegCount, ushort diskCount)
        {
            _weights = new ushort[diskCount];
            _weights[0] = 1;
            for(ushort i = 1; i < diskCount; i++)
                _weights[i] = (ushort)(_weights[i - 1] * pegCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetPointIndex(ushort[] coords)
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
