using System;

namespace Extensiondemo
{
   static class Program
    {
        public static int Factorial(this Int32 x)
        {
            int result = 1;
            for (int i = x; i > 1; i--)
            {
                result *= i;
            }
            return result;

        }
        public static string ToProperCase(this string x)
        {
            string[] words = x.Split(' ');
            string newstr = "";

            foreach (string word in words)
            {
                if (word.Length > 0)
                {
                    char[] ch = word.ToLower().ToCharArray();
                    ch[0] = char.ToUpper(ch[0]);
                    newstr += new string(ch) + " ";
                }
            }

            return newstr.Trim();
        }
        public static void Main()
        {
            int Number = 5;
            int fact = Number.Factorial();
            Console.WriteLine(fact);

            string str = "i Am maNu dP";
           str= str.ToProperCase();
            Console.WriteLine(str);
        }
    }
}

