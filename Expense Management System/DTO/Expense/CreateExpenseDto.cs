using System;

namespace Expense_Management_System.DTOs.Expense
{
    public class CreateExpenseDto
    {
        public int CategoryId { get; set; }

        public string Title { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public string Currency { get; set; } = "INR";

        public DateTime ExpenseDate { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}