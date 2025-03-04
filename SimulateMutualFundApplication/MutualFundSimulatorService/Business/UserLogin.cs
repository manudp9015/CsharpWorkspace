using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using MutualFundSimulatorService.Model;
using MutualFundSimulatorService.Repository;
using System;
using System.Text.RegularExpressions;


namespace MutualFundSimulatorService.Business
{
    public class UserLogin 
    {
        private MutualFundRepository _repository;
        private User _user;

        public UserLogin(MutualFundRepository repository, User user)
        {
            _repository = repository;
            _user = user;
        }


        /// <summary>
        /// Registers a new user by collecting and validating their details, then saving them to the database.
        /// </summary>
        public void RegisterUser()
        {
            try
            {
                bool validName = ValidateUserName();
                if (!validName) return;

                bool validAge = ValidateUserAge();
                if (!validAge) return;

                bool validPhoneNumber = ValidateUserMobilNumber();
                if (!validPhoneNumber) return;

                string pattern;
                bool validMail = ValidateUserMail(out pattern);
                if (!validMail) return;

                bool validPassword = ValidateUserPassword();
                if (!validPassword) return;

                // Create User object with collected details
                var userDetails = new User
                {
                    name = _user.name,
                    age = _user.age,
                    phoneNumber = _user.phoneNumber,
                    userEmail = _user.userEmail,
                    password = _user.password,
                    walletBalance = 0m // Default or prompt for this if needed
                };

                if (_repository.SaveUserDetails(userDetails)) // Updated call
                {
                    Console.WriteLine("User registration completed successfully!");
                }
                else
                {
                    Console.WriteLine("Registration failed, email may already exist.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Validates the user's password, ensuring it is not empty, with a maximum of 3 attempts.
        /// </summary>
        /// <returns></returns>
        private bool ValidateUserPassword()
        {
            try
            {
                int passwordAttempts = 0;
                do
                {
                    Console.Write("Enter your password: ");
                    _user.password = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(_user.password))
                    {
                        passwordAttempts++;
                        Console.WriteLine("Password cannot be empty.");
                    }
                    if (passwordAttempts == 3)
                    {
                        Console.WriteLine("Maximum attempts reached for password.");
                        return false;
                    }
                } while (string.IsNullOrWhiteSpace(_user.password)); return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Validates the user's email address, ensuring it is a valid Gmail address, with a maximum of 3 attempts.
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        private bool ValidateUserMail(out string pattern)
        {
            try
            {
                int emailAttempts = 0;
                do
                {
                    Console.Write("Enter your Gmail: ");
                    _user.userEmail = Console.ReadLine();
                    pattern = @"^[a-zA-Z0-9._%+-]+@gmail\.com$";
                    if (!Regex.IsMatch(_user.userEmail, pattern))
                    {
                        emailAttempts++;
                        Console.WriteLine("Invalid Gmail address. Please enter a valid Gmail.");
                    }
                    if (emailAttempts == 3)
                    {
                        Console.WriteLine("Maximum attempts reached for email.");
                        return false;
                    }
                } while (!Regex.IsMatch(_user.userEmail, pattern)); return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Validates the user's mobile number, ensuring it is a 10-digit number, with a maximum of 3 attempts.
        /// </summary>
        /// <returns></returns>
        private bool ValidateUserMobilNumber()
        {
            try
            {
                int phoneAttempts = 0;
                do
                {
                    Console.Write("Enter your phone number: ");
                    _user.phoneNumber = Console.ReadLine();
                    if (_user.phoneNumber.Length != 10 || !long.TryParse(_user.phoneNumber, out _))
                    {
                        phoneAttempts++;
                        Console.WriteLine("Invalid phone number. Please enter a 10-digit number.");
                    }
                    if (phoneAttempts == 3)
                    {
                        Console.WriteLine("Maximum attempts reached for phone number.");
                        return false;
                    }
                } while (_user.phoneNumber.Length != 10 || !long.TryParse(_user.phoneNumber, out _)); return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Validates the user's age, ensuring it is 18 or older, with a maximum of 3 attempts.
        /// </summary>
        /// <returns></returns>
        private bool ValidateUserAge()
        {
            try
            {
                int ageAttempts = 0;
                int age;
                while (ageAttempts < 3)
                {
                    Console.Write("Enter your age: ");
                    string input = Console.ReadLine();
                    if (!int.TryParse(input, out age))
                    {
                        ageAttempts++;
                        Console.WriteLine("Invalid input. Please enter a valid numeric age.");
                        continue;
                    }
                    if (age < 18)
                    {
                        Console.WriteLine("Access denied. Your age is less than 18.");
                        return false;
                    }
                    _user.age = age;
                    return true;
                }
                Console.WriteLine("Maximum attempts reached for age.");
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Validates the user's name, ensuring it is alphabetic and not empty, with a maximum of 3 attempts.
        /// </summary>
        /// <returns></returns>
        private bool ValidateUserName()
        {
            try
            {
                int nameAttempts = 0;
                do
                {
                    Console.Write("Enter your name: ");
                    _user.name = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(_user.name) || !_user.name.All(char.IsLetter))
                    {
                        nameAttempts++;
                        Console.WriteLine("User name must be alphabetic and cannot be empty.");
                        if (nameAttempts == 3)
                        {
                            Console.WriteLine("Maximum attempts reached for name.");
                            return false;
                        }
                    }
                } while (string.IsNullOrWhiteSpace(_user.name) || !_user.name.All(char.IsLetter)); return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Authenticates a user by validating email and password, retrieving wallet balance on success.
        /// </summary>
        /// <returns></returns>
        public bool LoginUser(string email, string password)
        {
            try
            {
                _user.userEmail = email;
                _user.password = password;

                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                    return false;

                if (_repository.AuthenticateUser())
                {
                    using (SqlConnection connection = new SqlConnection(_repository.ConnectionString))
                    {
                        connection.Open();
                        string query = "SELECT walletbalance FROM Users WHERE useremail = @useremail";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@useremail", email);
                            object result = command.ExecuteScalar();
                            _user.walletBalance = result != null && result != DBNull.Value ? Convert.ToDecimal(result) : 0m;
                        }
                    }
                    _repository.IncrementInstallments();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                return false;
            }
        }
    }
}