using AccesSpecifiesdemo1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesSpecifiersdemo1
{
    internal class Three
    {
        
        public static void Main()
        {
            First f = new First();

            f.Test3();
            f.Test5();
            f.Test2();
          //private and protected are not possible
            

        }
    }
}
