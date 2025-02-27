using WebApiDemo.Data;
using WebApiDemo.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace WebApiDemo.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext dbContext;

        public EmployeeService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<Employee> GetAllEmployees()
        {
            return dbContext.Emplyees.ToList();
        }

        public Employee GetEmployeeById(Guid id)
        {
            return dbContext.Emplyees.Find(id);
        }

        public Employee AddEmployee(EmployeeDto employeeDto)
        {
            Employee employeeEntity = new Employee()
            {
                Name = employeeDto.Name,
                Email = employeeDto.Email,
                Phone = employeeDto.Phone,
                Salary = employeeDto.Salary
            };
            dbContext.Emplyees.Add(employeeEntity);
            dbContext.SaveChanges();
            return employeeEntity;
        }

        public Employee UpdateEmployee(Guid id, UpdateEmployeeDto updateEmployeeDto)
        {
            var employee = dbContext.Emplyees.Find(id);
            if (employee is null)
                return null;

            employee.Phone = updateEmployeeDto.Phone;
            employee.Name = updateEmployeeDto.Name;
            employee.Salary = updateEmployeeDto.Salary;
            employee.Email = updateEmployeeDto.Email;

            dbContext.SaveChanges();
            return employee;
        }

        public bool DeleteEmployee(Guid id)
        {
            var employee = dbContext.Emplyees.Find(id);
            if (employee == null)
                return false;

            dbContext.Emplyees.Remove(employee);
            dbContext.SaveChanges();
            return true;
        }
    }
}
