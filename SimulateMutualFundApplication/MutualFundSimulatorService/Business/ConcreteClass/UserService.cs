using Microsoft.AspNetCore.Mvc;
using MutualFundSimulatorService.Business.Interfaces;
using MutualFundSimulatorService.Model;
using MutualFundSimulatorService.Model.DTO;
using MutualFundSimulatorService.Repository.Interface;
using System.Text.RegularExpressions;

namespace MutualFundSimulatorService.Business.ConcreteClass
{
    public class UserService : IUserService
    {
        private readonly IRepository _repository;
        private readonly User _user;

        public UserService(IRepository repository, User user)
        {
            _repository = repository;
            _user = user;
        }

        public IActionResult LoginUser(string userEmail, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userEmail) || string.IsNullOrWhiteSpace(password))
                    return new BadRequestObjectResult(new { Message = "Email and password are required" });

                _user.userEmail = userEmail;
                _user.password = password;
                bool success = _repository.AuthenticateUser();
                return success ? new OkObjectResult(new { Message = "Authentication successful", UserId = _user.id })
                               : new UnauthorizedObjectResult(new { Message = "Invalid credentials" });
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IActionResult Register(UserDto userdto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userdto.name) ||
                        string.IsNullOrWhiteSpace(userdto.phoneNumber) ||
                        string.IsNullOrWhiteSpace(userdto.userEmail) ||
                        string.IsNullOrWhiteSpace(userdto.password) ||
                        userdto.walletBalance < 1000)
                {
                    return new BadRequestObjectResult(new { Message = "Name, phone number, email, password, and wallet balance (minimum ₹1000) are required" });
                }

                if (userdto.age < 18)
                {
                    return new BadRequestObjectResult(new { Message = "Access Denied, Age must be greater than or equal to 18" });
                }

                if (!Regex.IsMatch(userdto.phoneNumber, @"^\d{10}$"))
                {
                    return new BadRequestObjectResult(new { Message = "Invalid phone number. Please enter a 10-digit number." });
                }

                if (!Regex.IsMatch(userdto.userEmail, @"^[a-zA-Z0-9._%+-]+@gmail\.com$"))
                {
                    return new BadRequestObjectResult(new { Message = "Invalid email format." });
                }

                var userDetails = new User
                {
                    name = userdto.name,
                    age = userdto.age,
                    phoneNumber = userdto.phoneNumber,
                    userEmail = userdto.userEmail,
                    password = userdto.password,
                    walletBalance = userdto.walletBalance
                };

                bool success = _repository.SaveUserDetails(userDetails);

                return success
                    ? new OkObjectResult(new { Message = "Registration successful", UserId = _user.id })
                    : new BadRequestObjectResult(new { Message = "Registration failed, email may already exist" });
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IActionResult AddMoneyToWallet(int id, decimal amount)
        {
            try
            {
                if (id <= 0 || amount == 0)
                    return new BadRequestObjectResult(new { Message = "User ID and non-zero amount are required" });

                if (!_repository.IsValidUserId(id))
                    return new BadRequestObjectResult(new { Message = "Invalid user ID" });

                _user.id = id;
                _repository.AddMoneyToWallet(amount);
                return new OkObjectResult(new { Message = "Wallet balance updated" });
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}