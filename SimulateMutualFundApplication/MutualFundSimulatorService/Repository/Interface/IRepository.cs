using MutualFundSimulatorService.Model;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace MutualFundSimulatorService.Repository.Interface
{
    public interface IRepository
    {
        bool AuthenticateUser();
        bool SaveUserDetails(User userDetails);
        void SaveLumpsumInvest(bool deducted);
        void SaveSIPInvest();
        decimal GetFundPrice(string fundName);
        void UpdateCurrentAmountsForAllInvestments();
        void UpdateCurrentAmount(int lumpsumId, decimal updatedCurrentAmount, SqlConnection connection);
        void IncrementInstallments();

        List<(string FundName, DateTime NextInstallmentDate)> GetUpcomingSIPInstallments();
        void UpdateFundNav();
        decimal GetLatestNAV(SqlConnection connection, string fundName);
        bool IsNavAlreadyUpdated();
        void SaveExpense(string fundName, decimal expenseAmount, DateTime expenseDate);
        void AddMoneyToWallet(decimal amount);
        bool IsValidUserId(int id);
        string ConnectionString { get; }
        List<LumpSumPortfolioItem> DisplayLumpSumPortfolio();
        List<SipPortfolioItem> DisplaySIPPortfolio();
        decimal GetWalletBalance();

    }
}
