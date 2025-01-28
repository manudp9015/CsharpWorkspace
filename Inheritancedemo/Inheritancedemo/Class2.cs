
using System;


namespace Inheritancedemo
{

    class class2:class1
    {
        
        public void method2()
        {
            Console.WriteLine("method 2 from class2");
        }
        public static void Main()
        {
            class2 o = new class2();
            o.method2();
            o.method1();

            class1 o1 = new class2();//we initialize the or use parent reference to call parent method;
            o1.method1();

        }
    }
}

