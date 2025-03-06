using Microsoft.AspNetCore.Mvc;
using MutualFundSimulatorService.Business.Interfaces;

namespace MutualFundSimulatorApplication.Controllers
{
    [Route("api/nav")]
    [ApiController]
    public class NavController : ControllerBase
    {
        private readonly INavService _navService;

        public NavController(INavService navService)
        {
            _navService = navService;
        }

        [HttpPut]
        [Route("update")]
        public IActionResult UpdateFundNav()
        {
            return _navService.UpdateFundNav();
        }

        [HttpGet]
        [Route("latest")]
        public IActionResult GetLatestNAV([FromQuery] string fundName)
        {
            return _navService.GetLatestNAV(fundName);
        }

        [HttpGet]
        [Route("updated")]
        public IActionResult IsNavAlreadyUpdated()
        {
            return _navService.IsNavAlreadyUpdated();
        }
    }
}
