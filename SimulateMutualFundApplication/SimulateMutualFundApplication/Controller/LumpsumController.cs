using Microsoft.AspNetCore.Mvc;
using MutualFundSimulatorService.Model;
using MutualFundSimulatorService.Repository;
using MutualFundSimulatorService.Business;

namespace MutualFundSimulatorApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LumpsumController : ControllerBase
    {
        private readonly MutualFundRepository _repository;
        private readonly User _user;
        private readonly UserLumpsumInvest _userLumpsum;
        private readonly Lumpsum _lumpsumInvest;

        public LumpsumController(
            MutualFundRepository repository,
            User user,
            UserLumpsumInvest userLumpsum,
            Lumpsum lumpsumInvest)
        {
            _repository = repository;
            _user = user;
            _userLumpsum = userLumpsum;
            _lumpsumInvest = lumpsumInvest;
        }

        [HttpPost("invest")]
        public IActionResult SaveLumpsumInvest(
            [FromQuery] string fundName,
            [FromQuery] decimal amount,
            [FromQuery] bool deducted)
        {
            if (string.IsNullOrWhiteSpace(fundName) || amount <= 0)
                return BadRequest(new { Message = "Fund name and amount are required" });

            try
            {
                _lumpsumInvest.ProcessInvestment(fundName, amount, deducted);
                return Ok(new { Message = "Lumpsum investment saved", Quantity = _userLumpsum.quantity });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("update")]
        public IActionResult UpdateCurrentAmountsForAllInvestments()
        {
            _repository.UpdateCurrentAmountsForAllInvestments();
            return Ok(new { Message = "Lumpsum amounts updated" });
        }
    }
}