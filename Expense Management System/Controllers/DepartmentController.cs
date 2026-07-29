using Expense_Management_System.DTOs.Department;
using Expense_Management_System.Services.Department;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Expense_Management_System.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpPost]
        public IActionResult CreateDepartment(CreateDepartmentDto createDepartmentDto)
        {
            var result = _departmentService.CreateDepartment(createDepartmentDto);

            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetAllDepartments

             (
               string? search = "",
               int pageNumber = 1,
               int pageSize = 10
             )

        {
            int totalRecords;

            var departments = _departmentService.GetAllDepartments
            (
                search,
                pageNumber,
                pageSize,
                out totalRecords
            );

            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            return Ok(new
            {
                TotalRecords = totalRecords,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                Data = departments
            });
        }

        [HttpGet("{id}")]
        public IActionResult GetDepartmentById(int id)
        {
            var result = _departmentService.GetDepartmentById(id);

            if (result == null)
            {
                return NotFound("Department Not Found");
            }

            return Ok(result);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateDepartment(int id, UpdateDepartmentDto updateDepartmentDto)
        {
            var result = _departmentService.UpdateDepartment(id, updateDepartmentDto);

            if (result == "Department Not Found")
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDepartment(int id)
        {
            var result = _departmentService.DeleteDepartment(id);

            if (result == "Department Not Found")
            {
                return NotFound(result);
            }

            return Ok(result);
        }
    }
}