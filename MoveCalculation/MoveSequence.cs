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

        public static Task<MoveCollection> CalculateMoveSequenceAsync(int pegCount, int diskCount) => CalculateMoveSequenceAsync(pegCount, diskCount, CancellationToken.None);
        public static async Task<MoveCollection> CalculateMoveSequenceAsync(int pegCount, int diskCount, CancellationToken token)
        {
            ulong unsignedMoveCount = MathematicFunctions.MoveCount((ulong)pegCount, (ulong)diskCount);
            var moveCount = unsignedMoveCount <= int.MaxValue ? (int)unsignedMoveCount : throw new TooManyMovesException();
            var moveCollection = new MoveCollection(moveCount);
            await CalculateMoveSequenceAsync(pegCount, diskCount, GeneratePegs(pegCount), 0, pegCount - 1, token, moveCollection, moveCount);
            return moveCollection;
        }
        private static Task CalculateMoveSequenceAsync(int pegCount, int diskCount, int[] pegs, int startPeg, int endPeg, CancellationToken token, MoveCollectionBase moveCollection, int moveCount)
     => diskCount == 1
        ? Task.Run(() =>
        {
            moveCollection.Add(startPeg, endPeg);
        }, token)
        : pegCount == 3
            ? Calculate3PegAsync(diskCount, pegs, startPeg, endPeg, token, moveCollection)
            : Calculate4PlusPegAsync(pegCount, diskCount, pegs, startPeg, endPeg, token, moveCollection, moveCount);

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

        private static Task Calculate3PegAsync(int diskCount, int[] pegs, int startPeg, int endPeg, CancellationToken token, MoveCollectionBase moveCollection) => Task.Run(() => Calculate3Peg(diskCount, pegs, startPeg, endPeg, moveCollection), token);
        private static void Calculate3Peg(int diskCount, int[] pegs, int startPeg, int endPeg, MoveCollectionBase moveCollection)
        {
            int middlePeg =
                pegs[0] == startPeg || pegs[0] == endPeg
                    ? pegs[1] == startPeg || pegs[1] == endPeg
                        ? pegs[2]
                        : pegs[1]
                    : pegs[0];

            int count = 0;
            moveCollection.Add(startPeg, diskCount % 2 == 1 ? endPeg : middlePeg);
            for(int i = 1; i < diskCount; i++)
            {
                int tempPeg = (diskCount - i) % 2 == 1 ? endPeg : middlePeg;
                count = count * 2 + 1;
                moveCollection.Add(startPeg, tempPeg);
                moveCollection.InverseMoves(tempPeg, new MoveCollectionSegment(moveCollection, count + 1, count), 1 + count);
            }
        }
        private static async Task Calculate4PlusPegAsync(int pegCount, int diskCount, int[] pegs, int startPeg, int endPeg, CancellationToken token, MoveCollectionBase moveCollection, int moveCount)
        {
            var towerHeightTask = GetTowerHeightsAsync(pegCount, (int)MathematicFunctions.Increment((ulong)pegCount, (ulong)diskCount), token);

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

            var buildMoveTasks = new List<Task>();
            int tempBase = 0;
            for(int i = 0; i < pegs.Length - 2; i++)
            {
                var pegArray = GetPegArray();
                var end = GetEndPeg();
                if(heightsToBuild[i] != 0)
                {
                    var internPegCount = pegArray.Length;
                    var internMoveCount = (int)MathematicFunctions.MoveCount((ulong)internPegCount, (ulong)heightsToBuild[i]);
                    var collection = new MoveCollectionSegment(moveCollection, tempBase, internMoveCount);
                    buildMoveTasks.Add(CalculateMoveSequenceAsync(internPegCount, heightsToBuild[i], pegArray, 0, end, token, collection, internMoveCount));
                    tempBase += internMoveCount;
                }
            }

            await Task.WhenAll(buildMoveTasks);
            moveCollection.Add(startPeg, endPeg);

            int internBase = (moveCount + 1) / 2;
            int internCount = (moveCount - 1) / 2;
            moveCollection.InverseMoves(endPeg, new MoveCollectionSegment(moveCollection, internBase, internCount), 1 + internCount);
        }
    }
}
