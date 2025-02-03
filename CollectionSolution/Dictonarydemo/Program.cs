using System;


namespace Dictonarydemo
{
    class Program
    {
        public static void Main()
        {
            Dictionary<string,Object> dict = new Dictionary<string,Object>();

            dict.Add("name", "manu");

            dict.Add("age", 21);
            dict.Add("job", "data scientist");
            dict.Add("location", "Bangalore");

            foreach(string key in dict.Keys)
            {
                Console.WriteLine(key+" "+dict[key]);
            }
        }
    }
}