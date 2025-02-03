using System;
using System.Collections;
using System.Collections.Generic;

namespace IEnumerabledemo
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    class StudentDetails : IEnumerable<Student>
    {
        private List<Student> students;

        public StudentDetails()
        {
            students = new List<Student>()
            {
                new Student() { Name = "manu", Id = 1 },
                new Student() { Name = "rohi", Id = 2 }
            };
        }

        public IEnumerator<Student> GetEnumerator()
        {
            foreach (var student in students)
            {
                yield return student; 
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    class Program
    {
        public static void Main()
        {
            StudentDetails studentList = new StudentDetails();

            foreach (var student in studentList)
            {
                Console.WriteLine($"ID: {student.Id}, Name: {student.Name}");
            }
        }
    }
}
