namespace Expense_Management_System.DTOs.Expense
{
    public class ExpenseListResponseDto
    {
        public int Id { get; set; }

        public string EmployeeName { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public DateTime ExpenseDate { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}