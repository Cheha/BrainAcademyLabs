using System;
using System.Threading;

namespace ThreadManipulatoring
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            var manipulator = new ThreadManipulator();

            var addOneThr1 = new Thread(manipulator.AddingOne);
            var addOneThr2 = new Thread(manipulator.AddingOne);

            var addCustThr = new Thread(manipulator.AddingCustomValue);

            var stopThr = new Thread(manipulator.Stop) { IsBackground = true };

            stopThr.Start();

            addOneThr1.Start(10);
            addOneThr2.Start(20);
            // addCustThr.Start( new[]{15, 5} );
            addCustThr.Start(new[] { 5, 15 });
            addOneThr1.Join();
            addOneThr2.Join();
            addCustThr.Join();

            Console.ReadKey();

        }
    }
}
