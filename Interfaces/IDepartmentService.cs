using DepartmentEmployeeSystem.API.Models;

namespace DepartmentEmployeeSystem.API.Interfaces
{
    public interface IDepartmentService
    {
        Task<List<Department>> GetAllDepartmentsAsync();
        Task<Department?> GetDepartmentByIdAsync(int id);
        Task<Department> CreateDepartmentAsync(CreateDepartmentDto dto);
        Task<Department?> UpdateDepartmentAsync(int id, UpdateDepartmentDto dto);
        Task<bool> DeleteDepartmentAsync(int id);
    }
}
