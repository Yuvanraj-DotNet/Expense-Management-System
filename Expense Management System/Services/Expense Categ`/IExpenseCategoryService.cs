using Expense_Management_System.DTOs.ExpenseCategory;

namespace Expense_Management_System.Services.ExpenseCategory
{
    public interface IExpenseCategoryService
    {
        string CreateCategory(CreateExpenseCategoryDto createExpenseCategoryDto);
    }
}
