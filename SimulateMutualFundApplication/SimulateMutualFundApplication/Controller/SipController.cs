using Microsoft.AspNetCore.Mvc;
using MutualFundSimulatorService.Business.Interfaces;
using MutualFundSimulatorService.Model.DTO;

namespace MutualFundSimulatorApplication.Controllers
{
    [Route("api/sip")]
    [ApiController]
    public class SipController : ControllerBase
    {
        private readonly ISipService _sipService;

        public SipController(ISipService sipService)
        {
            _sipService = sipService;
        }

        [HttpPost]
        [Route("invest")]
        public IActionResult SaveSIPInvest([FromQuery] int id, [FromBody] SaveSIPInvestDto saveSipInvestDto)
        {
            return _sipService.SaveSIPInvest(id, saveSipInvestDto);
        }

        [HttpPut]
        [Route("increment")]
        public IActionResult IncrementInstallments([FromQuery] int id)
        {
            return _sipService.IncrementInstallments(id);
        }
    }
}