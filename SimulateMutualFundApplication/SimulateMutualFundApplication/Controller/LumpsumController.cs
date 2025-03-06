using Microsoft.AspNetCore.Mvc;
using MutualFundSimulatorService.Business.Interfaces;

namespace MutualFundSimulatorApplication.Controllers
{
    [ApiController]
    public class LumpsumController : ControllerBase
    {
        private readonly ILumpsumService _lumpsumService;

        public LumpsumController(ILumpsumService lumpsumService)
        {
            _lumpsumService = lumpsumService;
        }

        [HttpPost]
        [Route("api/lumpsum/invest")]
        public IActionResult SaveLumpsumInvest([FromQuery] int id, [FromQuery] string fundName, [FromQuery] decimal amount)
        {
            return _lumpsumService.SaveLumpsumInvest(id, fundName, amount);
        }

        [HttpPut]
        [Route("api/lumpsum/update")]
        public IActionResult UpdateCurrentAmountsForAllInvestments()
        {
            return _lumpsumService.UpdateCurrentAmountsForAllInvestments();
        }
    }
}
