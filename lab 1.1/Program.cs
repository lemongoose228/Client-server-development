using System;
using System.Threading;

internal class Program
{
    private static void Main(string[] args)
    {
        ThreadStart thread1Start = () => PrintNumbersInRange(0, 10);
        ThreadStart thread2Start = () => PrintNumbersInRange(100, 110);

        Thread thread1 = new Thread(thread1Start);
        Thread thread2 = new Thread(thread2Start);

        thread1.Start();
        thread2.Start();

        thread1.Join();
        thread2.Join();

        Console.WriteLine("Оба потока завершили выполнение");
    }

    private static void PrintNumbersInRange(int start, int end)
    {
        Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} начинает вывод чисел от {start} до {end}");
        for (int i = start; i <= end; i++)
        {
            Console.WriteLine(i);
            Thread.Sleep(100); //задержка
        }
        Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} завершил вывод чисел от {start} до {end}");
    }
}
