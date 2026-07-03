namespace Expense_Management_System.DTOs.Expense
{
    public class ReimburseExpenseDto
    {
        public int ProcessedBy { get; set; }

        public string ReferenceNumber { get; set; } = string.Empty;
    }
}