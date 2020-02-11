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
        public Task CalculateAsync(uint breakCondition)
        {
            Graph[_startPoint] = new DijkstraData(new ushort[0], 0); //Workaround
            return CalculateAsync(_startPoint, breakCondition, 1);
        }

        private async Task CalculateAsync(ushort[] point, uint breakCondition, uint depth)
        {
            if(breakCondition < depth)
                return;

            List<Task> tasks = new List<Task>();
            var neighbourList = Graph.GetNeighbours(point);

            for(int i = 0; i < neighbourList.Count; i++)
            {
                ushort[] neighbour = neighbourList[i];
                uint neighbourDepth = Graph[neighbour].Depth;
                var neighbourPoint = Graph[neighbour].PreviousPoint;
                if(neighbourPoint is null || neighbourDepth > depth)
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
