using System;


namespace IMultiLeveldemo
{
    interface IInterface1
    {
        void show();
    }
    interface IInterface2
    {

        void show();
    }
    class Multi:IInterface1,IInterface2
    {
        //1st way

        public void show()
        {
            Console.WriteLine("multilevel interface done by interface");
        }

        //second way
        void IInterface1.show()
        {
            Console.WriteLine("multilevel interface done by interface1");
        }
        void IInterface2.show()
        {
            Console.WriteLine("multilevel interface done by interface2");
        }

    }
    class Program
    {
        public static void Main()
        {
            Multi multi = new Multi();
            multi.show();

            IInterface1 interface1 = new Multi();
            interface1.show();
            IInterface2 interface2 = new Multi();   
            interface2 .show();
        }
    }
}