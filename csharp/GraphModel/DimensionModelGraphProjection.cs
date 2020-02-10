using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Molytho.TowerOfHanoi
{
    public class DimensionModelGraphProjection<T>
    {
        private readonly DimensionModelGraph<T> _dimensionGraph;
        private readonly DimensionModelGraphPointNeighbours _dimensionNeightbours;
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
        public List<ushort[]> GetNeighbours(ushort[] coords) => _dimensionNeightbours.GetNeighbours(coords);

        public DimensionModelGraphProjection(ushort pegCount, ushort diskCount)
        {
            _dimensionGraph = new DimensionModelGraph<T>(pegCount, diskCount);
            _dimensionNeightbours = new DimensionModelGraphPointNeighbours(_dimensionGraph.Count, _dimensionGraph.Lenght, _dimensionGraph.Dimension, _dimensionGraph.GetPointIndex);
        }
    }
    class DimensionModelGraphPointNeighbours
    {
        private readonly ushort _lenght;
        private readonly ushort _dimension;
        public DimensionModelGraphPointNeighbours(int size, ushort lenght, ushort dimension, Func<ushort[],int> indexFunction)
        {
            _directionCache = new List<ushort>[size];
            _lenght = lenght;
            _dimension = dimension;
            _indexFunction = indexFunction;
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

        private readonly Func<ushort[], int> _indexFunction;
        private readonly List<ushort>[] _directionCache;
        private bool IsPointDimAllowed(ushort dimension, ushort[] coords)
        {
            int index = _indexFunction(coords);
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
