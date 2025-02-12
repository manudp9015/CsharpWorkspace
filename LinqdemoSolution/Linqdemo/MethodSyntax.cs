using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linqdemo
{
    internal class MethodSyntax
    {

        public static void Main()
        {
            int[] arr = { 1, 4, 2, 7, 4, 6, 3, 0, 7, 8 };

            var result = arr.Where(e => e > 5);

            foreach (int n in result)
            {
                Console.WriteLine(n);
            }
        }
    }
}
