using System;
using System.Threading;

namespace ThreadManipulatoring
{
    class ThreadManipulator
    {
        private ConsoleKey _key;

        //In general, avoid locking on a public type, or instances beyond your code's control. The common constructs lock (this), lock (typeof (MyType)), and lock ("myLock") violate this guideline:
        //lock (this) is a problem if the instance can be accessed publicly.
        //lock (typeof (MyType)) is a problem if MyType is publicly accessible.
        //lock("myLock") is a problem because any other code in the process using the same string, will share the same lock.
        //Best practice is to define a private object to lock on, or a private static object variable to protect data common to all instances.

        private static readonly object _block = new object();    //lock

        //private object _block = new object();    //lock

        public void AddingOne(object raw)
        {
            var number = (int)raw;
            int ThreadId = Thread.CurrentThread.ManagedThreadId;
            lock (_block)
            {

                for (var counter = 0; counter < 100 && _key != ConsoleKey.Q; ++counter)
                {
                    if (counter % number == 0)
                    {
                        var str = string.Format("{0} is divisible by {1}. AddingOne, ManagedThreadId = {2}", counter, number, ThreadId);
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine(str);

                        Thread.Sleep(500);
                    }
                }
            }
        }



        public void AddingCustomValue(object args)
        {
            int ThreadId = Thread.CurrentThread.ManagedThreadId;
            var arr = (int[])args;
            var number = arr[0];
            var step = arr[1];

            for (var counter = 0; counter < 100 * step && _key != ConsoleKey.W; counter += step)
            {
                if (counter % number != 0) continue;

                var str = string.Format("{0} is divisible by {1}. AddingCustomValue, ManagedThreadId = {2}", counter, number, ThreadId);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(str);
                Thread.Sleep(500);
            }

        }


        public void Stop()
        {
            int ThreadId = Thread.CurrentThread.ManagedThreadId;
            while (true)
            {
                _key = Console.ReadKey().Key;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Stop, ManagedThreadId = {0}", ThreadId);
            }
        }
    }
}
