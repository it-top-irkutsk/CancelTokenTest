using System;
using System.Threading;
using System.Threading.Tasks;
using CancelTokenTest.Lib;

namespace CancelTokenTest.App
{
    class Program
    {
        static void Main()
        {
            using var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            var calc = new Calc();
            calc.Log = Console.WriteLine;

            for (int i = 1; i < 10; i++)
            {
                var x = i;
                Task.Run(() => calc.FactTask(x, token), token);
            }

            var key = Console.ReadKey();
            if (key.Key == ConsoleKey.Spacebar)
            {
                tokenSource.Cancel();
            }

            Console.ReadKey();
        }
    }
}