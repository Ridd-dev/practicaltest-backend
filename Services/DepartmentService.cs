using AutoMapper;
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

        public async Task<List<Department>> GetAllDepartmentsAsync()
        {
            return await _departmentRepository.GetAllAsync();
        }

        public async Task<Department?> GetDepartmentByIdAsync(int id)
        {
            return await _departmentRepository.GetByIdAsync(id);
        }

        public async Task<Department> CreateDepartmentAsync(CreateDepartmentDto dto)
        {
            if (await _departmentRepository.CodeExistsAsync(dto.DepartmentCode))
            {
                throw new ArgumentException("Department code already exists.");
            }

            var department = _mapper.Map<Department>(dto);
            return await _departmentRepository.CreateAsync(department);
        }

        public async Task<Department?> UpdateDepartmentAsync(int id, UpdateDepartmentDto dto)
        {
            var existingDepartment = await _departmentRepository.GetByIdAsync(id);
            if (existingDepartment == null) return null;

            if (await _departmentRepository.CodeExistsAsync(dto.DepartmentCode, id))
            {
                throw new ArgumentException("Department code already exists.");
            }

            _mapper.Map(dto, existingDepartment);
            existingDepartment.DepartmentId = id;
            
            return await _departmentRepository.UpdateAsync(existingDepartment);
        }

        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null) return false;

            if (department.EmployeeCount > 0)
            {
                throw new InvalidOperationException("Cannot delete department with active employees.");
            }

            return await _departmentRepository.DeleteAsync(id);
        }
    }
}
