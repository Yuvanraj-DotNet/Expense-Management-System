using Expense_Management_System.DTOs.ExpenseCategory;
using Expense_Management_System.Services.ExpenseCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Expense_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    [Authorize]
    public class ExpenseCategoryController : ControllerBase
    {
        private readonly IExpenseCategoryService _expenseCategoryService;


    public ExpenseCategoryController(IExpenseCategoryService expenseCategoryService)
        {
            _expenseCategoryService = expenseCategoryService;
        }

        [HttpPost]
        public IActionResult CreateCategory(CreateExpenseCategoryDto createExpenseCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _expenseCategoryService.CreateCategory(createExpenseCategoryDto);

            if (result == "Expense Category Created Successfully")
            {
                return Ok(result);
            }

            return BadRequest(result);
        }



        [HttpGet]
        public IActionResult GetAllCategories(
            string? search,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var result = _expenseCategoryService.GetAllCategories(
                search,
                pageNumber,
                pageSize,
                out int totalRecords);

            if (result.Count == 0)
            {
                return NotFound("No Expense Categories Found");
            }

            return Ok(new
            {
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = result
            });
        }


        [HttpGet("{id}")]
        public IActionResult GetCategoryById(int id)
        {
            var category = _expenseCategoryService.GetCategoryById(id);

            if (category == null)
            {
                return NotFound("Expense Category Not Found");
            }

            return Ok(category);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id, UpdateExpenseCategoryDto updateExpenseCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _expenseCategoryService.UpdateCategory(id, updateExpenseCategoryDto);

            if (result == "Expense Category Updated Successfully")
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var result = _expenseCategoryService.DeleteCategory(id);

            if (result == "Expense Category Deleted Successfully")
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }


}
