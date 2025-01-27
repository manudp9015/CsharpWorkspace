using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorDemo
{
    internal class Parameterized
    {
        int x;
        Parameterized(int i)
        {
            x = i;
        }
        void display()
        {
            Console.WriteLine(x);
        }
        static void Main(string[] args)
        {
            Parameterized p = new Parameterized(10);
            p.display();


        }
    }
}
