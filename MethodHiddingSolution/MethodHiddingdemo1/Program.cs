using System;

namespace MethodHiddingdemo1
{
    class ParentClass
    {
        public virtual void test1()
        {
            Console.WriteLine("parent method test1");
        }
        public void test2()
        {
            Console.WriteLine("parent method test2");
        }
    }
    class ChildClass:ParentClass
    {
        public override void test1()
        {
            Console.WriteLine("child  method test1 overriding parent");//parent class method overriding happrning here
        }
        public new void test2()//parentclass method hidding happening here
        {
            Console.WriteLine("parent method test2 hidding parent");
        }
        public static void Main()
        {
            ParentClass parent = new ParentClass();
            parent.test1();
            parent.test2();

            ChildClass child = new ChildClass();
            child.test1();
            child.test2();

            ParentClass parent2 = new ChildClass();
            parent2.test1();//but it call child overriden method because it have references here
            parent2.test2();//it just call parent test2 method


        }
    }
    }