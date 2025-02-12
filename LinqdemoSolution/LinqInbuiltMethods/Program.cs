using System;
using System.Text.RegularExpressions;

namespace LinqInbuiltMethods
{
    class Program
    {
        public static void Where()
        {
            int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var evenNumbers = numbers.Where(n => n % 2 == 0);  // Filters even numbers

            foreach (var num in evenNumbers)
            {
                Console.Write("even number in array: "+num);  // Output: 2, 4, 6, 8
            }
            Console.WriteLine();


        }
        public static void Select()
        {
            int[] numbers = { 1, 2, 3, 4, 5 };
            var squares = numbers.Select(n => n * n);  // Squares each number

            foreach (var square in squares)
            {
                Console.Write("square of number: "+square);  // Output: 1, 4, 9, 16, 25
            }
            Console.WriteLine();

        }

        public static void OrderByAscOrDesc()
        {
            int[] numbers = { 5, 1, 4, 2, 8 };
            var sorted = numbers.OrderBy(n => n);

            foreach (var num in sorted)
            {
                Console.Write("ascending: "+num+" ");  // Output: 1, 2, 4, 5, 8
            }
            Console.WriteLine();
        }
        public static void FirstOrDefault()
        {
            int[] numbers = { 1, 2, 3, 4, 5 };
            Console.Write("first element in array: "+numbers.First(n => n > 2));  // Output: 3
            Console.WriteLine();
            int[] emptyArray = { };
            Console.Write("first or default in empty array: "+emptyArray.FirstOrDefault());  // Output: 0 (default value for int)
            Console.WriteLine();
        }
        public static void LastOrDefault()
        {
            int[] numbers = { 1, 2, 3, 4, 5 };
            Console.Write("last element: "+numbers.Last(n => n > 2));
            Console.WriteLine();
            int[] emptyArray = { };
            Console.Write("last element of empty array: "+emptyArray.LastOrDefault());
            Console.WriteLine();
        }

        public static void Count()
        {
            int[] numbers = { 1, 2, 3, 4, 5 };
            int count = numbers.Count(n => n > 2);  // Counts numbers greater than 2
            Console.Write("count: "+count);  // Output: 3
            Console.WriteLine();

        }

        public static void Aggregate()
        {
            int[] numbers = { 1, 2, 3, 4, 5 };
            Console.WriteLine("sum: "+numbers.Sum());       // Output: 15
            Console.WriteLine("Average: "+numbers.Average());   // Output: 3
            Console.WriteLine("Min: "+numbers.Min());       // Output: 1
            Console.WriteLine("Max: "+numbers.Max());       // Output: 5

        }
        public static void Distinct()
        {
            int[] numbers = { 1, 2, 2, 3, 4, 4, 5 };
            var distinctNumbers = numbers.Distinct();
            Console.WriteLine("distict numbers are: ");
            foreach (var num in distinctNumbers)
            {
                Console.Write(num+" ");  // Output: 1, 2, 3, 4, 5
            }

        }
        public static void AnyOrAll()
        {
            int[] numbers = { 1, 2, 3, 4, 5 };
            Console.WriteLine("any numbers: "+numbers.Any(n => n > 4));  // Output: True
            Console.WriteLine("all numbers: "+numbers.All(n => n > 0));  // Output: True

        }

        public static void TakeorSkip()
        {
            int[] numbers = { 1, 2, 3, 4, 5 };
            var firstTwo = numbers.Take(2);  // Takes the first two elements
            var skipTwo = numbers.Skip(2);  // Skips the first two elements

            Console.WriteLine(string.Join(", ", firstTwo));  // Output: 1, 2
            Console.WriteLine(string.Join(", ", skipTwo));   // Output: 3, 4, 5
            //here join is inbuilt method     string.join(separator,collection);

        }

        public static void GroupBy()
        {
            List<string> words = new List<string> { "anna","amma","mama","mami"};
            var groups = words.GroupBy(e => e[0]);

            foreach (var group in groups)
            {
                Console.WriteLine($"Words starting with '{group.Key}': {string.Join(", ", group)}");
            }
        }


        public static void Main()
        {
            Where();
            Select();
            OrderByAscOrDesc();
            FirstOrDefault();
            LastOrDefault();
            Count();
            FirstOrDefault();
            Aggregate();
            AnyOrAll();
            TakeorSkip();
            Distinct();
            GroupBy();
        }
    }
}
