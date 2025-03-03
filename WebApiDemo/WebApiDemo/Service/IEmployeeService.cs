// WebApiDemo/Services/IEmployeeService.cs
using WebApiDemo.Models.Entity;

namespace WebApiDemo.Services
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAllEmployees();
        Task<Employee> GetEmployeeById(Guid id);
        Task<Employee> AddEmployee(EmployeeDto employeeDto);
        Task<Employee> UpdateEmployee(Guid id, UpdateEmployeeDto updateEmployeeDto);
        Task<bool> DeleteEmployee(Guid id);
    }
}