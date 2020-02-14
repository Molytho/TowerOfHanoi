using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Molytho.TowerOfHanoi
{
    public class DimensionModelGraphProjection<T>
    {
        private readonly DimensionModelPointTranslator _pointTranslator;
        private readonly DimensionModelGraph<T> _dimensionGraph;
        private readonly DimensionModelGraphPointNeighbours _dimensionNeighbours;
        public T this[ushort[] coords]
        {
            get
            {
                return _dimensionGraph[coords];
            }
            set
            {
                _dimensionGraph[coords] = value;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public List<ushort[]> GetNeighbours(ushort[] coords) => _dimensionNeighbours.GetNeighbours(coords);
        public uint Count => _dimensionGraph.Count;

        public DimensionModelGraphProjection(ushort diskCount, ushort pegCount)
        {
            _pointTranslator = new DimensionModelPointTranslator(diskCount, pegCount);
            _dimensionGraph = new DimensionModelGraph<T>(diskCount, pegCount, _pointTranslator);
            _dimensionNeighbours = new DimensionModelGraphPointNeighbours(_dimensionGraph.Count, _dimensionGraph.Lenght, _dimensionGraph.Dimension, _pointTranslator);
        }
    }
    class DimensionModelGraphPointNeighbours
    {
        private readonly ushort _lenght;
        private readonly ushort _dimension;
        public DimensionModelGraphPointNeighbours(uint size, ushort lenght, ushort dimension, DimensionModelPointTranslator pointTranslator)
        {
            _directionCache = new List<ushort>[size];
            _lenght = lenght;
            _dimension = dimension;
            _pointTranslator = pointTranslator;
        }
        public List<ushort[]> GetNeighbours(ushort[] coords)
        {
            List<ushort[]> ret = new List<ushort[]>();
            for(ushort i = 0; i < _dimension; i++)
            {
                if(!IsPointDimAllowed(i, coords))
                    continue;
                for(ushort l = 0; l < _lenght; l++)
                {
                    if(l == coords[i])
                        continue;
                    ushort[] coordsTemp = new ushort[_dimension];
                    coords.CopyTo(coordsTemp, 0);

                    coordsTemp[i] = l;
                    if(IsPointDimAllowed(i, coordsTemp))
                        ret.Add(coordsTemp);
                }
            }
            return ret;
        }

        private readonly DimensionModelPointTranslator _pointTranslator;
        private readonly List<ushort>[] _directionCache;
        private bool IsPointDimAllowed(ushort dimension, ushort[] coords)
        {
            uint index = _pointTranslator.GetPointIndex(coords);
            _directionCache[index] ??= GetPointDimAllowed(coords);
            List<ushort> pointDims = _directionCache[index];

            return pointDims.Contains(dimension);
        }
        private List<ushort> GetPointDimAllowed(ushort[] coords)
        {
            List<ushort> ret = new List<ushort>();
            for(ushort i = 0; i < _lenght; i++)
            {
                for(ushort index = 0; index < coords.Length; index++)
                {
                    if(coords[index] == i)
                    {
                        ret.Add(index);
                        break;
                    }
                }
            }
            return ret;
        }
    }
}
