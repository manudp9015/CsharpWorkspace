using System;
using Microsoft.Data.SqlClient;

class Program
{
    static void Main()
    {
        string connectionString = @"Server=LAPTOP-HS9AFKH4;Database=demo;User Id=sa;Password=manudp9015;TrustServerCertificate=True;";
        //string connectionString = @"Server=LAPTOP-HS9AFKH4;Database=demo;Trusted_Connection=True;TrustServerCertificate=True;";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
           
                conn.Open();
                string query = "SELECT empid, empname, empage, empdept from employee";
                SqlCommand command = new SqlCommand(query, conn);
                SqlDataReader reader = command.ExecuteReader();

                Console.WriteLine("empid\tename\tempage\tempdept");
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["empid"]}\t{reader["empname"]}\t{reader["empage"]}\t{reader["empdept"]}");
                }

                reader.Close(); ;
                conn.Close();
        }
    }
}
