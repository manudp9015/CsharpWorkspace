using System;


namespace Structuredemo
{
    class CodeFile1
    {

        int number;

        public CodeFile1(int number)
        {
            this.number = number;
            Console.WriteLine("value is "+ number);
        }
        public static void Main()
        {
            CodeFile1 codefile = new CodeFile1(5);


            Console.WriteLine();//parameter less constructor gav e by program implicitly
            codefile.number = 6;
            Console.WriteLine(codefile.number);


            // struct cannot support inheritance
            //struct only used for simple program
            //we cannot ctreate parametrless constructor explicitly
            //new keyword is optional

        }
    }
}