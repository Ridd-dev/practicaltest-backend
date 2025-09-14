using System.ComponentModel.DataAnnotations;

namespace DepartmentEmployeeSystem.API.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        
        [Required]
        [StringLength(10)]
        public string DepartmentCode { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string DepartmentName { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedDate { get; set; }
        
        public DateTime? ModifiedDate { get; set; }

        public int EmployeeCount { get; set; }
    }

    public class CreateDepartmentDto
    {
        [Required]
        [StringLength(10)]
        public string DepartmentCode { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string DepartmentName { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Description { get; set; }
    }

    public class UpdateDepartmentDto
    {
        [Required]
        [StringLength(10)]
        public string DepartmentCode { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string DepartmentName { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        public bool IsActive { get; set; } = true;
    }
}
