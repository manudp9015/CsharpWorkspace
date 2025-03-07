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
            try
            {
                return _fundService.GetMainFunds();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("api/funds/equity")]
        public IActionResult GetEquityFunds()
        {
            try
            {
                return _fundService.GetEquityFunds();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("api/funds/debt")]
        public IActionResult GetDebtFunds()
        {
            try
            {
                return _fundService.GetDebtFunds();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("api/funds/index")]
        public IActionResult GetIndexFunds()
        {
            try
            {
                return _fundService.GetIndexFunds();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("api/funds/balanced")]
        public IActionResult GetBalancedFunds()
        {
            try
            {
                return _fundService.GetBalancedFunds();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("api/funds/commodity")]
        public IActionResult GetCommodityFunds()
        {
            try
            {
                return _fundService.GetCommodityFunds();

            }
            catch (Exception)
            {
                throw;
            }        
        }

        [HttpGet]
        [Route("api/fund/details")]
        public IActionResult GetFundDetails([FromQuery] string fundName)
        {
            try
            {
                return _fundService.GetFundDetails(fundName);

            }
            catch (Exception)
            {
                throw;
            }        
        }

        [HttpGet]
        [Route("api/fund/price")]
        public IActionResult GetFundPrice([FromQuery] string fundName)
        {
            try
            {
                return _fundService.GetFundPrice(fundName);

            }
            catch (Exception)
            {
                throw;
            }        
        }
    }
}
