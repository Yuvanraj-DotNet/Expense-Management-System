using Expense_Management_System.DTOs.ExpenseCategory;

namespace Expense_Management_System.Services.ExpenseCategory
{
    public interface IExpenseCategoryService
    {
        string CreateCategory(CreateExpenseCategoryDto createExpenseCategoryDto);
        List<ExpenseCategoryResponseDto> GetAllCategories(
              string? search,
              int pageNumber,
              int pageSize,
              out int totalRecords);

        Models.ExpenseCategory? GetCategoryById(int id);
        string UpdateCategory(int id, UpdateExpenseCategoryDto updateExpenseCategoryDto);
        string DeleteCategory(int id);
    }
}
