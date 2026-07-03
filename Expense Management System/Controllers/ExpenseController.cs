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

        [HttpPost("{id}/submit")]
        public IActionResult SubmitExpense(int id)
        {
            var result = _expenseService.SubmitExpense(id);

            return Ok(result);

        }

        [HttpGet("pending-approval")]
        public IActionResult GetPendingApprovals()
        {
            var expenses = _expenseService.GetPendingApprovals();

            if (expenses.Count == 0)
            {
                return NotFound("No Pending Expenses Found");
            }

            return Ok(expenses);

        }

        [HttpPost("{id}/approve")]
        public IActionResult ApproveExpense(int id, ApproveExpenseDto approveExpenseDto)
        {
            var result = _expenseService.ApproveExpense(id, approveExpenseDto);

            return Ok(result);
        }

        [HttpPost("{id}/reject")]
        public IActionResult RejectExpense(int id, RejectExpenseDto rejectExpenseDto)
        {
            var result = _expenseService.RejectExpense(id, rejectExpenseDto);

            return Ok(result);
        }

        [HttpGet("approved")]
        public IActionResult GetApprovedExpenses()
        {
            var result = _expenseService.GetApprovedExpenses();

            if (result.Count == 0)
            {
                return NotFound("No Approved Expenses Found");
            }

            return Ok(result);
        }

        [HttpPost("{id}/reimburse")]
        public IActionResult ReimburseExpense(int id, ReimburseExpenseDto reimburseExpenseDto)
        {
            var result = _expenseService.ReimburseExpense(id, reimburseExpenseDto);

            return Ok(result);
        }




    }

}
