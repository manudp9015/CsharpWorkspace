using System;
using Microsoft.Data.SqlClient;


namespace LinqJoindemo
{
    class Program
    {
        public static void Main(string[] args)
        {
            string connectionString = @"Server=LAPTOP-HS9AFKH4;Database=demo;User Id=sa;Password=manudp9015;TrustServerCertificate=True;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string querry = "select * from emp";
                SqlCommand cmd = new SqlCommand(querry,conn);
                try
                {
                    int rowaffected = cmd.ExecuteNonQuery();
                    Console.WriteLine("rowaffected" + rowaffected);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                

            }
        }
    }
}