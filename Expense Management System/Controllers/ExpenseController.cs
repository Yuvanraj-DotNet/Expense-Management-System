using Expense_Management_System.DTOs.Expense;
using Expense_Management_System.Services.Expense;
using Microsoft.AspNetCore.Mvc;

namespace Expense_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;


        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [HttpPost]
        public IActionResult CreateExpense(CreateExpenseDto createExpenseDto)
        {
            var result = _expenseService.CreateExpense(createExpenseDto);

            return Ok(result);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateExpense(int id, UpdateExpenseDto updateExpenseDto)
        {
            var result = _expenseService.UpdateExpense(id, updateExpenseDto);

             return Ok(result);

        }


        [HttpGet("my")]
        public IActionResult GetMyExpenses(int userId)
        {
            var expenses = _expenseService.GetMyExpenses(userId);


            if (expenses.Count == 0)
            {
                return NotFound("No expenses found for this user");
            }

            return Ok(expenses);


        }





    }

}
