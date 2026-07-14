using Expense_Management_System.Data;
using Expense_Management_System.DTOs.ExpenseCategory;

namespace Expense_Management_System.Services.ExpenseCategory
{
    public class ExpenseCategoryService : IExpenseCategoryService
    {
        private readonly ApplicationDbContext _context;


        public ExpenseCategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public string CreateCategory(CreateExpenseCategoryDto createExpenseCategoryDto)
        {
            if (createExpenseCategoryDto.MaxAllowedAmount <= 0)
            {
                return "Maximum Allowed Amount must be greater than 0";
            }

            var categoryName = createExpenseCategoryDto.Name.Trim();

            var existingCategory = _context.ExpenseCategories
                .FirstOrDefault(c =>
                    c.Name.ToLower() == categoryName.ToLower());

            if (existingCategory != null)
            {
                return "Expense Category Already Exists";
            }

            var category = new Models.ExpenseCategory
            {
                Name = categoryName,
                MaxAllowedAmount = createExpenseCategoryDto.MaxAllowedAmount
            };

            _context.ExpenseCategories.Add(category);
            _context.SaveChanges();

            return "Expense Category Created Successfully";
        }

        public List<Models.ExpenseCategory> GetAllCategories()
        {
            return _context.ExpenseCategories.ToList();
        }

        public Models.ExpenseCategory? GetCategoryById(int id)
        {
            return _context.ExpenseCategories
                .FirstOrDefault(c => c.Id == id);
        }

        public string UpdateCategory(int id, UpdateExpenseCategoryDto updateExpenseCategoryDto)
        {
            var category = _context.ExpenseCategories
                .FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return "Expense Category Not Found";
            }

            var existingCategory = _context.ExpenseCategories
                .FirstOrDefault(c =>
                    c.Name.ToLower() == updateExpenseCategoryDto.Name.Trim().ToLower()
                    && c.Id != id);

            if (existingCategory != null)
            {
                return "Expense Category Already Exists";
            }

            if (updateExpenseCategoryDto.MaxAllowedAmount <= 0)
            {
                return "Maximum Allowed Amount must be greater than 0";
            }

            category.Name = updateExpenseCategoryDto.Name.Trim();
            category.MaxAllowedAmount = updateExpenseCategoryDto.MaxAllowedAmount;

            _context.SaveChanges();

            return "Expense Category Updated Successfully";
        }


        public string DeleteCategory(int id)
        {
            var category = _context.ExpenseCategories
                .FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return "Expense Category Not Found";
            }

            _context.ExpenseCategories.Remove(category);
            _context.SaveChanges();

            return "Expense Category Deleted Successfully";
        }

    }


}
