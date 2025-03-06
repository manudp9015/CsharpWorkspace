using Microsoft.AspNetCore.Mvc;
using MutualFundSimulatorService.Business.Interfaces;
using MutualFundSimulatorService.Repository.Interface;
using System.Collections.Generic;
using System.Linq;

namespace MutualFundSimulatorService.Business.ConcreteClass
{
    public class FundService : IFundService
    {
        private readonly IRepository _repository;

        public FundService(IRepository repository)
        {
            _repository = repository;
        }

        public IActionResult GetMainFunds()
        {
            var mainFunds = new List<string> { "Equity", "Debt", "Index", "Balanced", "Commodity" };
            return new OkObjectResult(mainFunds);
        }

        public IActionResult GetEquityFunds()
        {
            var allFunds = FundDetailsUtility.GetFundDetails();
            var equityFunds = new List<string>
            {
                "Large-Cap Equity Fund", "Mid-Cap Equity Fund", "Small-Cap Equity Fund",
                "Sectoral/Thematic Fund", "Multi-Cap Fund"
            };
            var result = equityFunds.Where(fund => allFunds.ContainsKey(fund)).ToList();
            return new OkObjectResult(result);
        }

        public IActionResult GetDebtFunds()
        {
            var allFunds = FundDetailsUtility.GetFundDetails();
            var debtFunds = new List<string>
            {
                "Overnight Fund", "Liquid Fund", "Ultra-Short Term Fund",
                "Short-Term Debt Fund", "Low Duration Fund"
            };
            var result = debtFunds.Where(fund => allFunds.ContainsKey(fund)).ToList();
            return new OkObjectResult(result);
        }

        public IActionResult GetIndexFunds()
        {
            var allFunds = FundDetailsUtility.GetFundDetails();
            var indexFunds = new List<string>
            {
                "Nifty 50 Index Fund", "Sensex Index Fund", "Nifty Next 50 Index Fund",
                "Nifty Bank Index Fund", "Nifty IT Index Fund"
            };
            var result = indexFunds.Where(fund => allFunds.ContainsKey(fund)).ToList();
            return new OkObjectResult(result);
        }

        public IActionResult GetBalancedFunds()
        {
            var allFunds = FundDetailsUtility.GetFundDetails();
            var balancedFunds = new List<string>
            {
                "Aggressive Hybrid Fund", "Conservative Hybrid Fund", "Dynamic Asset Allocation Fund",
                "Balanced Advantage Fund", "Multi-Asset Allocation Fund"
            };
            var result = balancedFunds.Where(fund => allFunds.ContainsKey(fund)).ToList();
            return new OkObjectResult(result);
        }

        public IActionResult GetCommodityFunds()
        {
            var allFunds = FundDetailsUtility.GetFundDetails();
            var commodityFunds = new List<string>
            {
                "Gold ETF Fund", "Silver ETF Fund", "Multi-Commodity Fund",
                "Energy Commodity Fund", "Agricultural Commodity Fund"
            };
            var result = commodityFunds.Where(fund => allFunds.ContainsKey(fund)).ToList();
            return new OkObjectResult(result);
        }

        public IActionResult GetFundDetails(string fundName)
        {
            if (string.IsNullOrWhiteSpace(fundName))
                return new BadRequestObjectResult(new { Message = "Fund name is required" });

            var fundDetails = FundDetailsUtility.GetFundDetails();
            if (fundDetails.ContainsKey(fundName))
            {
                var (description, risk, expenseRatio) = fundDetails[fundName];
                return new OkObjectResult(new
                {
                    FundName = fundName,
                    Description = description,
                    Risk = risk,
                    ExpenseRatio = expenseRatio
                });
            }
            return new NotFoundObjectResult(new { Message = $"Fund '{fundName}' not found" });
        }

        public IActionResult GetFundPrice(string fundName)
        {
            if (string.IsNullOrWhiteSpace(fundName))
                return new BadRequestObjectResult(new { Message = "Fund name is required" });

            decimal price = _repository.GetFundPrice(fundName);
            return new OkObjectResult(new { FundName = fundName, Price = price });
        }
    }
}