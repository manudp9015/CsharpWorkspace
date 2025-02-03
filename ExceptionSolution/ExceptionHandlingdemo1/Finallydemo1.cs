using System;


namespace Exceptiondemo1

{
    class Finallydemo1
    {

        public static void Main()
        {
            try
            {
                Console.Write("enter first number: ");
                int first = int.Parse(Console.ReadLine());

                Console.Write("enter second number: ");
                int second = int.Parse(Console.ReadLine());

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
