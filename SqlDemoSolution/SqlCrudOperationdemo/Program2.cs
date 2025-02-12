using System;
using Microsoft.Data.SqlClient;

class Program2
{
    static void Main()
    {
        //string connectionString = @"Server=LAPTOP-HS9AFKH4;Database=demo;User Id=sa;Password=Manudp9015@;TrustServerCertificate=True;";
        string connectionString = @"Server=LAPTOP-HS9AFKH4;Database=demo;Trusted_Connection=True;TrustServerCertificate=True;";
        Console.WriteLine("Choose an operation:");
        Console.WriteLine("1. Insert");
        Console.WriteLine("2. Update");
        Console.WriteLine("3. Delete");
        Console.WriteLine("4. Read");
        int choice = Convert.ToInt32(Console.ReadLine());

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();

            switch (choice)
            {
                case 1:
                    InsertData(conn);
                    break;
                case 2:
                    UpdateData(conn);
                    break;
                case 3:
                    DeleteData(conn);
                    break;
                case 4:
                    ReadData(conn);
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
            conn.Close();
        }
    }

    static void InsertData(SqlConnection conn)
    {
        string query = "insert into emp values(2,'manu',21 )";
        SqlCommand command = new SqlCommand(query, conn);
        int rowsAffected = command.ExecuteNonQuery();
        Console.WriteLine($"{rowsAffected} row(s) inserted.");
    }

    static void UpdateData(SqlConnection conn)
    {
        string query = "update emp set empage=20 where empid=1";
        SqlCommand command = new SqlCommand(query, conn);
        int rowsAffected = command.ExecuteNonQuery();
        Console.WriteLine($"{rowsAffected} row(s) updated.");
    }

    static void DeleteData(SqlConnection conn)
    {
        string query = "delete from emp where empid=3 ";
        SqlCommand command = new SqlCommand(query, conn);
        int rowsAffected = command.ExecuteNonQuery();
        Console.WriteLine($"{rowsAffected} row(s) deleted.");
    }

    static void ReadData(SqlConnection conn)
    {
        string query = "SELECT empid, ename, empage from emp";
        SqlCommand command = new SqlCommand(query, conn);
        SqlDataReader reader = command.ExecuteReader();

        Console.WriteLine("EmpID\tName\tDepartment");
        while (reader.Read())
        {
            Console.WriteLine($"{reader["empid"]}\t{reader["name"]}\t{reader["department"]}");
        }

        reader.Close();
    }
}
