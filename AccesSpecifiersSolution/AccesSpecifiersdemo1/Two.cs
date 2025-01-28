using AccesSpecifiesdemo1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesSpecifiersdemo1
{
    internal class Two: First
    {

        public static void Main()
        {
           Two f1 = new Two();

           // f1.Test1(); its not possible because it is private acces specifier
            f1.Test2(); f1.Test3(); f1.Test4(); f1.Test5();

        }
    }
}
