using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FactorialParallel_1._6._2
{
    class Program
    {
        static void Main(string[] args)
        {
            
        }

        static void FactorialTask(int num)
        {
            Console.WriteLine("Factorial calculation. Press escape to cancel task");
            var cTokenSource = new CancellationTokenSource();
            var factorialTask = Task<int>.Factory.StartNew(() => 
                Factorial(num, cTokenSource.Token), cTokenSource.Token);
            cTokenSource.Token.Register(() => CancelNotification());
            if (Console.ReadKey().Key == ConsoleKey.Escape) {
                cTokenSource.Cancel();
            }
            Console.WriteLine("Factorial number = {0}", factorialTask.Result);
            Console.ReadLine();
        }

        static void FactorialContinue(int num)
        {
            Console.WriteLine("Factorial calculation. Press escape to cancel task");
            var cTokenSource = new CancellationTokenSource();
            var factorialTask = Task<int>.Factory.StartNew(() =>
                Factorial(num, cTokenSource.Token), cTokenSource.Token);
            cTokenSource.Token.Register(() => CancelNotification());
            if (Console.ReadKey().Key == ConsoleKey.Escape)
            {
                cTokenSource.Cancel();
            }
            var continuationResultTask = factorialTask.ContinueWith(f => {
                Console.WriteLine("Factorial value = {0}", f.Result);
                Console.ReadLine();
            }, 
                TaskContinuationOptions.OnlyOnRanToCompletion
            );

            var continuationCancelTask = factorialTask.ContinueWith(f => {
                Console.WriteLine("Factorial aborted");
                Console.ReadLine();
            },
                TaskContinuationOptions.NotOnRanToCompletion
            );

            try
            {
                Task.WaitAll(continuationResultTask, continuationCancelTask);
            }
            catch { }
            Console.WriteLine(continuationResultTask.IsCanceled);
            Console.WriteLine(continuationCancelTask.IsCanceled);
        }

        static async void FactorialAsync(int num)
        {
            try
            {
                Console.WriteLine("Factorial calculation. Press escape to cancel task");
                var cTokenSource = new CancellationTokenSource();
                var factorialTask = Task<int>.Factory.StartNew(() =>
                    Factorial(num, cTokenSource.Token), cTokenSource.Token);
                cTokenSource.Token.Register(() => CancelNotification());
                if (Console.ReadKey().Key == ConsoleKey.Escape)
                {
                    cTokenSource.Cancel();
                }
                await factorialTask;
                Console.WriteLine("Factorial value = {0}", factorialTask.Result);
                Console.ReadLine();
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }


        static int Factorial(int num, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            Console.WriteLine("Factorial calculation, argument = : {0}, go to sleep 500ms", num);
            if (!token.IsCancellationRequested) {
                Thread.Sleep(500);
                return (num == 0) ? 1 : num * Factorial(num - 1, token);
            }
            return 0;
        }

        static void CancelNotification()
        {
            Console.WriteLine("Cancellation request made");
        }
    }
}
