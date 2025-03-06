using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MutualFundSimulatorService.Business.Interfaces
{
    public interface IFundService
    {
        IActionResult GetMainFunds();
        IActionResult GetEquityFunds();
        IActionResult GetDebtFunds();
        IActionResult GetIndexFunds();
        IActionResult GetBalancedFunds();
        IActionResult GetCommodityFunds();
        IActionResult GetFundDetails(string fundName);
        IActionResult GetFundPrice(string fundName);
    }
}