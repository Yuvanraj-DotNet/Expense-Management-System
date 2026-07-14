using System.ComponentModel.DataAnnotations;

namespace Expense_Management_System.DTOs.ExpenseCategory
{
    public class UpdateExpenseCategoryDto
    {
        [Required(ErrorMessage = "Category Name is Required")]
        public string Name { get; set; } = string.Empty;

        [Range(1, double.MaxValue, ErrorMessage = "Maximum Allowed Amount must be greater than 0")]
        public decimal MaxAllowedAmount { get; set; }
    }
}