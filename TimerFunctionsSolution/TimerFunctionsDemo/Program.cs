using System;
using System.Timers;

class Program
{
    static void Main()
    {
        System.Timers.Timer timer = new System.Timers.Timer(1000);  
        timer.Elapsed += OnTimerElapsed;
        timer.Start();

        Console.ReadLine();
    }

    static void OnTimerElapsed(object s, ElapsedEventArgs e)
    {
        Console.WriteLine("System.Timers.Timer event: " + DateTime.Now);
    }
}
