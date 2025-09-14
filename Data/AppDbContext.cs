using Microsoft.EntityFrameworkCore;
using DepartmentEmployeeSystem.API.Models;

namespace DepartmentEmployeeSystem.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Department configuration
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(d => d.DepartmentId);
                entity.HasIndex(d => d.DepartmentCode).IsUnique();
                entity.Property(d => d.DepartmentCode).IsRequired().HasMaxLength(10);
                entity.Property(d => d.DepartmentName).IsRequired().HasMaxLength(100);
                entity.Property(d => d.Description).HasMaxLength(500);
            });

            // Employee configuration
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeId);
                entity.HasIndex(e => e.EmailAddress).IsUnique();
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.EmailAddress).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Salary).HasColumnType("decimal(18,2)");
                entity.Property(e => e.PhoneNumber).HasMaxLength(15);

                entity.HasOne(e => e.Department)
                      .WithMany(d => d.Employees)
                      .HasForeignKey(e => e.DepartmentId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // Seed departments
            modelBuilder.Entity<Department>().HasData(
                new Department 
                { 
                    DepartmentId = 1, 
                    DepartmentCode = "IT", 
                    DepartmentName = "Information Technology", 
                    Description = "Technology and software development",
                    IsActive = true,
                    CreatedDate = seedDate
                },
                new Department 
                { 
                    DepartmentId = 2, 
                    DepartmentCode = "HR", 
                    DepartmentName = "Human Resources", 
                    Description = "Personnel and employee relations",
                    IsActive = true,
                    CreatedDate = seedDate
                },
                new Department 
                { 
                    DepartmentId = 3, 
                    DepartmentCode = "FIN", 
                    DepartmentName = "Finance", 
                    Description = "Financial operations and accounting",
                    IsActive = true,
                    CreatedDate = seedDate
                }
            );

            // Seed employees
            modelBuilder.Entity<Employee>().HasData(
                new Employee 
                { 
                    EmployeeId = 1, 
                    FirstName = "John", 
                    LastName = "Doe", 
                    EmailAddress = "john.doe@company.com", 
                    DateOfBirth = new DateTime(1990, 1, 15), 
                    Salary = 75000, 
                    PhoneNumber = "1234567890", 
                    DepartmentId = 1,
                    IsActive = true,
                    CreatedDate = seedDate
                },
                new Employee 
                { 
                    EmployeeId = 2, 
                    FirstName = "Jane", 
                    LastName = "Smith", 
                    EmailAddress = "jane.smith@company.com", 
                    DateOfBirth = new DateTime(1985, 5, 22), 
                    Salary = 65000, 
                    PhoneNumber = "0987654321", 
                    DepartmentId = 2,
                    IsActive = true,
                    CreatedDate = seedDate
                }
            );
        }
    }
}
