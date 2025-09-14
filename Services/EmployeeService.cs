using AutoMapper;
using DepartmentEmployeeSystem.API.Interfaces;
using DepartmentEmployeeSystem.API.Models;

namespace DepartmentEmployeeSystem.API.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public EmployeeService(
            IEmployeeRepository employeeRepository,
            IDepartmentRepository departmentRepository,
            IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _employeeRepository.GetAllAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            return await _employeeRepository.GetByIdAsync(id);
        }

        public async Task<List<Employee>> GetEmployeesByDepartmentAsync(int departmentId)
        {
            return await _employeeRepository.GetByDepartmentAsync(departmentId);
        }

        public async Task<Employee> CreateEmployeeAsync(CreateEmployeeDto dto)
        {
            if (await _employeeRepository.EmailExistsAsync(dto.EmailAddress))
            {
                throw new ArgumentException("Email address already exists.");
            }

            if (!await _departmentRepository.ExistsAsync(dto.DepartmentId))
            {
                throw new ArgumentException("Department does not exist.");
            }

            var employee = _mapper.Map<Employee>(dto);
            return await _employeeRepository.CreateAsync(employee);
        }

        public async Task<Employee?> UpdateEmployeeAsync(int id, UpdateEmployeeDto dto)
        {
            var existingEmployee = await _employeeRepository.GetByIdAsync(id);
            if (existingEmployee == null) return null;

            if (await _employeeRepository.EmailExistsAsync(dto.EmailAddress, id))
            {
                throw new ArgumentException("Email address already exists.");
            }

            if (!await _departmentRepository.ExistsAsync(dto.DepartmentId))
            {
                throw new ArgumentException("Department does not exist.");
            }

            _mapper.Map(dto, existingEmployee);
            existingEmployee.EmployeeId = id;

            return await _employeeRepository.UpdateAsync(existingEmployee);
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            return await _employeeRepository.DeleteAsync(id);
        }
    }
}
