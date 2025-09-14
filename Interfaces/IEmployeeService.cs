using DepartmentEmployeeSystem.API.Models;

namespace DepartmentEmployeeSystem.API.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<Employee?> GetEmployeeByIdAsync(int id);
        Task<List<Employee>> GetEmployeesByDepartmentAsync(int departmentId);
        Task<Employee> CreateEmployeeAsync(CreateEmployeeDto dto);
        Task<Employee?> UpdateEmployeeAsync(int id, UpdateEmployeeDto dto);
        Task<bool> DeleteEmployeeAsync(int id);
    }
}
