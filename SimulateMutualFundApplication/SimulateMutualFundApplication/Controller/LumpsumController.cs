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
        [Route("api/lumpsum/invest/{id}/{fundName}")]
        public IActionResult SaveLumpsumInvest(int id, string fundName, [FromQuery] decimal amount)
        {
            try
            {
                return _lumpsumService.SaveLumpsumInvest(id, fundName, amount);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut]
        [Route("api/lumpsum/update")]
        public IActionResult UpdateCurrentAmountsForAllInvestments()
        {
            try
            {
                return _lumpsumService.UpdateCurrentAmountsForAllInvestments();
            }
            catch (Exception)
            {
                throw;
            }        
        }
    }
}
