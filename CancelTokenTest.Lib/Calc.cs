using System;
using System.Threading;

namespace CancelTokenTest.Lib
{
    public class Calc
    {
        public Action<string> Log;
        public long FactTask(int value, CancellationToken cancel)
        {
            Log?.Invoke($"Start {value}!");
            if (cancel.IsCancellationRequested)
            {
                Log?.Invoke($"Stop {value}!");
                return -1;
            }
            
            switch (value)
            {
                case 0:
                case 1:
                    Log?.Invoke($"End factorial");
                    return 1;
                default:
                    Log?.Invoke($"Start new factorial");
                    Thread.Sleep(5000);
                    return value * FactTask(value - 1, cancel);
            }
        }
        
        public long FactTask2(int value, CancellationToken cancel)
        {
            Log?.Invoke($"Start {value}!");
            //cancel.ThrowIfCancellationRequested();
            if (cancel.IsCancellationRequested)
            {
                Log?.Invoke($"Stop {value}!");
                cancel.ThrowIfCancellationRequested();
            }
            
            switch (value)
            {
                case 0:
                case 1:
                    Log?.Invoke($"End factorial");
                    return 1;
                default:
                    Log?.Invoke($"Start new factorial");
                    Thread.Sleep(2000);
                    return value * FactTask2(value - 1, cancel);
            }
        }
    }
}