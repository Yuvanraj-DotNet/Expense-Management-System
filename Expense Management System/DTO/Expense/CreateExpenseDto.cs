using System.ComponentModel.DataAnnotations;

namespace Expense_Management_System.DTOs.Expense
{
    public class CreateExpenseDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Title is Required")]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime ExpenseDate { get; set; }

        [Required(ErrorMessage = "Description is Required")]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
    }
}