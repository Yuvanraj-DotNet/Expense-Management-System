namespace Expense_Management_System.Models
{
    public class ExpenseCategory
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal MaxAllowedAmount { get; set; }
    }
}