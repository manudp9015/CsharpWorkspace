using System;

using System.Collections;

namespace IComparabledemo
{
    class Student:IComparable<Student>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int clas { get; set; }
        public int marks { get; set; }

        public int CompareTo(Student other)
        {
            if(this.Id < other.Id) return -1;
            else if(this.Id > other.Id) return 1;
            else return 0;
        }
    }
    class StudentTest
    {
        public static void Main()
        {
            Student s1 = new Student{Id = 1, Name = "manu",clas=10,marks=544};
            Student s2 = new Student { Id = 2, Name = "rohith", clas = 10, marks = 555 };
            Student s3 = new Student { Id = 3, Name = "bharath", clas = 10, marks = 444 };
            Student s4 = new Student { Id = 4, Name = "sagar", clas = 10, marks = 500 };

            List<Student> list = new List<Student>(){ s1, s3, s2, s4 };

            list.Sort();

            foreach(Student student in list)
            {
                Console.WriteLine(+student.Id + " "+student.Name+" "+student.clas+" "+student.marks);
            }





        }
    }
}


