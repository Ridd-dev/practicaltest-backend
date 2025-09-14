using DepartmentEmployeeSystem.API.DTOs;

namespace DepartmentEmployeeSystem.API.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
        Task<EmployeeDto?> GetEmployeeByIdAsync(int id);
        Task<IEnumerable<EmployeeDto>> GetEmployeesByDepartmentAsync(int departmentId);
        Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto createEmployeeDto);
        Task<EmployeeDto?> UpdateEmployeeAsync(int id, UpdateEmployeeDto updateEmployeeDto);
        Task<bool> DeleteEmployeeAsync(int id);
    }
}
