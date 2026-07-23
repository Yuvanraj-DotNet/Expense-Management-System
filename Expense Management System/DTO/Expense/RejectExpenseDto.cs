using System.ComponentModel.DataAnnotations;

namespace Expense_Management_System.DTOs.Expense
{
    public class RejectExpenseDto
    {
        [Required]
        public int ManagerId { get; set; }

        [Required(ErrorMessage = "Reject Reason is Required")]
        public string Comment { get; set; } = string.Empty;
    }
}