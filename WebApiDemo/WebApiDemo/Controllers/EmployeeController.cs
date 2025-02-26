using Microsoft.AspNetCore.Mvc;
using WebApiDemo.Data;
using WebApiDemo.Models.Entity;

namespace WebApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class EmployeeController:ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        public EmployeeController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult GetAllEmployees()
        {
           // var allEmployees = dbContext.Emplyees.ToList();

            //return Ok(allEmployees);
            return Ok(dbContext.Emplyees.ToList());
        }
        [HttpPut]
        public IActionResult AddEmployee(EmployeeDto employeeDto)
        {
            var addEmployeeEntity = new Employee() { 
                Phone = employeeDto.Phone,
                Name = employeeDto.Name,
                Salary = employeeDto.Salary,
                Email = employeeDto.Email
                };
            dbContext.Emplyees.Add(addEmployeeEntity);
            dbContext.SaveChanges();
            return Ok(addEmployeeEntity);
        }
    }
}
