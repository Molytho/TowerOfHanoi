using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Molytho.TowerOfHanoi;

namespace Molytho.TowerOfHanoi
{
    class DijkstraAlgorithm
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
            Graph = new DimensionModelGraphProjection<DijkstraData>(diskCount, pegCount);
            _startPoint = startPoint;
        }
        public Task CalculateAsync(uint breakCondition)
        {

            Graph[_startPoint] = new DijkstraData(null, uint.MaxValue); // Workaround that StartPoint is initialized
            if(breakCondition == uint.MaxValue)
                breakCondition = uint.MaxValue - 1;

            return CalculateAsync(_startPoint, breakCondition, 0);
        }

        private async Task CalculateAsync(ushort[] point, uint breakCondition, uint depth)
        {
            if(breakCondition == depth)
                return;

            List<Task> tasks = new List<Task>();
            var neighbourList = Graph.GetNeighbours(point);

            for(int i = 0; i < neighbourList.Count; i++)
            {
                ushort[] neighbour = neighbourList[i];
                uint neighbourDepth = Graph[neighbour].Depth;
                if(neighbourDepth == 0 || neighbourDepth > depth)
                {
                    Graph[neighbour] = new DijkstraData(point, depth);
                    tasks.Add(CalculateAsync(neighbour, breakCondition, depth + 1));
                }
            }
            neighbourList = null;

            await Task.WhenAll(tasks);
        }
    }
}
