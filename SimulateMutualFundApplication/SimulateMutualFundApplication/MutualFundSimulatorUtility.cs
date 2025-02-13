using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MutualFundSimulatorApplication
{
    internal class MutualFundSimulatorUtility
    {

        public void MainMenu()
        {
            try
            {
               
                while (true)
                {
                    Console.WriteLine();
                    Console.WriteLine("Main Menu");
                    Console.WriteLine("1: User sign in");
                    Console.WriteLine("2: User sign up");
                    Console.WriteLine("5: Exit");
                    Console.WriteLine();

                    Console.Write("Enter your Choice: ");
                    if (int.TryParse(Console.ReadLine(), out int input))
                    {
                        switch (input)
                        {
                            //case 1: UserSignIn(); break;
                            case 2: UserSignUp(); break;
                            case 5: Console.WriteLine("exiting..."); return;
                            default: Console.WriteLine("Invalid Input. Give Valid Input"); break;
                        }
                    }
                    else Console.WriteLine("Invalid Input. Give Valid Input");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void UserSignUp()
        {
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("User name cannot be empty.");
                return;
            }
            Console.Write("Enter your age: ");
            string  input = Console.ReadLine();
            if (!int.TryParse(input,out int age))
            {
                Console.WriteLine("Invalid age input. Please enter a valid age.");
                return;
            }

            if (age < 18)
            {
                Console.WriteLine("Access denied. Age must be 18 or above.");
                return;
            }
            
            Console.Write("Enter your phone number: ");
            string phoneNumber = Console.ReadLine();

            if (phoneNumber.Length == 10 && long.TryParse(phoneNumber, out _))
            {
                Console.WriteLine("Valid phone number.");
            }
            else
            {
                Console.WriteLine("Invalid phone number. Please enter a 10-digit number.");
            }
            

            Console.Write("Enter your email: ");
            string email = Console.ReadLine();
            string pattern = @"^[a-zA-Z0-9._%+-]+@gmail\.com$";
           
            if (Regex.IsMatch(email, pattern))
            {
                    Console.WriteLine("User registration completed.");
            }
            
            else Console.WriteLine("Invalid Gmail address.");

            Console.Write("Enter your password: ");
            string password = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("User password cannot be empty.");
                return;
            }

            SaveUserInputValues(email,name,age,password,phoneNumber);
        }

        private void UserSignIn()
        {
            Console.Write("Enter your email: ");
            string userEmail = Console.ReadLine();
            bool ans = CheckUserMail(userEmail);
            if (!ans)
            {
                Console.WriteLine($"Your Patient Email: {userEmail} is invalid or not registered yet.");
                return;
            }

            Console.Write("Enter your password: ");
            string userpassword = Console.ReadLine();
            if (!List.where(l => l.userpassword))
            {
                Console.WriteLine($"Your Patient Email: {useremail} is invalid or not registered yet.");
                return;
            }
        }
        public bool CheckUserMail(string usermail)
        {
            string connectionString = @"Server=LAPTOP-HS9AFKH4;Database=MutualFundSimulator;Trusted_Connection=True;TrustServerCertificate=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(1) FROM Users WHERE useremail = @usermail";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@usermail", usermail);

                    int count = (int)command.ExecuteScalar();  // Returns the number of matching rows
                    return count > 0;
                }
            }
        }


        public void SaveUserInputValues(string useremail, string username, int userage, string userpassword, string userphone)
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
                    try
                    {
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
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
        }
    }
}
