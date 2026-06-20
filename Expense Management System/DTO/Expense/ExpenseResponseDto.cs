using System;

namespace Expense_Management_System.DTOs.Expense
{
    public class ExpenseResponseDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime ExpenseDate { get; set; }
    }
}