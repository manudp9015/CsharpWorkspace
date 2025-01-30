
using System;


namespace Enumerationdemo1
{


    class Program
    {
        public enum  Days
        {
            Monday,Tuesday,Wednesday,Thursday,Friday
        }

        Days meetingDate { get; set; } = (Days)4;

        public void Dispaly()
        {
            Console.WriteLine("meetingday is: " + meetingDate);
        }
        public static void Main()
        {
            Program program = new Program();
            program.Dispaly();
        }
    }
}