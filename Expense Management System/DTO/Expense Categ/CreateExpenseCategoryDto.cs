namespace Expense_Management_System.DTOs.ExpenseCategory
{
    public class CreateExpenseCategoryDto
    {
        public string Name { get; set; } = string.Empty;

       public decimal MaxAllowedAmount { get; set; }
    }

}
