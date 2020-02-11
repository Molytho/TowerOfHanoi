using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Molytho.TowerOfHanoi
{
    class DimensionModelPointTranslator
    {
        private readonly uint[] _weights;

        public DimensionModelPointTranslator(ushort diskCount, ushort pegCount)
        {
            _weights = new uint[diskCount];
            _weights[0] = 1;
            for(uint i = 1; i < diskCount; i++)
                _weights[i] = _weights[i - 1] * pegCount;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint GetPointIndex(ushort[] coords)
        {
            if(coords.Length != _weights.Length)
                throw new ArgumentException();
            uint ret = 0;
            for(int i = 0; i < coords.Length; i++)
            {
                ret += coords[i] * _weights[i];
            }
            return ret;
        }
    }
}
