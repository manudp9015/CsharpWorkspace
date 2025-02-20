using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MutualFundSimulatorApplication
{
    internal class MutualFundBusiness
    {
        private MutualFundRepository _repository;

        public MutualFundBusiness(MutualFundRepository repository)
        {
            _repository = repository;
        }
        public bool AuthenticateUser()
        {
            return _repository.AuthenticateUser();
        }
        public void SaveUserDetails()
        {
            _repository.SaveUserDetails();
        }
        public void SaveLumpsumInvest()
        {
            _repository.SaveLumpsumInvest();
                
        }
        public void SaveSIPInvest()
        {
            _repository.SaveSIPInvest();
        }
        public decimal GetFundPrice(string fundName)
        {
            return _repository.GetFundPrice(fundName);
        }   
        public void UpdateCurrentAmountsForAllInvestments()
        {
            _repository.UpdateCurrentAmountsForAllInvestments();
        }
        public void UpdateCurrentAmount(int lumpsumid, decimal updatedCurrentAmount, SqlConnection connection)
        {
            _repository.UpdateCurrentAmount(lumpsumid, updatedCurrentAmount, connection);
        }
        public void IncrementInstallments()
        {
            _repository.IncrementInstallments();    
        }

        public void DisplayLumpSumPortfolio()
        {
            _repository.DisplayLumpSumPortfolio();
        }
        public void DisplaySIPPortfolio()
        {
            _repository.DisplaySIPPortfolio();
        }
        public void GetUpcomingSIPInstallments()
        {
            _repository.GetUpcomingSIPInstallments();
        }

        public void UpdateFundNav()
        {
            _repository.UpdateFundNav();
        }
        public decimal GetLatestNAV(SqlConnection connection, string fundName)
        {
            return _repository.GetLatestNAV(connection, fundName);
        }
        public bool IsNavAlreadyUpdated()
        {
            return _repository.IsNavAlreadyUpdated();
        }
    }
}
