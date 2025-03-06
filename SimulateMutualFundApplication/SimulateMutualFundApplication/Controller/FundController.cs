using Microsoft.AspNetCore.Mvc;
using MutualFundSimulatorService.Business.Interfaces;

namespace MutualFundSimulatorApplication.Controllers
{
    [ApiController]
    public class FundController : ControllerBase
    {
        private readonly IFundService _fundService;

        public FundController(IFundService fundService)
        {
            _fundService = fundService;
        }

        [HttpGet]
        [Route("api/funds")]
        public IActionResult GetMainFunds()
        {
            return _fundService.GetMainFunds();
        }

        [HttpGet]
        [Route("api/funds/equity")]
        public IActionResult GetEquityFunds()
        {
            return _fundService.GetEquityFunds();
        }

        [HttpGet]
        [Route("api/funds/debt")]
        public IActionResult GetDebtFunds()
        {
            return _fundService.GetDebtFunds();
        }

        [HttpGet]
        [Route("api/funds/index")]
        public IActionResult GetIndexFunds()
        {
            return _fundService.GetIndexFunds();
        }

        [HttpGet]
        [Route("api/funds/balanced")]
        public IActionResult GetBalancedFunds()
        {
            return _fundService.GetBalancedFunds();
        }

        [HttpGet]
        [Route("api/funds/commodity")]
        public IActionResult GetCommodityFunds()
        {
            return _fundService.GetCommodityFunds();
        }

        [HttpGet]
        [Route("api/fund/details")]
        public IActionResult GetFundDetails([FromQuery] string fundName)
        {
            return _fundService.GetFundDetails(fundName);
        }

        [HttpGet]
        [Route("api/fund/price")]
        public IActionResult GetFundPrice([FromQuery] string fundName)
        {
            return _fundService.GetFundPrice(fundName);
        }
    }
}
