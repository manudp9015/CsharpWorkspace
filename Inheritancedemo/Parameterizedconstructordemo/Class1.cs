using System;

namespace Parameterizedconstructordemo
{
    class Class1
    {
       public Class1(int i)
        {
            Console.WriteLine(i);
        }
        
    }
    class Class2:Class1
    {
        Class2(int a):base(a)
        {
           
        }
        public static void Main()
        {
            Class2 c2 = new Class2(10);

          
        }
    }

}