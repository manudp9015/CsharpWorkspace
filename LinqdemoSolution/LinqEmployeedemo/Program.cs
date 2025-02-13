using System;

namespace LinqEmployeedemo
{
    class Employee
    {
        public string name { get; set; }
        public int age { get; set; }
        public  double salary { get; set; }
    }
    class Program
    {
        public static void Main()
        {
            List<Employee> list = new List<Employee> {
                                                        new Employee{ name="manu",age=21,salary=50000 },
                                                        new Employee{ name="rohith",age=20,salary=50000 },
                                                        new Employee{ name="roja",age=23,salary=500000 },
                                                       };
            var result = list.Where(l => l.salary > 50000);

            foreach ( Employee e in result )
            {
                Console.WriteLine($"name:{e.name} age:{e.age} salary:{e.salary}");
            }
          
        }
    }

}

