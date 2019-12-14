using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Molytho.TowerOfHanoi
{
    public static class MoveSequence
    {
        private static int[] GeneratePegs(int count)
        {
            int[] pegs = new int[count];
            for(int i = 0; i < count; i++)
                pegs[i] = i;
            return pegs;
        }

        public static Task<MoveCollection> CalculateMoveSequenceAsync(int pegCount, int diskCount) => CalculateMoveSequenceAsync(pegCount, diskCount, GeneratePegs(pegCount), 0, pegCount - 1, CancellationToken.None);
        public static Task<MoveCollection> CalculateMoveSequenceAsync(int pegCount, int diskCount, CancellationToken token) => CalculateMoveSequenceAsync(pegCount, diskCount, GeneratePegs(pegCount), 0, pegCount - 1, token);
        private static Task<MoveCollection> CalculateMoveSequenceAsync(int pegCount, int diskCount, int[] pegs, int startPeg, int endPeg, CancellationToken token)
        => diskCount == 1
            ? Task.Run(() =>
            {
                var ret = new MoveCollection(1);
                ret.Add(startPeg, endPeg);
                return ret;
            }, token)
            : pegCount == 3
                ? Calculate3PegAsync(diskCount, pegs, startPeg, endPeg, token)
                : Calculate4PlusPegAsync(pegCount, diskCount, pegs, startPeg, endPeg, token);

        private static Task<int[,]> GetTowerHeightsAsync(int pegCount, int increment, CancellationToken token) => Task.Run(() => GetTowerHeights(pegCount, increment), token);
        private static int[,] GetTowerHeights(int pegCount, int increment)
        {
            int[,] ret = new int[2, pegCount - 1];

            ret[0, pegCount - 2] = 1;
            ret[1, pegCount - 2] = 1;

            for(int i = 1; i < pegCount - 1; i++)
            {
                int n = increment - 2 + i;
                int k = i;
                ret[0, pegCount - 2 - i] = ret[0, pegCount - 1 - i] * n / k;
                ret[1, pegCount - 2 - i] = ret[1, pegCount - 1 - i] * (n - 1) / k;
            }

            return ret;
        }

        private static Task<MoveCollection> Calculate3PegAsync(int diskCount, int[] pegs, int startPeg, int endPeg, CancellationToken token) => Task.Run(() => Calculate3Peg(diskCount, pegs, startPeg, endPeg), token);
        private static MoveCollection Calculate3Peg(int diskCount, int[] pegs, int startPeg, int endPeg)
        {
            MoveCollection ret = new MoveCollection((int)HelperFunctions.Pow(2, (ulong)diskCount) - 1);

            int middlePeg =
                pegs[0] == startPeg || pegs[0] == endPeg
                    ? pegs[1] == startPeg || pegs[1] == endPeg
                        ? pegs[2]
                        : pegs[1]
                    : pegs[0];

            ret.Add(startPeg, diskCount % 2 == 1 ? endPeg : middlePeg);
            for(int i = 1; i < diskCount; i++)
            {
                int tempPeg = (diskCount - i) % 2 == 1 ? endPeg : middlePeg;
                var inverseMoves = ret.InverseMoves(tempPeg);
                ret.Add(startPeg, tempPeg);
                ret.AddRange(inverseMoves);
            }

            return ret;
        }

        private static async Task<MoveCollection> Calculate4PlusPegAsync(int pegCount, int diskCount, int[] pegs, int startPeg, int endPeg, CancellationToken token)
        {
            var towerHeightTask = GetTowerHeightsAsync(pegCount, (int)MathematicFunctions.Increment((ulong)pegCount, (ulong)diskCount), token);
            var moveCount = MathematicFunctions.MoveCount((ulong)pegCount, (ulong)diskCount);
            MoveCollection moveCollection = new MoveCollection((int)moveCount);

            var towerHeights = await towerHeightTask;
            towerHeightTask = null;

            int[] heightsToBuild = new int[pegCount - 2];
            int tempDisk = diskCount - 1;

            for(int i = 0; i < pegCount - 2; i++)
            {
                heightsToBuild[i] = towerHeights[1, i];
                tempDisk -= towerHeights[1, i];
            }
            for(int i = 0; i < pegCount - 2; i++)
            {
                if(towerHeights[0, i] - towerHeights[1, i] < tempDisk)
                {
                    heightsToBuild[i] = towerHeights[0, i];
                    tempDisk -= towerHeights[0, i] - towerHeights[1, i];
                }
                else
                {
                    heightsToBuild[i] += tempDisk;
                    break;
                }
            }
            towerHeights = null;

            List<int> AlreadyUsed = new List<int>();
            short EndPegSum = 0;
            Func<int[]> GetPegArray = () =>
            {
                System.Collections.Generic.List<int> ret = (pegs.Clone() as int[]).ToList();
                AlreadyUsed.ForEach((b) => ret.Remove(b));
                ret.Sort((c, d) => c.CompareTo(d));
                return ret.ToArray();
            };
            Func<int> GetEndPeg = () =>
            {
                if(AlreadyUsed.Count != pegs.Length - 2)
                {
                    int i = AlreadyUsed.Count + EndPegSum;
                    if(pegs[pegs.Length - 1 - i] == endPeg)
                    {
                        EndPegSum = 1;
                        i = AlreadyUsed.Count + EndPegSum;
                    }
                    AlreadyUsed.Add(pegs[pegs.Length - i - 1]);
                    return pegs[pegs.Length - i - 1];
                }
                else
                {
                    return endPeg;
                }
            };

            var buildMoveTasks = new List<Task<MoveCollection>>();
            for(int i = 0; i < pegs.Length - 2; i++)
            {
                var pegArray = GetPegArray();
                var end = GetEndPeg();
                if(heightsToBuild[i] != 0)
                    buildMoveTasks.Add(CalculateMoveSequenceAsync((int)pegArray.Length, heightsToBuild[i], pegArray, 0, end, token));
            }

            var moveArray = await Task.WhenAll(buildMoveTasks);
            for(int i = 0; i < (int)moveArray.Length; i++)
                moveCollection.AddRange(moveArray[i]);
            moveCollection.Add(startPeg, endPeg);
            for(int i = moveArray.Length - 1; i >= 0; i--)
                moveCollection.AddRange(moveArray[i].InverseMoves(endPeg));

            return moveCollection;
        }
    }
}
