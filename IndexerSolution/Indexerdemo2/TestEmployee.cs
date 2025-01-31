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
            Employee emp = new Employee(1, "manu", "datalyzer", "developer", 10000, "jayanager");


            Console.WriteLine("employee eno: " + emp["eno"]);
            Console.WriteLine("employee ename: " + emp["ename"]);
            Console.WriteLine("employee dname: " + emp["dname"]);
            Console.WriteLine("employee job: " + emp["job"]);
            Console.WriteLine("employee sal: " + emp["sal"]);
            Console.WriteLine("employee location: " + emp["location"]);

            Console.WriteLine();


            emp["eno"] = 1;
            emp["dname"] = "data science";


            Console.WriteLine("employee eno: " + emp["eno"]);
            Console.WriteLine("employee ename: " + emp["ename"]);
            Console.WriteLine("employee dname: " + emp["dname"]);
            Console.WriteLine("employee job: " + emp["job"]);
            Console.WriteLine("employee sal: " + emp["sal"]);
            Console.WriteLine("employee location: " + emp["location"]);
        }


    }

}
