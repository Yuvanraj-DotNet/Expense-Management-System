namespace Expense_Management_System.DTOs.ExpenseCategory
{
    public class ExpenseCategoryResponseDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal MaxAllowedAmount { get; set; }
    }
}