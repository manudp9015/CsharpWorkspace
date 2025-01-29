using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitydemo1
{
    internal class Teaching :Staff
    {
        string qualification;
        string subject;

        Teaching( String destination,double salary, string qualification,string subject)
        {
            this.destination = destination;
            this.salary = salary;
            this.qualification = qualification;
            this.subject = subject;
        }
        public void display()
        {
            Console.WriteLine("destination is " + destination);
            Console.WriteLine("salary is " + salary);
            Console.WriteLine("qualification is " + qualification);
            Console.WriteLine("subject is " + subject);
        }
        public static void Main()
        {
            Teaching s = new Teaching("abc",100000,"b e","java");
            s.display();
        }

    }
}
