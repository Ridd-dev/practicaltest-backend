using AutoMapper;
using DepartmentEmployeeSystem.API.DTOs;
using DepartmentEmployeeSystem.API.Models;

namespace DepartmentEmployeeSystem.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Department mappings
            CreateMap<Department, DepartmentDto>()
                .ForMember(dest => dest.EmployeeCount, opt => opt.MapFrom(src => src.Employees.Count(e => e.IsActive)));

            CreateMap<CreateDepartmentDto, Department>()
                .ForMember(dest => dest.DepartmentId, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Employees, opt => opt.Ignore());

            CreateMap<UpdateDepartmentDto, Department>()
                .ForMember(dest => dest.DepartmentId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Employees, opt => opt.Ignore());

            // Employee mappings
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.DepartmentName))
                .ForMember(dest => dest.DepartmentCode, opt => opt.MapFrom(src => src.Department.DepartmentCode));

            CreateMap<CreateEmployeeDto, Employee>()
                .ForMember(dest => dest.EmployeeId, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Department, opt => opt.Ignore());

            CreateMap<UpdateEmployeeDto, Employee>()
                .ForMember(dest => dest.EmployeeId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Department, opt => opt.Ignore());
        }
    }
}
