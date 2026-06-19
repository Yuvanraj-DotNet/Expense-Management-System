using System;

namespace Expense_Management_System.Models
{
    public class Reimbursement
    {
        public int Id { get; set; }

        public int ExpenseId { get; set; }

        public int ProcessedBy { get; set; }

        public DateTime PaymentDate { get; set; }

        public string ReferenceNumber { get; set; } = string.Empty;

        public decimal Amount { get; set; }
    }
}