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
    public class NavController : ControllerBase
    {
        private readonly MutualFundRepository _repository;

        public NavController(MutualFundRepository repository)
        {
            _repository = repository;
        }
        [HttpPut("nav/update")]
        public IActionResult UpdateFundNav()
        {
            _repository.UpdateFundNav();
            return Ok(new { Message = "NAV values updated" });
        }

        [HttpGet("nav/latest")]
        public IActionResult GetLatestNAV([FromQuery] string fundName)
        {
            if (string.IsNullOrWhiteSpace(fundName))
                return BadRequest(new { Message = "Fund name is required" });

            using (var connection = new SqlConnection(_repository.ConnectionString))
            {
                connection.Open();
                decimal nav = _repository.GetLatestNAV(connection, fundName);
                return Ok(new { FundName = fundName, NAV = nav });
            }
        }

        [HttpGet("nav/updated")]
        public IActionResult IsNavAlreadyUpdated()
        {
            bool updated = _repository.IsNavAlreadyUpdated();
            return Ok(new { IsUpdated = updated });
        }
    }
}