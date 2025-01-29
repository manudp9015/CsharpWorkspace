using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitydemo1
{
    internal class NonTeaching : Staff
    {
        string dname;
        int mgrid;
        NonTeaching(String destination, double salary, string dname, int mgrid)
        {
            this.destination = destination;
            this.salary = salary;
            this.dname = dname;
            this.mgrid = mgrid;
        }
        public void display()
        {
            Console.WriteLine("destination is " + destination);
            Console.WriteLine("salary is " + salary);
            Console.WriteLine("dname is " + dname);
            Console.WriteLine("mgrid is " + mgrid);
        }
        
        
        public static void Main()
        {
            NonTeaching s = new NonTeaching("abcd",1200,"cs",12);
            s.display();
        }

    }
}
