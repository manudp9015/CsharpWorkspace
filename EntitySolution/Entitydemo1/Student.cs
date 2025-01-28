using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitydemo1
{
    internal class Student : Person
    {
        int clas;
        char grade;
        float marks;
        float fees;
        public Student(  int id,string name,string address,int phone, int clas,char grade,float marks,float fees)
        {
            this .id = id;  

            this .name = name;  
            this .address = address;    
            this .phone = phone;

            this.clas = clas;
            this.grade = grade;
            this.marks = marks;
            this.fees = fees;
        }
        public void display()
        {
            Console.WriteLine("id is " + id);
            Console.WriteLine("name is "+name);
            Console.WriteLine("address is "+address);
            Console.WriteLine("phone is "+phone);
            Console.WriteLine("class is "+clas);
            Console.WriteLine(" grade is "+grade);
            Console.WriteLine(" marks is "+marks);
            Console.WriteLine("fees is "+fees);
        }

        public static void Main()
        {
            Student s = new Student(1, "John Doe", "123 Main St", 1234567890, 10, 'A', 85.5f, 5000f);
            s.display();
        }
    }
}
