using Expense_Management_System.DTOs.Expense;

namespace Expense_Management_System.Services.Expense
{
    public interface IExpenseService
    {
        string CreateExpense(CreateExpenseDto createExpenseDto);
        string UpdateExpense(int id, UpdateExpenseDto updateExpenseDto);
        List<ExpenseResponseDto> GetMyExpenses(int userId);

        string SubmitExpense(int id);

        List<ExpenseResponseDto> GetPendingApprovals();

    }
}
