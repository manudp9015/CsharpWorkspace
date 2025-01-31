using System;

namespace Extensiondemo
{
    class Program
    {
        public void Test1()
        {
            Console.WriteLine("method 1");

        }
        public void Test2()
        {

            Console.WriteLine("method 2");
        }
        public static void Main()
        {
            Program p = new Program();
            p.Test1();
            p.Test2();
           
        }
    }
}
