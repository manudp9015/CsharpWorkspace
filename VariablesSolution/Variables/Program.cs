using System;
using System.Runtime.InteropServices.Marshalling;

namespace Variables
{

    class Program
    {
        static int id = 10;//only one copy can assign new value;
        const float pi = 3.14f;//fix value cannot change happen
        readonly bool ans;
        int val = 100;

       public  Program(bool v)
        {
            ans= v;
        }
        public static void Main()
        {
            Program p1 = new Program(false);
            Program.id = 20;
            Console.WriteLine(p1.val);
            Console.WriteLine(id);
            Console.WriteLine(p1.ans);
            Program p2 = new Program(true);
            Console.WriteLine(p2.ans);//  can assign new value for each new instance




        }
    }
}
