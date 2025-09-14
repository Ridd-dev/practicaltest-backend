using DepartmentEmployeeSystem.API.Models;

namespace DepartmentEmployeeSystem.API.Interfaces
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        Task<bool> IsDepartmentCodeUniqueAsync(string departmentCode, int? excludeId = null);
        Task<IEnumerable<Department>> GetDepartmentsWithEmployeeCountAsync();
    }
}
