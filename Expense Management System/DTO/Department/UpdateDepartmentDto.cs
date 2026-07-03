using System.ComponentModel.DataAnnotations;

namespace Expense_Management_System.DTOs.Department
{
    public class UpdateDepartmentDto
    {
        [Required(ErrorMessage = "Department Name is Required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Department Code is Required")]
        public string Code { get; set; } = string.Empty;

        public int? HeadUserId { get; set; }
    }
}