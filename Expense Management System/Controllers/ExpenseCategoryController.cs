using Expense_Management_System.DTOs.ExpenseCategory;
using Expense_Management_System.Services.ExpenseCategory;
using Microsoft.AspNetCore.Mvc;

namespace Expense_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            var result = _expenseCategoryService.CreateCategory(createExpenseCategoryDto);

            return Ok(result);
        }
    }


}
