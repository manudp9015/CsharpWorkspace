using Microsoft.AspNetCore.Mvc;
using WebApiDemo.Models.Entity;
using WebApiDemo.Services;

namespace WebApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            return Ok(employeeService.GetAllEmployees());
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetEmployee(Guid id)
        {
            var employee = employeeService.GetEmployeeById(id);
            if(employee is null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpPost]
        public IActionResult AddEmployee(EmployeeDto employeeDto)
        {
            var addedEmployee = employeeService.AddEmployee(employeeDto);
            return Ok(addedEmployee);
        }

        [HttpPut("{id:guid}")]
        public IActionResult UpdateEmployee(Guid id, UpdateEmployeeDto updateEmployeeDto)
        {
            var updatedEmployee = employeeService.UpdateEmployee(id, updateEmployeeDto);
            if(updatedEmployee is null)
            {
                return NotFound();
            }
            return Ok(updatedEmployee);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteEmployee(Guid id)
        {  
            if (employeeService.DeleteEmployee(id))
            {
                return Ok("Deleted Successfully");
            }
            return NotFound();
        }
    }
}
