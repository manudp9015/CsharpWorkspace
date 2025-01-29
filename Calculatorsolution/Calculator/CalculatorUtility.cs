using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    internal class CalculatorUtility
    {

        //method which perform basic math operation
        public void Calci() //give meaningful name
        {
            Console.WriteLine();
            Console.WriteLine("Basic Math Operations");
            Console.WriteLine("Enter 1: Addition");
            Console.WriteLine("Enter 2: Subtraction");
            Console.WriteLine("Enter 3: Multiplication");
            Console.WriteLine("Enter 4: Division");
            Console.WriteLine();
            try
            {
                Console.Write("Enter your Choice ");
                if (int.TryParse(Console.ReadLine(), out int operation))
                {
                    switch (operation)
                    {
                        case 1: Addition(); break;

                        case 2: Subtraction(); break;

                        case 3: Multiplication(); break;

                        case 4: Division(); break;
                                
                        default: Console.WriteLine("invalid input..."); break;
                    }
                }
                else{ Console.WriteLine("invalid input give integer input"); }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void Addition() //allow decimal
        {
            try
            {
                Console.Write("Enter First Number: ");
                if (int.TryParse(Console.ReadLine(), out int first))
                {
                    Console.Write("Enter Second Number: ");
                    if (int.TryParse(Console.ReadLine(), out int second))
                    {
                        Console.WriteLine("Addition: " + (first + second)); // Fixed from subtraction
                    }
                    else
                    {
                        Console.WriteLine("Please provide a valid integer for the second number.");
                    }
                }
                else
                {
                    Console.WriteLine("Please provide a valid integer for the first number.");
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public void Subtraction()//allow decimal
        {
            try
            {
                Console.Write("Enter First Number: ");
                if (int.TryParse(Console.ReadLine(), out int first))
                {
                    Console.Write("Enter Second Number: ");
                    if (int.TryParse(Console.ReadLine(), out int second))
                    {
                        Console.WriteLine("Subtraction: " + (first - second)); // Fixed from subtraction
                    }
                    else
                    {
                        Console.WriteLine("Please provide a valid integer for the second number.");
                    }
                }
                else
                {
                    Console.WriteLine("Please provide a valid integer for the first number.");
                }
            }
            catch (Exception)
            {

                throw;
            }



        }
        public void Multiplication()//allow decimal
        {
            try
            {

                Console.Write("Enter First Number: ");
                if (int.TryParse(Console.ReadLine(), out int first))
                {
                    Console.Write("Enter Second Number: ");
                    if (int.TryParse(Console.ReadLine(), out int second))
                    {
                        Console.WriteLine("Multiplication: " + (first * second)); // Fixed from subtraction
                    }
                    else
                    {
                        Console.WriteLine("Please provide a valid integer for the second number.");
                    }
                }
                else
                {
                    Console.WriteLine("Please provide a valid integer for the first number.");
                }
            }
            catch (Exception)
            {

                throw;
            }


        }
        public void Division()//allow decimal
        {

            try
            {

                Console.Write("Enter First Number: ");
                if (int.TryParse(Console.ReadLine(), out int first))
                {
                    Console.Write("Enter Second Number: ");
                    if (int.TryParse(Console.ReadLine(), out int second))
                    {
                        if (second == 0)
                        {
                            Console.WriteLine($"{first} can not  divided by zero");
                        }
                        else
                        {
                            Console.WriteLine("Division: " + (first / second)); // Fixed from subtraction
                        }
                    }
                    else
                    {
                        Console.WriteLine("Please provide a valid integer for the second number.");
                    }
                }
                

            
                else
                {
                    Console.WriteLine("please provide integer value");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void Average()//allow decimal
        {
            try
            {
                Console.Write("Enter First Number ");
                double first = Convert.ToDouble(Console.ReadLine());
                Console.Write("Enter Second Number ");
                double second = Convert.ToDouble(Console.ReadLine());
                double d = (first + second) / 2;
                Console.WriteLine("Average" + d);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void Percentage()
        {
            try
            {
                Console.Write("Enter Value ");
                double value = Convert.ToDouble(Console.ReadLine());

                Console.Write("Enter Total");
                double total = Convert.ToDouble(Console.ReadLine());

                if (total < value)
                {
                    Console.Write("invalid total value ");
                }
                else
                {
                    double perc = (value * 100) / total;
                    Console.WriteLine("Percentage is"+ perc + "%");
                }
            }
            catch (Exception)
            {

                throw;
            }


        }
        public void CompoundInterest()
        {
            try
            {
                Console.Write("Enter Principal Amount (P)");
                double p = Convert.ToDouble(Console.ReadLine());

                Console.Write("Enter Annual Interest Rate (r in %): ");
                double r = Convert.ToDouble(Console.ReadLine());

                Console.Write("Enter Time (t in years): ");
                double t = Convert.ToDouble(Console.ReadLine());

                double a = p * Math.Pow((1 + r / 100), t);
                double ci = a - p;

                Console.WriteLine("Compound Interest" + ci);
                Console.WriteLine("Total Amount" + a);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void MainMenu()
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("Main Menu");
                    Console.WriteLine("Enter 1: Basic Math Operations");
                    Console.WriteLine("Enter 2: Average of Numbers");
                    Console.WriteLine("Enter 3: Percentage of a Given Number");
                    Console.WriteLine("Enter 4: Compound Interest");
                    Console.WriteLine("Enter 5: Exit");
                    Console.WriteLine();

                    


                    Console.Write("Enter your Choice ");
                    if (int.TryParse(Console.ReadLine(), out int input))
                    {
                        switch (input)
                        {
                            case 1: Calci(); break;
                            case 2: Average(); break;
                            case 3: Percentage(); break;
                            case 4: CompoundInterest(); break;
                            case 5: Console.WriteLine("Calculator exit...."); return;
                            default: Console.WriteLine("Invalid choice,select valid choice "); break;
                        }


                    }
                    else
                    {
                        Console.WriteLine("inavild type, input  provide integer input");
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
