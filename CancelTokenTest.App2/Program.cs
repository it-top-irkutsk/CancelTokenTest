using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CancelTokenTest.Lib;

namespace CancelTokenTest.App2
{
    class Program
    {
        static void Main(string[] args)
        {
            using var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            var calc = new Calc();
            calc.Log = Console.WriteLine;
            token.Register(() => Console.WriteLine("Отмена!!!"));
            var tasks = new List<Task>();

            for (int i = 1; i < 10; i++)
            {
                var x = i;
                tasks.Add(new Task(() => calc.FactTask2(x, token), token));
            }

            try
            {
                foreach (var task in tasks)
                {
                    task.Start();
                }
                
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Spacebar)
                {
                    tokenSource.Cancel();
                }
                
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException ae)
            {
                foreach (var e in ae.InnerExceptions)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}