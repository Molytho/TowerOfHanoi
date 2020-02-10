﻿#define DimensionModelGraph

using System;
using System.IO;
using System.Threading;

namespace TestConsole
{
    class Program
    {
        static void Main()
        {
#if MoveSequence
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

            int n, k;
            Console.WriteLine("DiskCount:");
            n = int.Parse(Console.ReadLine());
            Console.WriteLine("PegCount");
            k = int.Parse(Console.ReadLine());

            stopwatch.Start();
            var i = Molytho.TowerOfHanoi.MoveSequence.CalculateMoveSequenceAsync(k, n);
            try
            {
                Console.WriteLine(i.Result.Count);
            }
            catch(Molytho.TowerOfHanoi.TooManyMovesException e)
            {
                Console.WriteLine(e.ToString());
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed.ToString());
            Console.WriteLine(GC.GetTotalMemory(true).ToString());

            Console.WriteLine("");
            Console.WriteLine(i.Result.ToString());


            //using(var fileStream = File.Create(string.Format("{0}_{1}.txt", n, k)))
            //{
            //    using StreamWriter streamWriter = new StreamWriter(fileStream);
            //    streamWriter.Write(i.Result.ToString());
            //}
#elif Game
            Molytho.TowerOfHanoi.TowerOfHanoiGame game = new Molytho.TowerOfHanoi.TowerOfHanoiGame(5, 4);
            game.DiskMoved += move =>
            {
                Console.WriteLine(move.ToString() + "done");
            };
            game.MoveAdded += move =>
            {
                Console.WriteLine(move.ToString() + "added");
            };
            game.AddMove(0, 1);
#elif DimensionModelGraph
            Molytho.TowerOfHanoi.DimensionModelGraphProjection<object> test = new Molytho.TowerOfHanoi.DimensionModelGraphProjection<object>(3, 4);
            var i = test.GetNeighbours(new ushort[] { 1, 0, 0 });
#endif


            Console.ReadKey();
        }
    }
}
