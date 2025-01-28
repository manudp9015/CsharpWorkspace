using System;

namespace AccesSpecifierdemo2
{
    public class  Four:AccesSpecifiesdemo1.First
    {
        public static void Main()
        {
            Four f1 = new Four();

            // private,internal are not possible here
            f1.Test2(); f1.Test4(); f1.Test5();
        }
    }
}
