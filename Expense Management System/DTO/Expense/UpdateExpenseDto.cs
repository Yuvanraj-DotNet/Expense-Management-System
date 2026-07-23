using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Expense_Management_System.DTOs.Expense
{
    public class UpdateExpenseDto
    {
        [Required]
        public int CategoryId { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime ExpenseDate { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        // Optional while editing
        public IFormFile? Receipt { get; set; }
    }
}