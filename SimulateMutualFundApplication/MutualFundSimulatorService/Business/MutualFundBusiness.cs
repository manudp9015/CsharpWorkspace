using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using MutualFundSimulatorService.Model;
using MutualFundSimulatorService.Repository;
using System;

namespace MutualFundSimulatorService.Business
{
    public class MutualFundBusiness
    {
        private MutualFundRepository _repository;

        public MutualFundBusiness(MutualFundRepository repository)
        {
             _repository = repository;
        }

        /// <summary>
        /// Gets the connection string from the repository.
        /// </summary>
        /// <returns></returns>
        public string GetConnectionString()
        {
            return _repository.ConnectionString;
        }

        /// <summary>
        /// Authenticates a user by calling AuthenticateUser method in the repository layer.
        /// </summary>
        /// <returns></returns>
        public bool AuthenticateUser()
        {
            return _repository.AuthenticateUser();
        }

        /// <summary>
        /// Saves user details by calling the repository's SaveUserDetails method.
        /// </summary>
        public bool SaveUserDetails(User userDetails)
        {
            return _repository.SaveUserDetails(userDetails);
        }

        /// <summary>
        /// Saves lump sum investment details by calling the repository's SaveLumpsumInvest method.
        /// </summary>
        /// <param name="deducted"></param>
        public void SaveLumpsumInvest(bool deducted)
        {
            _repository.SaveLumpsumInvest(deducted);
        }

        /// <summary>
        /// Saves SIP investment details by calling the repository's SaveSIPInvest method.
        /// </summary>
        public void SaveSIPInvest()
        {
            _repository.SaveSIPInvest();
        }

        /// <summary>
        /// Retrieves the current fund price for a specified fund from the repository.
        /// </summary>
        /// <param name="fundName"></param>
        /// <returns></returns>
        public decimal GetFundPrice(string fundName)
        {
            return _repository.GetFundPrice(fundName);
        }

        /// <summary>
        /// Updates current amounts for all lump sum investments via the repository.
        /// </summary>
        public void UpdateCurrentAmountsForAllInvestments()
        {
            _repository.UpdateCurrentAmountsForAllInvestments();
        }

        /// <summary>
        /// Updates the current amount for a specific lump sum investment through the repository.
        /// </summary>
        /// <param name="lumpsumid"></param>
        /// <param name="updatedCurrentAmount"></param>
        /// <param name="connection"></param>
        public void UpdateCurrentAmount(int lumpsumid, decimal updatedCurrentAmount, SqlConnection connection)
        {
            _repository.UpdateCurrentAmount(lumpsumid, updatedCurrentAmount, connection);
        }

        /// <summary>
        /// Increments SIP installments by calling the repository's IncrementInstallments method.
        /// </summary>
        public void IncrementInstallments()
        {
            _repository.IncrementInstallments();
        }

        /// <summary>
        /// Displays the lump sum portfolio and returns profit/loss by calling the repository.
        /// </summary>
        /// <returns></returns>
        public decimal DisplayLumpSumPortfolio()
        {
            return _repository.DisplayLumpSumPortfolio();
        }

        /// <summary>
        /// Displays the SIP portfolio and returns profit/loss by calling the repository.
        /// </summary>
        /// <returns></returns>
        public decimal DisplaySIPPortfolio()
        {
            return _repository.DisplaySIPPortfolio();
        }

        /// <summary>
        /// Retrieves upcoming SIP installments by calling the repository's GetUpcomingSIPInstallments method.
        /// </summary>
        public void GetUpcomingSIPInstallments()
        {
            _repository.GetUpcomingSIPInstallments();
        }

        /// <summary>
        /// Updates fund NAV values by calling the repository's update method.
        /// </summary>
        public void UpdateFundNav()
        {
            _repository.UpdateFundNav();
        }

        /// <summary>
        /// Fetches the latest NAV value for a fund using an existing connection via the repository.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="fundName"></param>
        /// <returns></returns>
        public decimal GetLatestNAV(SqlConnection connection, string fundName)
        {
            return _repository.GetLatestNAV(connection, fundName);
        }

        /// <summary>
        /// Checks if NAV values have been updated for the current date through the repository.
        /// </summary>
        /// <returns></returns>
        public bool IsNavAlreadyUpdated()
        {
            return _repository.IsNavAlreadyUpdated();
        }

        /// <summary>
        /// Saves an expense entry for a fund by calling the repository's SaveExpense method.
        /// </summary>
        /// <param name="fundName"></param>
        /// <param name="expenseAmount"></param>
        /// <param name="expenseDate"></param>
        public void SaveExpense(string fundName, decimal expenseAmount, DateTime expenseDate)
        {
            _repository.SaveExpense(fundName, expenseAmount, expenseDate);
        }

        /// <summary>
        /// Adds or subtracts money from the user's wallet by calling AddMoneyToWallet repository's method.
        /// </summary>
        /// <param name="amount"></param>
        public void AddMoneyToWallet(decimal amount)
        {
            _repository.AddMoneyToWallet(amount);
        }
    }
}