using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MutualFundSimulatorService.Business.Interfaces;
using MutualFundSimulatorService.Repository.Interface;

namespace MutualFundSimulatorService.Business.ConcreteClass
{
    public class NavService : INavService
    {
        private readonly IRepository _repository;

        public NavService(IRepository repository)
        {
            _repository = repository;
        }

        public IActionResult UpdateFundNav()
        {
            try
            {
                _repository.UpdateFundNav();
                return new OkObjectResult(new { Message = "NAV values updated" });
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IActionResult GetLatestNAV(string fundName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fundName))
                    return new BadRequestObjectResult(new { Message = "Fund name is required" });

                using (var connection = new SqlConnection(_repository.ConnectionString))
                {
                    connection.Open();
                    decimal nav = _repository.GetLatestNAV(connection, fundName);
                    return new OkObjectResult(new { FundName = fundName, NAV = nav });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IActionResult IsNavAlreadyUpdated()
        {
            try
            {
                bool updated = _repository.IsNavAlreadyUpdated();
                return new OkObjectResult(new { IsUpdated = updated });
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}