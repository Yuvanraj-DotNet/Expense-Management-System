using System;

namespace Expense_Management_System.Models
{
    public class ExpenseApproval
    {
        public int Id { get; set; }

        public int ExpenseId { get; set; }

        public int ApproverId { get; set; }

        public string Action { get; set; } = string.Empty; // Approve / Reject

        public string Comment { get; set; } = string.Empty;

        public DateTime ActionDate { get; set; } = DateTime.Now;
    }
}