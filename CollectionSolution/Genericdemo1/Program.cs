using System;

using System.Collections.Generic;


namespace Genericdemo1
{
    class Program
    {
        public static void Main()
        {
            List<int> list = new List<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(4);
            list.Add(5);

            foreach (int i in list)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine();

            list.RemoveAt(3);

            foreach (int i in list)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine();

            list.Insert(3, 4);

            foreach (int i in list)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine();

        }
    }
}