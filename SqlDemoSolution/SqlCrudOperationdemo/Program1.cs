using System;
using Microsoft.Data.SqlClient;

class Program1
{
    static void Main()
    {
        string connectionString = @"Server=LAPTOP-HS9AFKH4;Database=demo;User Id=sa;Password=manudp9015;TrustServerCertificate=True;";
        //string connectionString = @"Server=LAPTOP-HS9AFKH4;Database=demo;Trusted_Connection=True;TrustServerCertificate=True;";
     
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();

            //DropDatabase(conn);
            /*to drop database we must use database as master since currently using databse cannot be modified*/
                                            
            CreateDatabase(conn);

            DropTable(conn);       
            CreateTable(conn);    
            AlterTable(conn);   
            InsertData(conn);      
            UpdateData(conn);      
                 
            DeleteData(conn);      
            ReadData(conn);        
        }
    }

    static void InsertData(SqlConnection conn)
    {
        string query = "INSERT INTO employee (empid, empname, empage, empdept) VALUES (1, 'John', 30, 'IT')";
        SqlCommand command = new SqlCommand(query, conn);
        int affected = command.ExecuteNonQuery();
        Console.WriteLine($"{affected} row(s) inserted.");
    }



    static void UpdateData(SqlConnection conn)
    {
        string query = "update employee set empage=22 where empid=2";
        SqlCommand command = new SqlCommand(query, conn);
        int rowsAffected = command.ExecuteNonQuery();
        Console.WriteLine($"{rowsAffected} row(s) updated.");
    }

    static void DeleteData(SqlConnection conn)
    {
        string query = "delete from employee where empid=3 ";
        SqlCommand command = new SqlCommand(query, conn);
        int rowsAffected = command.ExecuteNonQuery();
        Console.WriteLine($"{rowsAffected} row(s) deleted.");
    }

    static void ReadData(SqlConnection conn)
    {
        string query = "SELECT empid, empname, empage, empdept from employee";
        SqlCommand command = new SqlCommand(query, conn);
        SqlDataReader reader = command.ExecuteReader();

        Console.WriteLine("empid\tename\tempage\tempdept");
        while (reader.Read())
        {
            Console.WriteLine($"{reader["empid"]}\t{reader["empname"]}\t{reader["empage"]}\t{reader["empdept"]}");
        }

        reader.Close();
    }
    static void CreateTable(SqlConnection conn)
    {
        string querry = "create table employee(empid int primary key,empname varchar(10),empage int,empdept varchar(10))";
        SqlCommand command=new SqlCommand(querry, conn);
        try
        {
            command.ExecuteNonQuery();
            Console.WriteLine("Table 'employee' created successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
    static void AlterTable(SqlConnection conn)
    {
        string querry = "alter table employee add sal int;";
        SqlCommand command=new SqlCommand( querry, conn);
        try
        {
            command.ExecuteNonQuery();
            Console.WriteLine("New Column added successfully");
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
    static void DropTable(SqlConnection conn)
    {
        string querry = "drop table employee ";
        SqlCommand command=new SqlCommand(querry,conn);
        try
        {
            command.ExecuteNonQuery();
            Console.WriteLine($"table employee droped successfully");

        }
        catch(Exception e)
        {
            Console.WriteLine(e);
        }
    }
    static void DropDatabase(SqlConnection conn)
    {
        string querry = "drop database demo";
        SqlCommand command = new SqlCommand(querry, conn);
        try
        {
            command.ExecuteNonQuery();
            Console.WriteLine("Database droped successfully");
        }
        catch(Exception e)
        {
            Console.WriteLine(e);   
        }
    }
    static void CreateDatabase(SqlConnection conn)
    {
        string querry = "create database demo";
        SqlCommand command = new SqlCommand(querry, conn);
        try
        {
            command.ExecuteNonQuery();
            Console.WriteLine("Database created successfully");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}


