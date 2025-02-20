using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MutualFundSimulatorApplication
{
    internal class UserLogin
    {

        private MutualFundRepository _repository;
        
        private User _user;
        public UserLogin(MutualFundRepository repository, User user)
        {
            _repository = repository;
            _user = user;
          
        }
        public void RegisterUser()
        {
            try
            {
                bool validName = ValidateUserName();
                if (!validName)
                {
                    return;
                }
                bool validAge = ValidateUserAge();
                if (!validAge)
                {
                    return;
                }
                bool validPhoneNumber = ValidateUserMobilNumber();
                if (!validPhoneNumber)
                {
                    return;
                }
                string pattern;
                bool validMail = ValidateUserMail( out pattern);
                if (!validMail)
                {
                    return;
                }
                bool validPassword = ValidateUserPassword();
                if (!validPassword)
                {
                    return;
                }
                _repository.SaveUserDetails();
                Console.WriteLine("User registration completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        private  bool ValidateUserPassword()
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

        private  bool ValidateUserMail(out string pattern)
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

        private  bool ValidateUserMobilNumber()
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

        public bool LoginUser()
        {
            try
            {
                Console.Write("Enter your email: ");
                _user.userEmail = Console.ReadLine();
                Console.Write("Enter your password: ");
                _user.password = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(_user.userEmail) || string.IsNullOrWhiteSpace(_user.password))
                {
                    Console.WriteLine("Both email and password are required. Please try again.");
                    return false;
                }
                if (_repository.AuthenticateUser())
                {
                    Console.WriteLine("Login successful.");
                    return true;
                }
                else
                {
                    Console.WriteLine("Invalid email or password.");
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
