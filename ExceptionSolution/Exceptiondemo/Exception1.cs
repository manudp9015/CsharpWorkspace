using System;


namespace Exceptiondemo1

{
    class Exception1
    {

        public static void Main()
        {
            Console.Write("enter first number: ");
            int first=int.Parse(Console.ReadLine());

            Console.Write("enter second number: ");
            int second = int.Parse(Console.ReadLine());

            int z = first / second;
            Console.WriteLine("The Value Of z is: " + z);
            Console.WriteLine("The End of the Program.");

            Console.ReadLine();

        }
    }

}
