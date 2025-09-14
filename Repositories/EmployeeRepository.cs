using Microsoft.EntityFrameworkCore;
using DepartmentEmployeeSystem.API.Data;
using DepartmentEmployeeSystem.API.Interfaces;
using DepartmentEmployeeSystem.API.Models;

namespace DepartmentEmployeeSystem.API.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> IsEmailUniqueAsync(string email, int? excludeId = null)
        {
            var query = _dbSet.Where(e => e.EmailAddress.ToLower() == email.ToLower());
            if (excludeId.HasValue)
            {
                query = query.Where(e => e.EmployeeId != excludeId.Value);
            }
            return !await query.AnyAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(int departmentId)
        {
            return await _dbSet
                .Include(e => e.Department)
                .Where(e => e.DepartmentId == departmentId && e.IsActive)
                .ToListAsync();
        }

        public override async Task<IEnumerable<Employee>> GetAllAsync(System.Linq.Expressions.Expression<Func<Employee, object>>[]? includes = null)
        {
            return await _dbSet
                .Include(e => e.Department)
                .Where(e => e.IsActive)
                .ToListAsync();
        }

        public override async Task<Employee?> GetByIdAsync(int id, System.Linq.Expressions.Expression<Func<Employee, object>>[]? includes = null)
        {
            return await _dbSet
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);
        }
    }
}
