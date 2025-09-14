using DepartmentEmployeeSystem.API.Data;
using DepartmentEmployeeSystem.API.Interfaces;
using DepartmentEmployeeSystem.API.Models;
using Microsoft.Data.SqlClient;

namespace DepartmentEmployeeSystem.API.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly DatabaseConnection _dbConnection;

        public DepartmentRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<List<Department>> GetAllAsync()
        {
            var departments = new List<Department>();

            using var connection = _dbConnection.CreateConnection();
            await connection.OpenAsync();

            var sql = @"
                SELECT d.DepartmentId, d.DepartmentCode, d.DepartmentName, d.Description,
                       d.IsActive, d.CreatedDate, d.ModifiedDate,
                       COUNT(e.EmployeeId) as EmployeeCount
                FROM Departments d
                LEFT JOIN Employees e ON d.DepartmentId = e.DepartmentId AND e.IsActive = 1
                GROUP BY d.DepartmentId, d.DepartmentCode, d.DepartmentName, d.Description,
                         d.IsActive, d.CreatedDate, d.ModifiedDate
                ORDER BY d.DepartmentName";

            using var command = new SqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var department = new Department
                {
                    DepartmentId = reader.GetInt32(0),
                    DepartmentCode = reader.GetString(1),
                    DepartmentName = reader.GetString(2),
                    Description = reader.IsDBNull(3) ? null : reader.GetString(3),
                    IsActive = reader.GetBoolean(4),
                    CreatedDate = reader.GetDateTime(5),
                    ModifiedDate = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
                    EmployeeCount = reader.GetInt32(7)
                };
                departments.Add(department);
            }

            return departments;
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            using var connection = _dbConnection.CreateConnection();
            await connection.OpenAsync();

            var sql = @"
                SELECT d.DepartmentId, d.DepartmentCode, d.DepartmentName, d.Description,
                       d.IsActive, d.CreatedDate, d.ModifiedDate,
                       COUNT(e.EmployeeId) as EmployeeCount
                FROM Departments d
                LEFT JOIN Employees e ON d.DepartmentId = e.DepartmentId AND e.IsActive = 1
                WHERE d.DepartmentId = @Id
                GROUP BY d.DepartmentId, d.DepartmentCode, d.DepartmentName, d.Description,
                         d.IsActive, d.CreatedDate, d.ModifiedDate";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Department
                {
                    DepartmentId = reader.GetInt32(0),
                    DepartmentCode = reader.GetString(1),
                    DepartmentName = reader.GetString(2),
                    Description = reader.IsDBNull(3) ? null : reader.GetString(3),
                    IsActive = reader.GetBoolean(4),
                    CreatedDate = reader.GetDateTime(5),
                    ModifiedDate = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
                    EmployeeCount = reader.GetInt32(7)
                };
            }

            return null;
        }

        public async Task<Department> CreateAsync(Department department)
        {
            using var connection = _dbConnection.CreateConnection();
            await connection.OpenAsync();

            var sql = @"
                INSERT INTO Departments (DepartmentCode, DepartmentName, Description, IsActive, CreatedDate)
                VALUES (@Code, @Name, @Description, @IsActive, @CreatedDate);
                
                SELECT SCOPE_IDENTITY();";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Code", department.DepartmentCode);
            command.Parameters.AddWithValue("@Name", department.DepartmentName);
            command.Parameters.AddWithValue("@Description", department.Description ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@IsActive", department.IsActive);
            command.Parameters.AddWithValue("@CreatedDate", DateTime.UtcNow);

            var newId = Convert.ToInt32(await command.ExecuteScalarAsync()!);
            
            return await GetByIdAsync(newId) ?? throw new InvalidOperationException("Failed to create department");
        }

        public async Task<Department> UpdateAsync(Department department)
        {
            using var connection = _dbConnection.CreateConnection();
            await connection.OpenAsync();

            var sql = @"
                UPDATE Departments
                SET DepartmentCode = @Code, DepartmentName = @Name, Description = @Description,
                    IsActive = @IsActive, ModifiedDate = @ModifiedDate
                WHERE DepartmentId = @Id";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", department.DepartmentId);
            command.Parameters.AddWithValue("@Code", department.DepartmentCode);
            command.Parameters.AddWithValue("@Name", department.DepartmentName);
            command.Parameters.AddWithValue("@Description", department.Description ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@IsActive", department.IsActive);
            command.Parameters.AddWithValue("@ModifiedDate", DateTime.UtcNow);

            await command.ExecuteNonQueryAsync();

            return await GetByIdAsync(department.DepartmentId) ?? throw new InvalidOperationException("Failed to update department");
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _dbConnection.CreateConnection();
            await connection.OpenAsync();

            // Check if department has employees
            var checkSql = "SELECT COUNT(1) FROM Employees WHERE DepartmentId = @Id AND IsActive = 1";
            using var checkCommand = new SqlCommand(checkSql, connection);
            checkCommand.Parameters.AddWithValue("@Id", id);
            var employeeCount = (int)(await checkCommand.ExecuteScalarAsync())!;

            if (employeeCount > 0)
            {
                throw new InvalidOperationException("Cannot delete department with active employees");
            }

            var sql = "DELETE FROM Departments WHERE DepartmentId = @Id";
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);

            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            using var connection = _dbConnection.CreateConnection();
            await connection.OpenAsync();

            var sql = "SELECT COUNT(1) FROM Departments WHERE DepartmentId = @Id";
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);

            var count = (int)(await command.ExecuteScalarAsync())!;
            return count > 0;
        }

        public async Task<bool> CodeExistsAsync(string code, int? excludeId = null)
        {
            using var connection = _dbConnection.CreateConnection();
            await connection.OpenAsync();

            var sql = "SELECT COUNT(1) FROM Departments WHERE DepartmentCode = @Code";
            
            if (excludeId.HasValue)
            {
                sql += " AND DepartmentId != @ExcludeId";
            }

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Code", code);
            
            if (excludeId.HasValue)
            {
                command.Parameters.AddWithValue("@ExcludeId", excludeId.Value);
            }

            var count = (int)(await command.ExecuteScalarAsync())!;
            return count > 0;
        }
    }
}
