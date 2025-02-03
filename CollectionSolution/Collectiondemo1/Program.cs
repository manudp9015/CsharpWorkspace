using System;

using  System.Collections;

namespace Collectiondemo1
{
    class Program
    {
        
        public static void Main ()
        {
            ArrayList al = new ArrayList();
            al.Add(1);
            al.Add(2);
            al.Add(3);
            al.Add(4);
            al.Add(5);

            foreach (int i in al) 
                Console.Write(i+" ");
            Console.WriteLine();

            Console.WriteLine(al.Capacity);

            al.Remove(4);
            foreach (int i in al)
                Console.Write(i+" ");

            Console.WriteLine();


                al.Insert(3,4);
            foreach (int i in al)
                Console.Write(i+" ");  
        }
    }
}
