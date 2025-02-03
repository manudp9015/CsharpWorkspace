



using System;
using System.Diagnostics;
using System.Text;

namespace StringBuilderdemo
{
    class program
    {

        

        public static void Main()
        {
            program p=new program();

            string s = "";

            Stopwatch sw1 = new Stopwatch();
            sw1.Start();
            for(int i=0;i<100000;i++)
            {
                s += i;
            }
            sw1.Stop();


            Stopwatch sw2 = new Stopwatch();
            StringBuilder sb = new StringBuilder();
            sw2.Start();
            for (int i = 0; i < 100000; i++)
            {
                sb.Append(i);            
            }
            sw2.Stop();


            Console.WriteLine("time taken by string to add 100000 values is: "+sw1.ElapsedMilliseconds);
            Console.WriteLine("time taken by string builder to append 100000 values is: "+sw2.ElapsedMilliseconds);

        }

    }
}