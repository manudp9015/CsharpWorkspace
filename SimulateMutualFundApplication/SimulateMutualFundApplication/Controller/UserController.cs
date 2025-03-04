using Microsoft.AspNetCore.Mvc;
using MutualFundSimulatorService.Model;
using MutualFundSimulatorService.Repository;
using MutualFundSimulatorService.Business;

namespace MutualFundSimulatorApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MutualFundRepository _repository;
        private readonly User _user;
        private readonly UserLogin _userLogin;

        public UserController(
            MutualFundRepository repository,
            User user,
            UserLogin userLogin)
        {
            _repository = repository;
            _user = user;
            _userLogin = userLogin;
        }

        [HttpPost("login")]
        public IActionResult LoginUser([FromQuery] string userEmail, [FromQuery] string password)
        {
            if (string.IsNullOrWhiteSpace(userEmail) || string.IsNullOrWhiteSpace(password))
                return BadRequest(new { Message = "Email and password are required" });

            bool success = _userLogin.LoginUser(userEmail, password);
            return success ? Ok(new { Message = "Authentication successful" }) : Unauthorized(new { Message = "Invalid credentials" });
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
        [HttpPut("wallet/add")]
        public IActionResult AddMoneyToWallet([FromBody] User request)
        {
            if (request.walletBalance == 0)
                return BadRequest(new { Message = "Amount must be non-zero" });

            _repository.AddMoneyToWallet(request.walletBalance);
            return Ok(new { Message = "Wallet balance updated" });
        }
    }
}