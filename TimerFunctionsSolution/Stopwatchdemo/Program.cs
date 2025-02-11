using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

class Program
{
    static void Main()
    {
        Stopwatch stopwatch = new Stopwatch();

        stopwatch.Start();
        TaskToMeasure();  
        stopwatch.Stop();

        Console.WriteLine("Elapsed Time: " + stopwatch.ElapsedMilliseconds + " ms");
    }

    static void TaskToMeasure()
    {
        int a = 0;
        for (int i = 0; i < 10000000; i++)
        {
            a++; 
        }
    }
}
