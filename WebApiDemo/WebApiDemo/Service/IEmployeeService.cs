using WebApiDemo.Models.Entity;

namespace WebApiDemo.Services
{
    public interface IEmployeeService
    {
        List<Employee> GetAllEmployees();
        Employee GetEmployeeById(Guid id);
        Employee AddEmployee(EmployeeDto employeeDto);
        Employee UpdateEmployee(Guid id, UpdateEmployeeDto updateEmployeeDto);
        bool DeleteEmployee(Guid id);
    }
}
