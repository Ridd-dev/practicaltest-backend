using Microsoft.AspNetCore.Mvc;
using DepartmentEmployeeSystem.API.DTOs;
using DepartmentEmployeeSystem.API.Services;

namespace DepartmentEmployeeSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetAllDepartments()
        {
            try
            {
                var departments = await _departmentService.GetAllDepartmentsAsync();
                return Ok(departments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving departments.", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentDto>> GetDepartment(int id)
        {
            try
            {
                var department = await _departmentService.GetDepartmentByIdAsync(id);
                return department == null ? NotFound(new { message = "Department not found." }) : Ok(department);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the department.", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<DepartmentDto>> CreateDepartment(CreateDepartmentDto createDepartmentDto)
        {
            try
            {
                var department = await _departmentService.CreateDepartmentAsync(createDepartmentDto);
                return CreatedAtAction(nameof(GetDepartment), new { id = department.DepartmentId }, department);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the department.", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DepartmentDto>> UpdateDepartment(int id, UpdateDepartmentDto updateDepartmentDto)
        {
            try
            {
                var department = await _departmentService.UpdateDepartmentAsync(id, updateDepartmentDto);
                return department == null ? NotFound(new { message = "Department not found." }) : Ok(department);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the department.", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                var result = await _departmentService.DeleteDepartmentAsync(id);
                return result ? NoContent() : NotFound(new { message = "Department not found." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the department.", error = ex.Message });
            }
        }
    }
}
