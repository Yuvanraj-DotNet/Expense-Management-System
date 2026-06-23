using System.ComponentModel.DataAnnotations;

namespace Expense_Management_System.DTOs.User
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = string.Empty;

        public int RoleId { get; set; }

        public int DepartmentId { get; set; }

        public int? ManagerId { get; set; }
    }
}