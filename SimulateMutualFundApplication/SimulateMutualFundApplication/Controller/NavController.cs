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
            try
            {
                return _navService.UpdateFundNav();
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        [Route("latest")]
        public IActionResult GetLatestNAV([FromQuery] string fundName)
        {
            try
            {
                return _navService.GetLatestNAV(fundName);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        [Route("updated")]
        public IActionResult IsNavAlreadyUpdated()
        {
            try
            {
                return _navService.IsNavAlreadyUpdated();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
