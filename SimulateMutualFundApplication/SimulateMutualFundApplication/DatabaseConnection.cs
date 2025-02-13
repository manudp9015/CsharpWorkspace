using System;
using Microsoft.Data.SqlClient;


namespace MutualFundSimulatorApplication
{
    class DatabaseConnection
    {
        public void SaveUserInputValues(string useremail,string username,int userage,string userpassword,string userphone)
        {
            string connectionString = @"Server=LAPTOP-HS9AFKH4;Database=MutualFundSimulator;Trusted_Connection=True;TrustServerCertificate=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Users (useremail, username, userage, userpassword, userphone)" +
                               " VALUES (@useremail, @username, @userage, @userpassword, @userphone)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@useremail", useremail);
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@userage", userage);
                    command.Parameters.AddWithValue("@userpassword", userpassword);
                    command.Parameters.AddWithValue("@userphone", userphone);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("User data inserted successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Failed to insert user data.");
                    }
                }
            }
        }
        static void InsertData(SqlConnection conn)
        {
            string query = "insert into Users values('manudp9015@gmail.com','manu',21,'1234','1234567890')";
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
            string query = "SELECT useremail, username, userage,userpassword,userphone from emp";
            SqlCommand command = new SqlCommand(query, conn);
            SqlDataReader reader = command.ExecuteReader();

            Console.WriteLine("UserEmail\tUserName\tUserAge\tUserPassword\tUserPhone");
            while (reader.Read())
            {
                Console.WriteLine($"{reader["useremail"]}\t{reader["username"]}\t{reader["userage"]}\t{reader["userpassword"]}\t{reader["userphone"]}");
            }
            reader.Close();
        }
    }
}