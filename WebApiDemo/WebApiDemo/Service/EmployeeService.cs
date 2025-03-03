using WebApiDemo.Models.Entity;
using WebApiDemo.Repository;
using WebApiDemo.Repository;

namespace WebApiDemo.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<List<Employee>> GetAllEmployees()
        {
            return await _employeeRepository.GetAllAsync();
        }

        public async Task<Employee> GetEmployeeById(Guid id)
        {
            return await _employeeRepository.GetByIdAsync(id);
        }

        public async Task<Employee> AddEmployee(EmployeeDto employeeDto)
        {
            var employee = new Employee
            {
                Name = employeeDto.Name,
                Email = employeeDto.Email,
                Phone = employeeDto.Phone,
                Salary = employeeDto.Salary
            };
            return await _employeeRepository.AddAsync(employee);
        }

        public async Task<Employee> UpdateEmployee(Guid id, UpdateEmployeeDto updateEmployeeDto)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                return null;

            employee.Name = updateEmployeeDto.Name;
            employee.Email = updateEmployeeDto.Email;
            employee.Phone = updateEmployeeDto.Phone;
            employee.Salary = updateEmployeeDto.Salary;

            return await _employeeRepository.UpdateAsync(employee);
        }

        public async Task<bool> DeleteEmployee(Guid id)
        {
            return await _employeeRepository.DeleteAsync(id);
        }
    }
}