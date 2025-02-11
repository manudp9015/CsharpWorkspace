using System;

namespace ListCopyDemo1
{
    class Program2
    {
        class Person
        {
            public String Name { get; set; }
            public Person(String name)
            {
                Name = name;
            }
            public Person clone()
            {
               return new Person(this.Name);
            }
        }

        public static void Main()
        {
            Person p = new Person("manu");
            Person p1 = new Person("rohith");
            Person p2 = new Person("roja");
            List<Person> original = new List<Person>();
            original.Add(p);
            original.Add(p2);
            original.Add(p1);


            List<Person> deepCopy=new List<Person>();
            foreach(Person person in original)
            {
                deepCopy.Add(person.clone());
            }

            deepCopy[0].Name = "deekshith";

            //using clone method make it diffrent object creation so change in one not affect another here
            Console.WriteLine(deepCopy[0].Name);
            Console.WriteLine(original[0].Name);
        }
    }
}