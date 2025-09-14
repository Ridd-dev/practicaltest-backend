using AutoMapper;
using DepartmentEmployeeSystem.API.DTOs;
using DepartmentEmployeeSystem.API.Interfaces;
using DepartmentEmployeeSystem.API.Models;

namespace DepartmentEmployeeSystem.API.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public DepartmentService(IDepartmentRepository departmentRepository, IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync()
        {
            var departments = await _departmentRepository.GetDepartmentsWithEmployeeCountAsync();
            return departments.Select(d => new DepartmentDto
            {
                DepartmentId = d.DepartmentId,
                DepartmentCode = d.DepartmentCode,
                DepartmentName = d.DepartmentName,
                Description = d.Description,
                IsActive = d.IsActive,
                CreatedDate = d.CreatedDate,
                ModifiedDate = d.ModifiedDate,
                EmployeeCount = d.Employees.Count(e => e.IsActive)
            });
        }

        public async Task<DepartmentDto?> GetDepartmentByIdAsync(int id)
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            return department == null ? null : _mapper.Map<DepartmentDto>(department);
        }

        public async Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto createDepartmentDto)
        {
            if (!await _departmentRepository.IsDepartmentCodeUniqueAsync(createDepartmentDto.DepartmentCode))
            {
                throw new ArgumentException("Department code already exists.");
            }

            var department = _mapper.Map<Department>(createDepartmentDto);
            var createdDepartment = await _departmentRepository.AddAsync(department);
            return _mapper.Map<DepartmentDto>(createdDepartment);
        }

        public async Task<DepartmentDto?> UpdateDepartmentAsync(int id, UpdateDepartmentDto updateDepartmentDto)
        {
            var existingDepartment = await _departmentRepository.GetByIdAsync(id);
            if (existingDepartment == null) return null;

            if (!await _departmentRepository.IsDepartmentCodeUniqueAsync(updateDepartmentDto.DepartmentCode, id))
            {
                throw new ArgumentException("Department code already exists.");
            }

            _mapper.Map(updateDepartmentDto, existingDepartment);
            existingDepartment.ModifiedDate = DateTime.UtcNow;
            
            var updatedDepartment = await _departmentRepository.UpdateAsync(existingDepartment);
            return _mapper.Map<DepartmentDto>(updatedDepartment);
        }

        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            // Use the specific repository method that already includes employees
            var departmentWithEmployees = (await _departmentRepository.GetDepartmentsWithEmployeeCountAsync())
                .FirstOrDefault(d => d.DepartmentId == id);
            
            if (departmentWithEmployees?.Employees.Any(e => e.IsActive) == true)
            {
                throw new InvalidOperationException("Cannot delete department with active employees.");
            }

            return await _departmentRepository.DeleteAsync(id);
        }
    }
}
