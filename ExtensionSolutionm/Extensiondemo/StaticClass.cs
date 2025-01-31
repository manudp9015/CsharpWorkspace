using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensiondemo
{
    internal static class StaticClass
    {
        public static void Test3(this Program p)
        {
            Console.WriteLine("method 3");
        }
        public static void Main()
        {
            Program p=new Program();
            p.Test1();
            p.Test2();
            p.Test3();  
        }

    }
}
