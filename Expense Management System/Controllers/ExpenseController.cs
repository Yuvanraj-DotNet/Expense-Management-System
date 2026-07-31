using Expense_Management_System.DTOs.Expense;
using Expense_Management_System.Services.Expense;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Expense_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    [Authorize]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;


        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }


        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult CreateExpense([FromForm] CreateExpenseDto createExpenseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = _expenseService.CreateExpense(createExpenseDto, userId);

            if (result == "Expense Created Successfully")
            {
                return Ok(result);
            }

            return BadRequest(result);
        }



        [Authorize(Roles = "1")]
        [HttpPut("{id}")]

        public IActionResult UpdateExpense(
                       int id,
              [FromForm] UpdateExpenseDto updateExpenseDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = _expenseService.UpdateExpense(id, userId, updateExpenseDto);

            if (result == "Expense Updated Successfully")
            {
                return Ok(result);
            }

            return BadRequest(result);
        }


        [HttpGet("my")]
        [Authorize(Roles = "1")]
        public IActionResult GetMyExpenses()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var expenses = _expenseService.GetMyExpenses(userId);

            if (expenses.Count == 0)
            {
                return NotFound("No expenses found for this user");
            }

            return Ok(expenses);
        }



        [Authorize(Roles = "2,3,4")]
        [HttpGet]

        public IActionResult GetAllExpenses
          (
             string? search,
             int pageNumber = 1,
             int pageSize = 10
          )
        {
            var expenses = _expenseService.GetAllExpenses(
                search,
                pageNumber,
                pageSize,
                out int totalRecords
            );

            if (expenses.Count == 0)
            {
                return NotFound("No Expenses Found");
            }

            return Ok(new
            {
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = expenses
            });
        }



        [Authorize(Roles = "1")]
        [HttpPost("{id}/submit")]
        public IActionResult SubmitExpense(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = _expenseService.SubmitExpense(id, userId);

            return Ok(result);
        }


        [Authorize(Roles = "2")]
        [HttpGet("pending-approval")]
        public IActionResult GetPendingApprovals()
        {
            var managerId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var expenses = _expenseService.GetPendingApprovals(managerId);

            if (expenses.Count == 0)
            {
                return NotFound("No Pending Expenses Found");
            }

            return Ok(expenses);
        }



        [Authorize(Roles = "2")]
        [HttpPost("{id}/approve")]
        public IActionResult ApproveExpense(int id, ApproveExpenseDto approveExpenseDto)
        {
            int managerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = _expenseService.ApproveExpense(id, managerId, approveExpenseDto);

            return Ok(result);
        }


        [Authorize(Roles = "2")]
        [HttpPost("{id}/reject")]
        public IActionResult RejectExpense(
                                    int id,
                          RejectExpenseDto rejectExpenseDto)
        {
            var managerId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = _expenseService.RejectExpense(
                id,
                managerId,
                rejectExpenseDto);

            return Ok(result);
        }


        [Authorize(Roles = "3,4")]
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


        [Authorize(Roles = "3,4")]
        [HttpGet("reimbursements")]

        public IActionResult GetAllReimbursements
          (
              string? search,
              int pageNumber = 1,
              int pageSize = 10
          )

        {
            var reimbursements = _expenseService.GetAllReimbursements
            (
                search,
                pageNumber,
                pageSize,
                out int totalRecords
            );

            if (reimbursements.Count == 0)
            {
                return NotFound("No Reimbursements Found");
            }

            return Ok(new
            {
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = reimbursements
            });
        }




        [Authorize(Roles = "3")]
        [HttpPost("{id}/reimburse")]
        public IActionResult ReimburseExpense(int id, ReimburseExpenseDto reimburseExpenseDto)
        {
            var financeUserId =
                int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = _expenseService.ReimburseExpense(
                id,
                reimburseExpenseDto,
                financeUserId);

            return Ok(result);
        }


        [Authorize(Roles = "4")]
        [HttpGet("monthly")]

        public IActionResult GetMonthlyReport(int month, int year)
        {
            var report = _expenseService.GetMonthlyReport(month, year);

            if (report.Count == 0)
            {
                return NotFound("No Monthly Report Found");
            }

            return Ok(report);
        }



        [Authorize(Roles = "4")]
        [HttpGet("export")]

        public IActionResult ExportMonthlyReport(int month, int year)
        {
            var file = _expenseService.ExportMonthlyReport(month, year);

            if (file.Length == 0)
            {
                return NotFound("No report found");
            }

            var fileName = $"MonthlyReport_{month}_{year}.csv";

            return File(file, "text/csv", fileName);
        }


    }

}
