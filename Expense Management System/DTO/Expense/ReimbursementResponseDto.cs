namespace Expense_Management_System.DTOs.Expense
{
    public class ReimbursementResponseDto
    {
        public int ReimbursementId { get; set; }

        public int ExpenseId { get; set; }

        public string EmployeeName { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        public string ReferenceNumber { get; set; } = string.Empty;

        public string ProcessedBy { get; set; } = string.Empty;
    }
}