﻿using System;
using System.IO;
using System.Threading;

namespace TestConsole
{
    class Program
    {
        static void Main()
        {
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

            Console.ReadKey();
        }
    }
}