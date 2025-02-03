using System;
using System.Security.Cryptography.X509Certificates;


namespace Exceptiondemo1

{
    class Throw1
    {
        public class DivideBYOddNoException : ApplicationException
        {
            public override string Message
            {
                get
                {
                    return "Attempted to Divide By Odd Number";
                }
            }
        }

        public static void Main()
        {


        
            try
            {
                Console.Write("enter first number: ");
                int first = int.Parse(Console.ReadLine());

                Console.Write("enter second number: ");
                int second = int.Parse(Console.ReadLine());

                if(second%2==1)
                {
                    //throw new Exception("can not divide a first by using odd number");
                   throw new  DivideBYOddNoException(); 
                }
                int z = first / second;
                Console.WriteLine("The Value Of z is: " + z);
            }
            catch (DivideByZeroException e1)
            {
                Console.WriteLine(e1.Message);
            }
            catch (FormatException e2)
            {
                Console.WriteLine(e2.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.WriteLine("finally block executed.");
            }

            Console.WriteLine("The End of the Program.");

            Console.ReadLine();

        }
    }

}
