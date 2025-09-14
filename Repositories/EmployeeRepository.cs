using DepartmentEmployeeSystem.API.Data;
using DepartmentEmployeeSystem.API.Interfaces;
using DepartmentEmployeeSystem.API.Models;
using Microsoft.Data.SqlClient;

namespace DepartmentEmployeeSystem.API.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DatabaseConnection _dbConnection;

        public EmployeeRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            var employees = new List<Employee>();

            using var connection = _dbConnection.CreateConnection();
            await connection.OpenAsync();

            var sql = @"
                SELECT e.EmployeeId, e.FirstName, e.LastName, e.EmailAddress, e.DateOfBirth,
                       e.Salary, e.PhoneNumber, e.IsActive, e.CreatedDate, e.ModifiedDate, e.DepartmentId,
                       d.DepartmentName, d.DepartmentCode
                FROM Employees e
                INNER JOIN Departments d ON e.DepartmentId = d.DepartmentId
                ORDER BY e.FirstName, e.LastName";

            using var command = new SqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var employee = new Employee
                {
                    EmployeeId = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    EmailAddress = reader.GetString(3),
                    DateOfBirth = reader.GetDateTime(4),
                    Salary = reader.GetDecimal(5),
                    PhoneNumber = reader.IsDBNull(6) ? null : reader.GetString(6),
                    IsActive = reader.GetBoolean(7),
                    CreatedDate = reader.GetDateTime(8),
                    ModifiedDate = reader.IsDBNull(9) ? null : reader.GetDateTime(9),
                    DepartmentId = reader.GetInt32(10),
                    DepartmentName = reader.GetString(11),
                    DepartmentCode = reader.GetString(12)
                };
                employees.Add(employee);
            }

            return employees;
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            using var connection = _dbConnection.CreateConnection();
            await connection.OpenAsync();

            var sql = @"
                SELECT e.EmployeeId, e.FirstName, e.LastName, e.EmailAddress, e.DateOfBirth,
                       e.Salary, e.PhoneNumber, e.IsActive, e.CreatedDate, e.ModifiedDate, e.DepartmentId,
                       d.DepartmentName, d.DepartmentCode
                FROM Employees e
                INNER JOIN Departments d ON e.DepartmentId = d.DepartmentId
                WHERE e.EmployeeId = @Id";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Employee
                {
                    EmployeeId = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    EmailAddress = reader.GetString(3),
                    DateOfBirth = reader.GetDateTime(4),
                    Salary = reader.GetDecimal(5),
                    PhoneNumber = reader.IsDBNull(6) ? null : reader.GetString(6),
                    IsActive = reader.GetBoolean(7),
                    CreatedDate = reader.GetDateTime(8),
                    ModifiedDate = reader.IsDBNull(9) ? null : reader.GetDateTime(9),
                    DepartmentId = reader.GetInt32(10),
                    DepartmentName = reader.GetString(11),
                    DepartmentCode = reader.GetString(12)
                };
            }

            return null;
        }

        public async Task<List<Employee>> GetByDepartmentAsync(int departmentId)
        {
            var employees = new List<Employee>();

            using var connection = _dbConnection.CreateConnection();
            await connection.OpenAsync();

            var sql = @"
                SELECT e.EmployeeId, e.FirstName, e.LastName, e.EmailAddress, e.DateOfBirth,
                       e.Salary, e.PhoneNumber, e.IsActive, e.CreatedDate, e.ModifiedDate, e.DepartmentId,
                       d.DepartmentName, d.DepartmentCode
                FROM Employees e
                INNER JOIN Departments d ON e.DepartmentId = d.DepartmentId
                WHERE e.DepartmentId = @DepartmentId
                ORDER BY e.FirstName, e.LastName";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@DepartmentId", departmentId);

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var employee = new Employee
                {
                    EmployeeId = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    EmailAddress = reader.GetString(3),
                    DateOfBirth = reader.GetDateTime(4),
                    Salary = reader.GetDecimal(5),
                    PhoneNumber = reader.IsDBNull(6) ? null : reader.GetString(6),
                    IsActive = reader.GetBoolean(7),
                    CreatedDate = reader.GetDateTime(8),
                    ModifiedDate = reader.IsDBNull(9) ? null : reader.GetDateTime(9),
                    DepartmentId = reader.GetInt32(10),
                    DepartmentName = reader.GetString(11),
                    DepartmentCode = reader.GetString(12)
                };
                employees.Add(employee);
            }

            return employees;
        }

        public async Task<Employee> CreateAsync(Employee employee)
        {
            using var connection = _dbConnection.CreateConnection();
            await connection.OpenAsync();

            var sql = @"
                INSERT INTO Employees (FirstName, LastName, EmailAddress, DateOfBirth, Salary,
                                     PhoneNumber, IsActive, CreatedDate, DepartmentId)
                VALUES (@FirstName, @LastName, @EmailAddress, @DateOfBirth, @Salary,
                        @PhoneNumber, @IsActive, @CreatedDate, @DepartmentId);
                
                SELECT SCOPE_IDENTITY();";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@FirstName", employee.FirstName);
            command.Parameters.AddWithValue("@LastName", employee.LastName);
            command.Parameters.AddWithValue("@EmailAddress", employee.EmailAddress);
            command.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth);
            command.Parameters.AddWithValue("@Salary", employee.Salary);
            command.Parameters.AddWithValue("@PhoneNumber", employee.PhoneNumber ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@IsActive", employee.IsActive);
            command.Parameters.AddWithValue("@CreatedDate", DateTime.UtcNow);
            command.Parameters.AddWithValue("@DepartmentId", employee.DepartmentId);

            var newId = Convert.ToInt32(await command.ExecuteScalarAsync()!);
            
            return await GetByIdAsync(newId) ?? throw new InvalidOperationException("Failed to create employee");
        }

        public async Task<Employee> UpdateAsync(Employee employee)
        {
            using var connection = _dbConnection.CreateConnection();
            await connection.OpenAsync();

            var sql = @"
                UPDATE Employees
                SET FirstName = @FirstName, LastName = @LastName, EmailAddress = @EmailAddress,
                    DateOfBirth = @DateOfBirth, Salary = @Salary, PhoneNumber = @PhoneNumber,
                    IsActive = @IsActive, ModifiedDate = @ModifiedDate, DepartmentId = @DepartmentId
                WHERE EmployeeId = @Id";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", employee.EmployeeId);
            command.Parameters.AddWithValue("@FirstName", employee.FirstName);
            command.Parameters.AddWithValue("@LastName", employee.LastName);
            command.Parameters.AddWithValue("@EmailAddress", employee.EmailAddress);
            command.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth);
            command.Parameters.AddWithValue("@Salary", employee.Salary);
            command.Parameters.AddWithValue("@PhoneNumber", employee.PhoneNumber ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@IsActive", employee.IsActive);
            command.Parameters.AddWithValue("@ModifiedDate", DateTime.UtcNow);
            command.Parameters.AddWithValue("@DepartmentId", employee.DepartmentId);

            await command.ExecuteNonQueryAsync();

            return await GetByIdAsync(employee.EmployeeId) ?? throw new InvalidOperationException("Failed to update employee");
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _dbConnection.CreateConnection();
            await connection.OpenAsync();

            var sql = "DELETE FROM Employees WHERE EmployeeId = @Id";
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);

            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            using var connection = _dbConnection.CreateConnection();
            await connection.OpenAsync();

            var sql = "SELECT COUNT(1) FROM Employees WHERE EmployeeId = @Id";
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);

            var count = (int)(await command.ExecuteScalarAsync())!;
            return count > 0;
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
        {
            using var connection = _dbConnection.CreateConnection();
            await connection.OpenAsync();

            var sql = "SELECT COUNT(1) FROM Employees WHERE EmailAddress = @Email";
            
            if (excludeId.HasValue)
            {
                sql += " AND EmployeeId != @ExcludeId";
            }

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", email);
            
            if (excludeId.HasValue)
            {
                command.Parameters.AddWithValue("@ExcludeId", excludeId.Value);
            }

            var count = (int)(await command.ExecuteScalarAsync())!;
            return count > 0;
        }
    }
}
