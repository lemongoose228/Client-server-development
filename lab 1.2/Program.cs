using System;
using System.Threading;

internal class Program
{
    private static ManualResetEvent firstThreadCompleted = new ManualResetEvent(false);

    private static void OutputNumbers()
    {
        for (int i = 1; i <= 100; i++)
        {
            Console.WriteLine(i);
        }
        firstThreadCompleted.Set(); 
    }

    private static void OutputNumbersWithDelay()
    {
        firstThreadCompleted.WaitOne(); 
        for (int i = 1; i <= 100; i++)
        {
            Console.WriteLine(i);
        }
    }

    private static void Main()
    {
        Thread firstThread = new Thread(OutputNumbers);
        firstThread.Start();

        Thread.Sleep(1000); // Задержка в 1 секунду

        Thread secondThread = new Thread(OutputNumbersWithDelay);
        secondThread.Start();

        firstThread.Join();
        secondThread.Join();
    }
}
