using Microsoft.AspNetCore.Mvc;
using MutualFundSimulatorService.Model.DTO;

namespace MutualFundSimulatorService.Business.Interfaces
{
    public interface ISipService
    {
        IActionResult SaveSIPInvest(int id, SaveSIPInvestDto saveSIPInvestDto);
        IActionResult IncrementInstallments(int id); // Update signature
    }
}