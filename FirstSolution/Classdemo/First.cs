using System;

namespace Classdemo
{
    class First
    {
        int x = 100;

        public static void Main()
        {
            First f1;//just defining  class variable ;
            f1= new First();// initializing variable f with instance of class using new keyward;
            First f2=f1;
          
            Console.WriteLine(f1.x);
            Console.WriteLine(f2.x);
        }
    }
   
}
