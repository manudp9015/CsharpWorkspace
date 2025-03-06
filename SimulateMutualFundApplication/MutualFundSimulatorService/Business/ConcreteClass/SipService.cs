using Microsoft.AspNetCore.Mvc;
using MutualFundSimulatorService.Business.Interfaces;
using MutualFundSimulatorService.Model;
using MutualFundSimulatorService.Model.DTO;
using MutualFundSimulatorService.Repository.Interface;

namespace MutualFundSimulatorService.Business.ConcreteClass
{
    public class SipService : ISipService
    {
        private readonly IRepository _repository;
        private readonly User _user;
        private readonly UserSipInvest _userSip;

        public SipService(IRepository repository, User user, UserSipInvest userSip)
        {
            _repository = repository;
            _user = user;
            _userSip = userSip;
        }

        public IActionResult SaveSIPInvest(int id, SaveSIPInvestDto saveSIPInvestDto)
        {
            if (id <= 0 || string.IsNullOrWhiteSpace(saveSIPInvestDto.fundName) || saveSIPInvestDto.sipAmount < 1000 || saveSIPInvestDto.durationInMonths < 12)
                return new BadRequestObjectResult(new { Message = "User ID, fund name, SIP amount(Min 1000), and duration are required" });

            if (!_repository.IsValidUserId(id))
                return new BadRequestObjectResult(new { Message = "Invalid user ID" });

            try
            {
                _user.id = id;
                decimal pricePerUnit = _repository.GetFundPrice(saveSIPInvestDto.fundName);
                if (pricePerUnit <= 0) throw new Exception("Invalid fund price");

                var fundDetails = FundDetailsUtility.GetFundDetails();
                decimal expenseRatio = fundDetails.ContainsKey(saveSIPInvestDto.fundName) ? fundDetails[saveSIPInvestDto.fundName].expenseRatio : 0.070m;
                decimal expense = saveSIPInvestDto.sipAmount * expenseRatio / 100m;
                decimal netAmount = saveSIPInvestDto.sipAmount - expense;

                _userSip.fundName = saveSIPInvestDto.fundName;
                _userSip.sipAmount = saveSIPInvestDto.sipAmount;
                _userSip.sipStartDate = saveSIPInvestDto.sipStartDate;
                _userSip.durationInMonths = saveSIPInvestDto.durationInMonths;
                _userSip.nextInstallmentDate = saveSIPInvestDto.sipStartDate.AddMonths(1);
                _userSip.sipEndDate = saveSIPInvestDto.sipStartDate.AddMonths(saveSIPInvestDto.durationInMonths);
                _userSip.totalUnits = netAmount / pricePerUnit;
                _userSip.totalInstallments = 1;
                _userSip.totalInvestedAmount = netAmount;
                _userSip.currentAmount = _userSip.totalUnits * pricePerUnit;

                _repository.AddMoneyToWallet(-saveSIPInvestDto.sipAmount);
                _repository.SaveExpense(saveSIPInvestDto.fundName, expense, saveSIPInvestDto.sipStartDate);
                _repository.SaveSIPInvest();

                return new OkObjectResult(new { Message = "SIP investment saved", Quantity = _userSip.totalUnits });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Message = ex.Message });
            }
        }

        public IActionResult IncrementInstallments(int id)
        {
            if (id <= 0)
                return new BadRequestObjectResult(new { Message = "User ID is required" });

            if (!_repository.IsValidUserId(id))
                return new BadRequestObjectResult(new { Message = "Invalid user ID" });

            _user.id = id; 
            _repository.IncrementInstallments();
            return new OkObjectResult(new { Message = "SIP installments incremented" });
        }
    }
}