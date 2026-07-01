using Expense_Management_System.Data;
using Expense_Management_System.DTOs.Expense;

namespace Expense_Management_System.Services.Expense
{
    public class ExpenseService : IExpenseService
    {
        private readonly ApplicationDbContext _context;

        public ExpenseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public string CreateExpense(CreateExpenseDto createExpenseDto)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Id == createExpenseDto.UserId);

            if (user == null)
            {
                return "User Not Found";
            }

            var category = _context.ExpenseCategories
                .FirstOrDefault(c => c.Id == createExpenseDto.CategoryId);

            if (category == null)
            {
                return "Expense Category Not Found";
            }

            var expense = new Models.Expense
            {
                UserId = createExpenseDto.UserId,
                CategoryId = createExpenseDto.CategoryId,
                Title = createExpenseDto.Title,
                Amount = createExpenseDto.Amount,
                ExpenseDate = createExpenseDto.ExpenseDate,
                Description = createExpenseDto.Description
            };

            _context.Expenses.Add(expense);
            _context.SaveChanges();

            return "Expense Created Successfully";
        }

        public string UpdateExpense(int id, UpdateExpenseDto updateExpenseDto)
        {
            var expense = _context.Expenses
                .FirstOrDefault(e => e.Id == id);

            if (expense == null)
            {
                return "Expense Not Found";
            }

            expense.CategoryId = updateExpenseDto.CategoryId;
            expense.Title = updateExpenseDto.Title;
            expense.Amount = updateExpenseDto.Amount;
            expense.ExpenseDate = updateExpenseDto.ExpenseDate;
            expense.Description = updateExpenseDto.Description;

            _context.SaveChanges();

            return "Expense Updated Successfully";
        }

        public List<ExpenseResponseDto> GetMyExpenses(int userId)
        {
            var expenses = _context.Expenses
                .Where(e => e.UserId == userId)
                .Select(e => new ExpenseResponseDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Amount = e.Amount,
                    Status = e.Status,
                    ExpenseDate = e.ExpenseDate
                })
                .ToList();

            return expenses;
        }

        public string SubmitExpense(int id)
        {
            var expense = _context.Expenses
                .FirstOrDefault(e => e.Id == id);

            if (expense == null)
            {
                return "Expense Not Found";
            }

            if (expense.Status != "Draft")
            {
                return "Only Draft Expenses Can Be Submitted";
            }

            expense.Status = "Submitted";

            _context.SaveChanges();

            return "Expense Submitted Successfully";
        }

        public List<ExpenseResponseDto> GetPendingApprovals()
        {
            var expenses = _context.Expenses
                .Where(e => e.Status == "Submitted")
                .Select(e => new ExpenseResponseDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Amount = e.Amount,
                    Status = e.Status,
                    ExpenseDate = e.ExpenseDate
                })
                .ToList();

            return expenses;
        }

        public string ApproveExpense(int id, ApproveExpenseDto approveExpenseDto)
        {
            var expense = _context.Expenses
                .FirstOrDefault(e => e.Id == id);

            if (expense == null)
            {
                return "Expense Not Found";
            }

            if (expense.Status != "Submitted")
            {
                return "Only Submitted Expenses Can Be Approved";
            }

            expense.Status = "Approved";

            _context.SaveChanges();

            return "Expense Approved Successfully";
        }



    }
}
