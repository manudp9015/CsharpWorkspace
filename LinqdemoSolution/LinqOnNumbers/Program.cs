using System;


namespace LinqOnNumbers
{

   
       
    
    class Program
    {
        public static void DistinctNum()
        {
            List<int> list = new List<int> { 1, 2, 3, 4, 5, 1, 2, 3, 8, 9 };
            var distict = list.Distinct().ToList();
            foreach ( var i in distict )
            {
                Console.Write( i+" ");
            }
            Console.WriteLine();
        }
        public static void FilterNum()
        {
            List<int> list = new List<int> { 1, 2, 3, 4, 5, 1, 2, 3, 8, 9 };
            var even = list.Where(f=> f%2==0).ToList();
            foreach (var i in even)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine();

            var odd = list.Where(f => f % 2 == 1).ToList();
            foreach (var i in odd)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine();
        }

        public static void SortingNum()
        {
            List<int> list = new List<int> { 1, 2, 3, 4, 5, 1, 2 };
            var ascn = list.OrderBy(e => e);
            var desc = list.OrderByDescending(e => e);
            foreach( var i in ascn)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine();
            foreach (var i in desc)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine();

        }
        public static void Main()
        {
            DistinctNum();
            FilterNum();
            SortingNum();
        }
    }
}