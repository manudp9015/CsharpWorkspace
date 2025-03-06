using Microsoft.AspNetCore.Mvc;
using MutualFundSimulatorService.Business.Interfaces;
using MutualFundSimulatorService.Model;
using MutualFundSimulatorService.Repository.Interface;

namespace MutualFundSimulatorService.Business.ConcreteClass
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IRepository _repository;
        private readonly User _user;

        public PortfolioService(IRepository repository, User user)
        {
            _repository = repository;
            _user = user;
        }

        public IActionResult DisplayLumpSumPortfolio(int id)
        {
            if (id <= 0)
                return new BadRequestObjectResult(new { Message = "User ID is required" });

            if (!_repository.IsValidUserId(id))
                return new BadRequestObjectResult(new { Message = "Invalid user ID" });

            _user.id = id;
            var portfolio = _repository.DisplayLumpSumPortfolio();
            decimal walletBalance = _repository.GetWalletBalance();

            if (portfolio.Count == 0)
                return new OkObjectResult(new
                {
                    Message = "No lumpsum portfolio found for this user",
                    UserId = id,
                    WalletBalance = walletBalance,
                    Portfolio = new List<object>()
                });

            return new OkObjectResult(new
            {
                UserId = id,
                WalletBalance = walletBalance,
                Portfolio = portfolio
            });
        }

        public IActionResult DisplaySIPPortfolio(int id)
        {
            if (id <= 0)
                return new BadRequestObjectResult(new { Message = "User ID is required" });

            if (!_repository.IsValidUserId(id))
                return new BadRequestObjectResult(new { Message = "Invalid user ID" });

            _user.id = id;
            var portfolio = _repository.DisplaySIPPortfolio();
            decimal walletBalance = _repository.GetWalletBalance();

            if (portfolio.Count == 0)
                return new OkObjectResult(new
                {
                    Message = "No SIP portfolio found for this user",
                    UserId = id,
                    WalletBalance = walletBalance,
                    Portfolio = new List<object>()
                });

            return new OkObjectResult(new
            {
                UserId = id,
                WalletBalance = walletBalance,
                Portfolio = portfolio
            });
        }

        public IActionResult GetUpcomingSIPInstallments(int id)
        {
            if (id <= 0)
                return new BadRequestObjectResult(new { Message = "User ID is required" });

            if (!_repository.IsValidUserId(id))
                return new BadRequestObjectResult(new { Message = "Invalid user ID" });

            _user.id = id;
            var upcomingInstallments = _repository.GetUpcomingSIPInstallments();
            if (upcomingInstallments.Count == 0)
                return new OkObjectResult(new { Message = "No upcoming SIP installments found for this user", UpcomingInstallments = new List<object>() });

            var response = upcomingInstallments.Select(i => new { FundName = i.FundName, NextInstallmentDate = i.NextInstallmentDate });
            return new OkObjectResult(new { UserId = id, UpcomingInstallments = response });
        }
    }
}