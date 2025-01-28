using System;

namespace AccesSpecifiesdemo1
{
    public  class First
    {
        private void Test1()
        {
            Console.WriteLine("i am private accesspecifier");
        }
        public  void Test2()
        {
            Console.WriteLine("i am public accesspecifier");
        }
        internal void Test3()
        {
            Console.WriteLine("i am internal accesspecifier");
        }
        protected void Test4()
        {
            Console.WriteLine("i am protected accesspecifier");
        }
        protected internal void Test5()
        {
            Console.WriteLine("i am protected internal accesspecifier");
        }

        public static void Main()
        {
            First f1= new First();
           
            f1.Test1(); f1.Test2(); f1.Test3(); f1.Test4(); f1.Test5();

        }
    }
}
