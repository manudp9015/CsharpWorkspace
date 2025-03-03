// WebApiDemo/Repositories/EmployeeRepository.cs
using Microsoft.EntityFrameworkCore;
using WebApiDemo.Data;
using WebApiDemo.Models.Entity;
using WebApiDemo.Repository;

namespace WebApiDemo.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public EmployeeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            return await _dbContext.Emplyees.ToListAsync();
        }

        public async Task<Employee> GetByIdAsync(Guid id)
        {
            return await _dbContext.Emplyees.FindAsync(id);
        }

        public async Task<Employee> AddAsync(Employee employee)
        {
            employee.Id = Guid.NewGuid(); // Ensure a new ID is generated
            _dbContext.Emplyees.Add(employee);
            await _dbContext.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> UpdateAsync(Employee employee)
        {
            _dbContext.Emplyees.Update(employee);
            await _dbContext.SaveChangesAsync();
            return employee;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var employee = await _dbContext.Emplyees.FindAsync(id);
            if (employee == null)
                return false;

            _dbContext.Emplyees.Remove(employee);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}