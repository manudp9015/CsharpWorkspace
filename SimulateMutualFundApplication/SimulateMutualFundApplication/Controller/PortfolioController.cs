using Microsoft.AspNetCore.Mvc;
using MutualFundSimulatorService.Business.Interfaces;

namespace MutualFundSimulatorApplication.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly IPortfolioService _portfolioService;

        public PortfolioController(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        [HttpGet]
        [Route("lumpsum")]
        public IActionResult DisplayLumpSumPortfolio([FromQuery] int id)
        {
            return _portfolioService.DisplayLumpSumPortfolio(id);
        }

        [HttpGet]
        [Route("sip")]
        public IActionResult DisplaySIPPortfolio([FromQuery] int id)
        {
            return _portfolioService.DisplaySIPPortfolio(id);
        }

        [HttpGet]
        [Route("sip/upcoming")]
        public IActionResult GetUpcomingSIPInstallments([FromQuery] int id)
        {
            return _portfolioService.GetUpcomingSIPInstallments(id);
        }
    }
}
