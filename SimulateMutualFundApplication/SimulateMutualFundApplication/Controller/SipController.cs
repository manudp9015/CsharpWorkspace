using Microsoft.AspNetCore.Mvc;
using MutualFundSimulatorService.Model;
using MutualFundSimulatorService.Repository;
using MutualFundSimulatorService.Business;

namespace MutualFundSimulatorApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SipController : ControllerBase
    {
        private readonly MutualFundRepository _repository;
        private readonly User _user;
        private readonly UserSipInvest _userSip;
        private readonly Sip _sipInvest;

        public SipController(
            MutualFundRepository repository,
            User user,
            UserSipInvest userSip,
            Sip sipInvest)
        {
            _repository = repository;
            _user = user;
            _userSip = userSip;
            _sipInvest = sipInvest;
        }

        [HttpPost("invest")]
        public IActionResult SaveSIPInvest(
            [FromQuery] string fundName,
            [FromQuery] decimal sipAmount,
            [FromQuery] DateTime sipStartDate,
            [FromQuery] int durationInMonths)
        {
            if (string.IsNullOrWhiteSpace(fundName) || sipAmount <= 0 || durationInMonths <= 0)
                return BadRequest(new { Message = "Fund name, SIP amount, and duration are required" });

            try
            {
                _sipInvest.ProcessSipInvestment(fundName, sipAmount, sipStartDate, durationInMonths);
                return Ok(new { Message = "SIP investment saved", Quantity = _userSip.totalUnits });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("increment")]
        public IActionResult IncrementInstallments()
        {
            _repository.IncrementInstallments();
            return Ok(new { Message = "SIP installments incremented" });
        }
    }
}