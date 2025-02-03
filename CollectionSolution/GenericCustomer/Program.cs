using System;


namespace GenericCustomer

{
    class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string job { get; set; }

        public string City { get; set; }
    }

    class Program
    {

        public static void Main()
        {
            List<Customer> list = new List<Customer>();
            Customer l1 = new Customer { Id = 101, Name = "manu", job = "developer", City = "hassan" };
            list.Add(l1);

            Customer l2 = new Customer { Id = 102, Name = "rohith", job = "full stack developer", City = "belur" };
            list.Add(l2);

            foreach(Customer obj in list)
            {
                Console.WriteLine(obj.Id + " " + obj.Name +" "+ obj.job+" "+obj.City);
            }
            Console.ReadLine();
        }
    }
}