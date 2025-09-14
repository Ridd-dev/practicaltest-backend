using DepartmentEmployeeSystem.API.Models;

namespace DepartmentEmployeeSystem.API.Interfaces
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<bool> IsEmailUniqueAsync(string email, int? excludeId = null);
        Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(int departmentId);
    }
}
