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



    }


}
