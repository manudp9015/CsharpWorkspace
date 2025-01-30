using System;

namespace Interfacedemo1
{

    interface IInterface1
    {
        void Add(int a, int b);
    }
    interface IInterface2:IInterface1
    {
        void Sub(int a, int b); 
    }
    class Program:IInterface2
    {


        public static void Main()
        {
            Program program = new Program();
            program.Sub(20, 10);
            program.Add(20, 20);

        }

        public void Add(int a, int b)
        {
           Console.WriteLine("Addition: "+ (a+b));
        }

        public void Sub(int a, int b)
        {
            Console.WriteLine("Subtraction: "+  (a-b));
        }
    }
}
