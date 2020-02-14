using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Molytho.TowerOfHanoi;

namespace Molytho.TowerOfHanoi
{
    public class DijkstraAlgorithm
    {
        public ushort DiskCount { get; }
        public ushort PegCount { get; }
        public DimensionModelGraphProjection<DijkstraData> Graph { get; }
        public IReadOnlyCollection<ushort> StartPoint => _startPoint;
        private readonly ushort[] _startPoint;

        public DijkstraAlgorithm(ushort diskCount, ushort pegCount, ushort[] startPoint)
        {
            PegCount = pegCount;
            DiskCount = diskCount;
            _startPoint = startPoint;

            Graph = new DimensionModelGraphProjection<DijkstraData>(diskCount, pegCount);
        }

        private Queue<ushort[]> pointQueue = new Queue<ushort[]>();

        public void Calculate()
        {
            Graph[_startPoint] = new DijkstraData(new ushort[0], 0); //Workaround
            pointQueue.Enqueue(_startPoint);
            
            while(pointQueue.Count != 0)
            {
                var currentPoint = pointQueue.Dequeue();
                var currentPointInfo = Graph[currentPoint];

                var neighbours = Graph.GetNeighbours(currentPoint);
                foreach(ushort[] neighbourPoint in neighbours)
                {
                    var neighbourPointInfo = Graph[neighbourPoint];
                    if(neighbourPointInfo.PreviousPoint == null || neighbourPointInfo.Depth > currentPointInfo.Depth + 1)
                    {
                        pointQueue.Enqueue(neighbourPoint);
                        Graph[neighbourPoint] = new DijkstraData(currentPoint, currentPointInfo.Depth + 1);
                    }
                }
            }
        }
    }
}
