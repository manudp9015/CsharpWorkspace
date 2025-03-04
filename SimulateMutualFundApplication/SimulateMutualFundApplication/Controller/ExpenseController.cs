using Microsoft.AspNetCore.Mvc;
using MutualFundSimulatorService.Model;
using MutualFundSimulatorService.Repository;

namespace MutualFundSimulatorApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly MutualFundRepository _repository;
        private readonly User _user;

        public ExpenseController(
            MutualFundRepository repository,
            User user)
        {
            _repository = repository;
            _user = user;
        }

        [HttpPost]
        public IActionResult SaveExpense([FromBody] Expense request)
        {
            if (string.IsNullOrWhiteSpace(request.FundName) || request.ExpenseAmount <= 0)
                return BadRequest(new { Message = "Fund name and expense amount are required" });

            _repository.SaveExpense(request.FundName, request.ExpenseAmount, request.ExpenseDate);
            return Ok(new { Message = "Expense saved" });
        }
    }
}