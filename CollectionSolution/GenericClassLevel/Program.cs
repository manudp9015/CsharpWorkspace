using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genericdemo1
{
    public class GenericClassLevel<T>
    {
        public void add(T a, T b)
        {
            dynamic d1 = a;
            dynamic d2 = b;
            Console.WriteLine("addition value is: " + (d1 + d2));
        }
        public void sub(T a, T b)
        {
            dynamic d1 = a;
            dynamic d2 = b;
            Console.WriteLine("subtraction value is: " + (d1 - d2));
        }
        public void mul(T a, T b)
        {
            dynamic d1 = a;
            dynamic d2 = b;
            Console.WriteLine("multiplication value is: " + (d1 * d2));
        }
        public void div(T a, T b)
        {
            dynamic d1 = a;
            dynamic d2 = b;
            Console.WriteLine("division value is: " + (d1 / d2));
        }


    }
    class generic
    {
        public static void Main()
        {
            GenericClassLevel<int> lt = new GenericClassLevel<int>();
            lt.add(10, 20);
            lt.sub(20, 30);
            lt.mul(20, 30);
            lt.div(30, 30);
        }
    }
}
