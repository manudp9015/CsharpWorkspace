using Microsoft.AspNetCore.Mvc;
using MutualFundSimulatorService.Business.Interfaces;
using MutualFundSimulatorService.Model;
using MutualFundSimulatorService.Repository.Interface;

namespace MutualFundSimulatorService.Business.ConcreteClass
{
    public class LumpsumService : ILumpsumService
    {
        private readonly IRepository _repository;
        private readonly User _user;
        private readonly UserLumpsumInvest _userLumpsum;

        public LumpsumService(IRepository repository, User user, UserLumpsumInvest userLumpsum)
        {
            _repository = repository;
            _user = user;
            _userLumpsum = userLumpsum;
        }

        public IActionResult SaveLumpsumInvest(int id, string fundName, decimal amount)
        {
            if (id <= 0 || string.IsNullOrWhiteSpace(fundName) || amount < 5000)
                return new BadRequestObjectResult(new { Message = "User ID, fund name, and amount(Min 5000) are required" });

            if (!_repository.IsValidUserId(id))
                return new BadRequestObjectResult(new { Message = "Invalid user ID" });

            try
            {
                _user.id = id;
                decimal pricePerUnit = _repository.GetFundPrice(fundName);
                if (pricePerUnit <= 0) throw new Exception("Invalid fund price");

                var fundDetails = FundDetailsUtility.GetFundDetails();
                decimal expenseRatio = fundDetails.ContainsKey(fundName) ? fundDetails[fundName].expenseRatio : 0.070m;
                decimal expense = amount * expenseRatio / 100m;
                decimal netAmount = amount - expense;

                _userLumpsum.fundName = fundName;
                _userLumpsum.investedAmount = netAmount;
                _userLumpsum.quantity = netAmount / pricePerUnit;
                _userLumpsum.currentAmount = _userLumpsum.quantity * pricePerUnit;
                _userLumpsum.lumpsumStartDate = User.CurrentDate;

                _repository.AddMoneyToWallet(-amount);
                _repository.SaveExpense(fundName, expense, _userLumpsum.lumpsumStartDate);
                _repository.SaveLumpsumInvest(true);

                return new OkObjectResult(new { Message = "Lumpsum investment saved", Quantity = _userLumpsum.quantity });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Message = ex.Message });
            }
        }

        public IActionResult UpdateCurrentAmountsForAllInvestments()
        {
            _repository.UpdateCurrentAmountsForAllInvestments();
            return new OkObjectResult(new { Message = "Lumpsum amounts updated" });
        }
    }
}