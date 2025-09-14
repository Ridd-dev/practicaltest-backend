using Microsoft.EntityFrameworkCore;
using DepartmentEmployeeSystem.API.Data;
using DepartmentEmployeeSystem.API.Interfaces;
using DepartmentEmployeeSystem.API.Models;

namespace DepartmentEmployeeSystem.API.Repositories
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> IsDepartmentCodeUniqueAsync(string departmentCode, int? excludeId = null)
        {
            var query = _dbSet.Where(d => d.DepartmentCode.ToLower() == departmentCode.ToLower());
            if (excludeId.HasValue)
            {
                query = query.Where(d => d.DepartmentId != excludeId.Value);
            }
            return !await query.AnyAsync();
        }

        public async Task<IEnumerable<Department>> GetDepartmentsWithEmployeeCountAsync()
        {
            return await _dbSet
                .Include(d => d.Employees.Where(e => e.IsActive))
                .ToListAsync();
        }
    }
}
