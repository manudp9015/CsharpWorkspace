﻿using Microsoft.AspNetCore.Mvc;
using MutualFundSimulatorService.Business.Interfaces;
using MutualFundSimulatorService.Model.DTO;

namespace MutualFundSimulatorApplication.Controllers
{
    [Route("api/sip")]
    [ApiController]
    public class SipController : ControllerBase
    {
        private readonly ISipService _sipService;

        public SipController(ISipService sipService)
        {
            _sipService = sipService;
        }

        [HttpPost]
        [Route("invest/{id}")]
        public IActionResult SaveSIPInvest(int id, [FromBody] SaveSIPInvestDto saveSipInvestDto)
        {
            try
            {
                return _sipService.SaveSIPInvest(id, saveSipInvestDto);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPut]
        [Route("increment/{id}")]
        public IActionResult IncrementInstallments(int id)
        {
            try
            {
                return _sipService.IncrementInstallments(id);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}