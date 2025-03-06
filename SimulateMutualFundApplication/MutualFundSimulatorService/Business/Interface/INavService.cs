using Microsoft.AspNetCore.Mvc;

namespace MutualFundSimulatorService.Business.Interfaces
{
    public interface INavService
    {
        IActionResult UpdateFundNav();
        IActionResult GetLatestNAV(string fundName);
        IActionResult IsNavAlreadyUpdated();
    }
}