using Microsoft.AspNetCore.Mvc;
using System;

namespace MutualFundSimulatorService.Business.Interfaces
{
    public interface ILumpsumService
    {
        IActionResult SaveLumpsumInvest(int id, string fundName, decimal amount);
        IActionResult UpdateCurrentAmountsForAllInvestments();
    }
}