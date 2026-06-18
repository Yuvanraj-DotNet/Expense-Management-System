using System;

namespace Expense_Management_System.Models
{
    public class Expense
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int CategoryId { get; set; }

        public string Title { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public string Currency { get; set; } = "INR";

        public DateTime ExpenseDate { get; set; }

        public string Description { get; set; } = string.Empty;

        public string ReceiptPath { get; set; } = string.Empty;

        public string Status { get; set; } = "Draft";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}