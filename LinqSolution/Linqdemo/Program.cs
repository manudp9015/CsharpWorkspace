using System;

using System.Linq;


namespace Linqdemo
{
    class Program
    {
        public static void Main()
        {
            int [] arr = { 10, 67, 12, 56, 93, 47, 27, 48, 95, 8, 3, 38 };
            var brr=from i in arr where i > 40 select i;

            foreach(var i in brr)
            {
                Console.Write(i+" ");
            }
            Console.ReadLine(); 
        }
    }
}