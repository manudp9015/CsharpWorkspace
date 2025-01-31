using Indexerdemo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indexersdemo
{
    internal class TestEmployee
    {

        public static void Main()
        {
            Employee emp=new Employee(1,"manu","datalyzer","developer",10000, "jayanager");


            Console.WriteLine("employee eno: " + emp[0]);
            Console.WriteLine("employee ename: " + emp[1]);
            Console.WriteLine("employee dname: " + emp[2]);
            Console.WriteLine("employee job: " + emp[3]);
            Console.WriteLine("employee sal: " + emp[4]);
            Console.WriteLine("employee location: " +emp[5]);

            Console.WriteLine();


            emp[0] = 1;
            emp[2] = "data science";

            Console.WriteLine("employee eno: " + emp[0]);
            Console.WriteLine("employee ename: " + emp[1]);
            Console.WriteLine("employee dname: " + emp[2]);
            Console.WriteLine("employee job: " + emp[3]);
            Console.WriteLine("employee sal: " + emp[4]);
            Console.WriteLine("employee location: " + emp[5]);

        }


    }
   
}
