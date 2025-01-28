using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorDemo
{
    internal class Static
    {
        static Static()
        {
            Console.WriteLine("i am static constructor");
        }
        public static void Main()
        {
            Console.WriteLine("i am main method");
        }
    }
   

  }
