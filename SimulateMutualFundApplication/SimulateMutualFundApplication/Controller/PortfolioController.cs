using Microsoft.AspNetCore.Mvc;
using MutualFundSimulatorService.Model;
using MutualFundSimulatorService.Repository;
using Microsoft.Data.SqlClient;

namespace MutualFundSimulatorApplication.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly MutualFundRepository _repository;
        private readonly User _user;

        public PortfolioController(MutualFundRepository repository, User user)
        {
            _repository = repository;
            _user = user;
        }

        [HttpGet("lumpsum/portfolio")]
        public IActionResult DisplayLumpSumPortfolio([FromQuery] string userEmail)
        {
            if (string.IsNullOrWhiteSpace(userEmail))
                return BadRequest(new { Message = "User email is required" });

            _user.userEmail = userEmail;

            decimal profitLoss = _repository.DisplayLumpSumPortfolio();
            if (profitLoss == 0)
            {
                using (var connection = new SqlConnection(_repository.ConnectionString))
                {
                    connection.Open();
                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE useremail = @useremail";
                    using (var command = new SqlCommand(checkQuery, connection))
                    {
                        command.Parameters.AddWithValue("@useremail", userEmail);
                        int count = (int)command.ExecuteScalar();
                        if (count == 0)
                            return NotFound(new { Message = "User not found" });
                    }
                }
                return Ok(new { Message = "No lumpsum portfolio found for this user", ProfitLoss = profitLoss });
            }

            return Ok(new { UserEmail = userEmail, ProfitLoss = profitLoss });
        }

        [HttpGet("sip/portfolio")]
        public IActionResult DisplaySIPPortfolio([FromQuery] string userEmail)
        {
            if (string.IsNullOrWhiteSpace(userEmail))
                return BadRequest(new { Message = "User email is required" });

            _user.userEmail = userEmail;

            decimal profitLoss = _repository.DisplaySIPPortfolio();
            if (profitLoss == 0)
            {
                using (var connection = new SqlConnection(_repository.ConnectionString))
                {
                    connection.Open();
                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE useremail = @useremail";
                    using (var command = new SqlCommand(checkQuery, connection))
                    {
                        command.Parameters.AddWithValue("@useremail", userEmail);
                        int count = (int)command.ExecuteScalar();
                        if (count == 0)
                            return NotFound(new { Message = "User not found" });
                    }
                }
                return Ok(new { Message = "No SIP portfolio found for this user", ProfitLoss = profitLoss });
            }

            return Ok(new { UserEmail = userEmail, ProfitLoss = profitLoss });
        }

        [HttpGet("sip/upcoming")]
        public IActionResult GetUpcomingSIPInstallments([FromQuery] string userEmail)
        {
            if (string.IsNullOrWhiteSpace(userEmail))
                return BadRequest(new { Message = "User email is required" });

            _user.userEmail = userEmail;
            _repository.GetUpcomingSIPInstallments();
            return Ok(new { Message = "Upcoming SIP installments retrieved for " + userEmail });
        }
    }
}