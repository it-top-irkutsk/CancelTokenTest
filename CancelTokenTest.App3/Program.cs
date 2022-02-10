using System;
using System.Threading;
using System.Threading.Tasks;
using CancelTokenTest.Lib;

namespace CancelTokenTest.App3
{
    class Program
    {
        static void Main(string[] args)
        {
            var calc = new Calc();
            calc.Log = (msg) =>
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(msg);
                Console.ResetColor();
            };

            var tasks = new Task[10];
            var cts = new CancellationTokenSource[10];
            var ct = new CancellationToken[10];
            for (int i = 0; i < 10; i++)
            {
                var x = i;
                
                cts[x] = new CancellationTokenSource();
                ct[x] = cts[x].Token;
                ct[x].Register(() =>
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Отмена задачи #{x}");
                    Console.ResetColor();
                });
                
                tasks[x] = new Task(() => calc.FactTask2(x, ct[x]), ct[x]);
            }

            try
            {
                for (int i = 0; i < 10; i++)
                {
                    tasks[i].Start();
                }

                var num = Convert.ToInt32(Console.ReadLine());
                cts[num].Cancel();
                
                Task.WaitAll(tasks);
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