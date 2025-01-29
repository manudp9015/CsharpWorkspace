using System;


namespace Overloding
{
    class Program
    {
        public void Test1()
        {
            Console.WriteLine("method1");
        }
        public void Test2(int i)
        {
            Console.WriteLine("method1"+i);
        }
        public void Test3(string s)
        {
            Console.WriteLine("method1" + s);
        }
        public void Test4(string s,int i)
        {
            Console.WriteLine("method1" + s,i);
        }
        public void Test5(int i,string s)
        {
            Console.WriteLine("method1" + i,s);
        }
        public static void Main()
        {
            Program p=new Program();
            p.Test1();
            p.Test2(10);
            p.Test3("manu");
            p.Test4("manu", 10);
            p.Test5(10, "manu");

        }

    }
}
