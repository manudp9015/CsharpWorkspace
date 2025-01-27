using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorDemo
{
    internal class Copy
    {
        int x;
        public Copy(int i)
        {
            x = i;
        }
       public Copy(Copy o)
        {
             x=o.x;    
        }
        void display()
        {
            Console.WriteLine(x);
        }
        static void Main(string[] args)
        {
            Copy c1 = new Copy(20);
            c1.display();
            Copy c2 = new Copy(c1);
            c2.display();


        }
    }
}
