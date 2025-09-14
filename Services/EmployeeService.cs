using AutoMapper;
using DepartmentEmployeeSystem.API.DTOs;
using DepartmentEmployeeSystem.API.Interfaces;
using DepartmentEmployeeSystem.API.Models;

namespace DepartmentEmployeeSystem.API.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }

        public async Task<EmployeeDto?> GetEmployeeByIdAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            return employee == null ? null : _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployeesByDepartmentAsync(int departmentId)
        {
            var employees = await _employeeRepository.GetEmployeesByDepartmentAsync(departmentId);
            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }

        public async Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto createEmployeeDto)
        {
            if (!await _employeeRepository.IsEmailUniqueAsync(createEmployeeDto.EmailAddress))
            {
                throw new ArgumentException("Email address already exists.");
            }

            if (!await _departmentRepository.ExistsAsync(d => d.DepartmentId == createEmployeeDto.DepartmentId && d.IsActive))
            {
                throw new ArgumentException("Department does not exist.");
            }

            var employee = _mapper.Map<Employee>(createEmployeeDto);
            var createdEmployee = await _employeeRepository.AddAsync(employee);
            return _mapper.Map<EmployeeDto>(createdEmployee);
        }

        public async Task<EmployeeDto?> UpdateEmployeeAsync(int id, UpdateEmployeeDto updateEmployeeDto)
        {
            var existingEmployee = await _employeeRepository.GetByIdAsync(id);
            if (existingEmployee == null) return null;

            if (!await _employeeRepository.IsEmailUniqueAsync(updateEmployeeDto.EmailAddress, id))
            {
                throw new ArgumentException("Email address already exists.");
            }

            if (!await _departmentRepository.ExistsAsync(d => d.DepartmentId == updateEmployeeDto.DepartmentId && d.IsActive))
            {
                throw new ArgumentException("Department does not exist.");
            }

            _mapper.Map(updateEmployeeDto, existingEmployee);
            existingEmployee.ModifiedDate = DateTime.UtcNow;
            
            var updatedEmployee = await _employeeRepository.UpdateAsync(existingEmployee);
            return _mapper.Map<EmployeeDto>(updatedEmployee);
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            return await _employeeRepository.DeleteAsync(id);
        }
    }
}
