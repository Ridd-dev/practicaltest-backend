namespace DepartmentEmployeeSystem.API.DTOs
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public string EmailAddress { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public decimal Salary { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string DepartmentCode { get; set; } = string.Empty;
    }

    public class CreateEmployeeDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public decimal Salary { get; set; }
        public string? PhoneNumber { get; set; }
        public int DepartmentId { get; set; }
    }

    public class UpdateEmployeeDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public decimal Salary { get; set; }
        public string? PhoneNumber { get; set; }
        public int DepartmentId { get; set; }
        public bool IsActive { get; set; }
    }
}
