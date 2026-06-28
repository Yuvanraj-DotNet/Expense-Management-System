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
            var existingCategory = _context.ExpenseCategories
            .FirstOrDefault(c => c.Name == createExpenseCategoryDto.Name);


            if (existingCategory != null)
            {
                return "Expense Category Already Exists";
            }

            var category = new Models.ExpenseCategory
            {
                Name = createExpenseCategoryDto.Name,
                MaxAllowedAmount = createExpenseCategoryDto.MaxAllowedAmount
            };

            _context.ExpenseCategories.Add(category);
            _context.SaveChanges();

            return "Expense Category Created Successfully";


        }


    }


}
