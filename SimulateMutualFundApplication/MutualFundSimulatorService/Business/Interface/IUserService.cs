using Microsoft.AspNetCore.Mvc;
using MutualFundSimulatorService.Model;
using MutualFundSimulatorService.Model.DTO;

namespace MutualFundSimulatorService.Business.Interfaces
{
    public interface IUserService
    {
        IActionResult LoginUser(string userEmail, string password);
        IActionResult Register(UserDto userdto);
        IActionResult AddMoneyToWallet(int id, decimal amount);
    }
}

