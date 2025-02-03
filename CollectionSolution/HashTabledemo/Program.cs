using System;
using System.Collections;

namespace HashTabledemo
{
    class Program
    {
        public static void Main()
        {
            Hashtable ht = new Hashtable();
            ht.Add("name", "manu");
            ht.Add("age", 21);
            ht.Add("job", "developer");
            ht.Add("company", "datalyzer");

            foreach(object  i in ht.Keys)
            {
                Console.WriteLine(i+" "+ ht[i]);
            }
            Console.WriteLine();


            ht.Remove("company");
            foreach (object i in ht.Keys)
            {
                Console.WriteLine(i + " " + ht[i]);
            }
            Console.WriteLine();



        }
    }
}