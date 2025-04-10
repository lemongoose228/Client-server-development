using System;
using System.Threading;

class Program
{
    static double sharedValue = 0.5; // Начальное значение 
    static Mutex mutex = new Mutex(); 

    static void CalculateCosine()
    {
        while (true)
        {
            mutex.WaitOne(); 
            double result = Math.Cos(sharedValue); // косинус
            Console.WriteLine($"Косинус {sharedValue}: {result}");
            sharedValue = result; 
            mutex.ReleaseMutex(); 

            Thread.Sleep(1000); 
        }
    }

    static void CalculateArcCosine()
    {
        while (true)
        {
            mutex.WaitOne(); 
            double result = Math.Acos(sharedValue); // арккосинус
            Console.WriteLine($"Арккосинус {sharedValue}: {result}");
            sharedValue = result; 
            mutex.ReleaseMutex(); 

            Thread.Sleep(1000); 
        }
    }

    static void Main(string[] args)
    {
        Thread cosineThread = new Thread(CalculateCosine);
        Thread arcCosineThread = new Thread(CalculateArcCosine);

        cosineThread.Start(); 
        arcCosineThread.Start(); 

        cosineThread.Join(); 
        arcCosineThread.Join(); 
    }
}
