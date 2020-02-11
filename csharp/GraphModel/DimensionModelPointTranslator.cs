using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Molytho.TowerOfHanoi
{
    class DimensionModelPointTranslator
    {
        private readonly uint[] _weights;

        public DimensionModelPointTranslator(ushort dimension, ushort length)
        {
            _weights = new uint[dimension];
            _weights[0] = 1;
            for(uint i = 1; i < dimension; i++)
                _weights[i] = _weights[i - 1] * length;
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
