namespace Expense_Management_System.DTOs.Expense
{
    public class ApprovedExpenseDto
    {
        public int ExpenseId { get; set; }

        public int ApproverId { get; set; }

        public string EmployeeName { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public DateTime ExpenseDate { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}