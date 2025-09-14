using Microsoft.AspNetCore.Mvc;
using DepartmentEmployeeSystem.API.DTOs;
using DepartmentEmployeeSystem.API.Services;

namespace DepartmentEmployeeSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAllEmployees()
        {
            try
            {
                var employees = await _employeeService.GetAllEmployeesAsync();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving employees.", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeByIdAsync(id);
                return employee == null ? NotFound(new { message = "Employee not found." }) : Ok(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the employee.", error = ex.Message });
            }
        }

        [HttpGet("department/{departmentId}")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesByDepartment(int departmentId)
        {
            try
            {
                var employees = await _employeeService.GetEmployeesByDepartmentAsync(departmentId);
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving employees.", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> CreateEmployee(CreateEmployeeDto createEmployeeDto)
        {
            try
            {
                var employee = await _employeeService.CreateEmployeeAsync(createEmployeeDto);
                return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeId }, employee);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the employee.", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeeDto>> UpdateEmployee(int id, UpdateEmployeeDto updateEmployeeDto)
        {
            try
            {
                var employee = await _employeeService.UpdateEmployeeAsync(id, updateEmployeeDto);
                return employee == null ? NotFound(new { message = "Employee not found." }) : Ok(employee);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the employee.", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var result = await _employeeService.DeleteEmployeeAsync(id);
                return result ? NoContent() : NotFound(new { message = "Employee not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the employee.", error = ex.Message });
            }
        }
    }
}
