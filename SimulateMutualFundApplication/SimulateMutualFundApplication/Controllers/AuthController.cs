using Microsoft.AspNetCore.Mvc;
using MutualFundSimulatorService.Interfaces;
using MutualFundSimulatorService.Model;
using MutualFundSimulatorService.Repository;

namespace MutualFundSimulatorApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserLogin _userLogin;
        private readonly MutualFundRepository _repository;

        public AuthController(IUserLogin userLogin, MutualFundRepository repository)
        {
            _userLogin = userLogin;
            _repository = repository;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User request)
        {
            bool success = _userLogin.LoginUser(request.userEmail, request.password);
            return success ? Ok(new { Message = "Login successful" }) : Unauthorized(new { Message = "Invalid credentials" });
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User request)
        {
            if (string.IsNullOrWhiteSpace(request.name) || request.age <= 0 ||
                string.IsNullOrWhiteSpace(request.phoneNumber) || string.IsNullOrWhiteSpace(request.userEmail) ||
                string.IsNullOrWhiteSpace(request.password) || request.walletBalance < 1000)
            {
                return BadRequest(new { Message = "All fields are required and wallet balance must be at least ₹1000" });
            }

            bool success = _repository.SaveUserDetails(request);
            return success ? Ok(new { Message = "Registration successful" }) :
                             BadRequest(new { Message = "Registration failed, email may already exist" });
        }
    }
}