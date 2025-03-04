using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MutualFundSimulatorService.Business;
using MutualFundSimulatorService.Model;
using MutualFundSimulatorService.Repository;
using System.Collections.Generic;
using System.Linq;

namespace MutualFundSimulatorApplication.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class FundController : ControllerBase
    {
        private readonly MutualFundRepository _repository;

        public FundController(MutualFundRepository repository)
        {
            _repository = repository;
        }
        
        [HttpGet("funds")]
        public IActionResult GetMainFunds()
        {
            var mainFunds = new List<string>();
            mainFunds.Add("Equity");
            mainFunds.Add("Debt");
            mainFunds.Add("Index");
            mainFunds.Add("Balanced");
            mainFunds.Add("Commodity");
            return Ok(mainFunds);
        }

        [HttpGet("funds/equity")]
        public IActionResult GetEquityFunds()
        {
            var allFunds = MutualFundSimulatorUtility.GetFundDetails();
            var equityFunds = new List<string>();
            equityFunds.Add("Large-Cap Equity Fund");
            equityFunds.Add("Mid-Cap Equity Fund");
            equityFunds.Add("Small-Cap Equity Fund");
            equityFunds.Add("Sectoral/Thematic Fund");
            equityFunds.Add("Multi-Cap Fund");

            var result = equityFunds.Where(fund => allFunds.ContainsKey(fund)).ToList();
            return Ok(result);
        }

        [HttpGet("funds/debt")]
        public IActionResult GetDebtFunds()
        {
            var allFunds = MutualFundSimulatorUtility.GetFundDetails();
            var debtFunds = new List<string>();
            debtFunds.Add("Overnight Fund");
            debtFunds.Add("Liquid Fund");
            debtFunds.Add("Ultra-Short Term Fund");
            debtFunds.Add("Short-Term Debt Fund");
            debtFunds.Add("Low Duration Fund");
            var result = debtFunds.Where(fund => allFunds.ContainsKey(fund)).ToList();
            return Ok(result);
        }

        [HttpGet("funds/index")]
        public IActionResult GetIndexFunds()
        {
            var allFunds = MutualFundSimulatorUtility.GetFundDetails();
            var indexFunds = new List<string>();
            indexFunds.Add("Nifty 50 Index Fund");
            indexFunds.Add("Sensex Index Fund");
            indexFunds.Add("Nifty Next 50 Index Fund");
            indexFunds.Add("Nifty Bank Index Fund");
            indexFunds.Add("Nifty IT Index Fund");
            var result = indexFunds.Where(fund => allFunds.ContainsKey(fund)).ToList();
            return Ok(result);
        }

        [HttpGet("funds/balanced")]
        public IActionResult GetBalancedFunds()
        {
            var allFunds = MutualFundSimulatorUtility.GetFundDetails();
            var balancedFunds = new List<string>();
            balancedFunds.Add("Aggressive Hybrid Fund");
            balancedFunds.Add("Conservative Hybrid Fund");
            balancedFunds.Add("Dynamic Asset Allocation Fund");
            balancedFunds.Add("Balanced Advantage Fund");
            balancedFunds.Add("Multi-Asset Allocation Fund");
            var result = balancedFunds.Where(fund => allFunds.ContainsKey(fund)).ToList();
            return Ok(result);
        }

        [HttpGet("funds/commodity")]
        public IActionResult GetCommodityFunds()
        {
            var allFunds = MutualFundSimulatorUtility.GetFundDetails();
            var commodityFunds = new List<string>();
            commodityFunds.Add("Gold ETF Fund");
            commodityFunds.Add("Silver ETF Fund");
            commodityFunds.Add("Multi-Commodity Fund");
            commodityFunds.Add("Energy Commodity Fund");
            commodityFunds.Add("Agricultural Commodity Fund");
            var result = commodityFunds.Where(fund => allFunds.ContainsKey(fund)).ToList();
            return Ok(result);
        }

        [HttpGet("fund/details")]
        public IActionResult GetFundDetails([FromQuery] string fundName)
        {
            if (string.IsNullOrWhiteSpace(fundName))
                return BadRequest(new { Message = "Fund name is required" });

            var fundDetails = MutualFundSimulatorUtility.GetFundDetails();
            if (fundDetails.ContainsKey(fundName))
            {
                var (description, risk, expenseRatio) = fundDetails[fundName];
                return Ok(new
                {
                    FundName = fundName,
                    Description = description,
                    Risk = risk,
                    ExpenseRatio = expenseRatio
                });
            }
            return NotFound(new { Message = $"Fund '{fundName}' not found" });
        }

        [HttpGet("fund/price")]
        public IActionResult GetFundPrice([FromQuery] string fundName)
        {
            if (string.IsNullOrWhiteSpace(fundName))
                return BadRequest(new { Message = "Fund name is required" });

            decimal price = _repository.GetFundPrice(fundName);
            return Ok(new { FundName = fundName, Price = price });
        }
    }
}