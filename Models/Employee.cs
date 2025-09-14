using System.ComponentModel.DataAnnotations;

namespace DepartmentEmployeeSystem.API.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string EmailAddress { get; set; } = string.Empty;
        
        [Required]
        public DateTime DateOfBirth { get; set; }
        
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Salary { get; set; }
        
        [StringLength(15)]
        public string? PhoneNumber { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedDate { get; set; }
        
        public DateTime? ModifiedDate { get; set; }
        
        [Required]
        public int DepartmentId { get; set; }

        // Computed properties
        public string FullName => $"{FirstName} {LastName}";
        public int Age => DateTime.Now.Year - DateOfBirth.Year - (DateTime.Now.DayOfYear < DateOfBirth.DayOfYear ? 1 : 0);
        public string DepartmentName { get; set; } = string.Empty;
        public string DepartmentCode { get; set; } = string.Empty;
    }

    public class CreateEmployeeDto
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string EmailAddress { get; set; } = string.Empty;
        
        [Required]
        public DateTime DateOfBirth { get; set; }
        
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Salary { get; set; }
        
        [StringLength(15)]
        public string? PhoneNumber { get; set; }
        
        [Required]
        public int DepartmentId { get; set; }
    }

    public class UpdateEmployeeDto
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string EmailAddress { get; set; } = string.Empty;
        
        [Required]
        public DateTime DateOfBirth { get; set; }
        
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Salary { get; set; }
        
        [StringLength(15)]
        public string? PhoneNumber { get; set; }
        
        [Required]
        public int DepartmentId { get; set; }
        
        public bool IsActive { get; set; } = true;
    }
}
