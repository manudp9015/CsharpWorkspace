using System;

namespace ListCopyDemo1
{
    class Program1
    {
        class Person
        {
            public String Name { get; set;}
            public Person(String name)
            {
                Name = name;
            }
        }

        public static void Main()
        {
            Person p = new Person("manu");
            Person p1 = new Person("rohith");
            Person p2 = new Person("roja");
            List<Person> original = new List<Person> ();
            original.Add(p);
            original.Add(p2);
            original.Add(p1);


            List<Person>shallowCopy=original.ToList();
         
           //Change made in Shallowcopy reflect in original list since because it is *Shallow copy*using ToList
            shallowCopy[0].Name = "deekshith";

            Console.WriteLine(shallowCopy[0].Name);
            Console.WriteLine(original[0].Name);
        }
    }
}