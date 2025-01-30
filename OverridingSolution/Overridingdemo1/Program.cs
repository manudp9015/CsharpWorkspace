using System;

namespace Overriding
{
    class ParentClass
    {
        public virtual void show ()
        {
            Console.WriteLine("parent method");
        }

      
    }
    class ChildClass:ParentClass
    {
        public override void show()
        {
            Console.WriteLine("child overriding parent method");
        }
        public static void Main()
        {
            ParentClass parent = new ParentClass();
            parent.show();

            ChildClass child = new ChildClass();
            child.show();
        }
    }
}