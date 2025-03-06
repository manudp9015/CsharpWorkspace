using Microsoft.AspNetCore.Mvc;
using MutualFundSimulatorService.Repository;

namespace MutualFundSimulatorService.Business.Interfaces
{
    public interface IPortfolioService
    {
        IActionResult DisplayLumpSumPortfolio(int id);
        IActionResult DisplaySIPPortfolio(int id);
        IActionResult GetUpcomingSIPInstallments(int id);
    }
}