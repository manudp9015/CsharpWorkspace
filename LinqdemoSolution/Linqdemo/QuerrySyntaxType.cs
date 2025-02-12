using System;

namespace Linqdemo
{
    class QuerrySyntaxType
    {
        public static void Main()
        {
            int[] arr = { 1, 4, 2, 7, 4, 6, 3, 0, 7, 8 };

            var result = from i in  arr
                         where i < 5 
                         select i;

            foreach (int n in result)
            {
                Console.WriteLine(n);
            }



        }
    }
}