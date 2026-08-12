using System.ComponentModel.DataAnnotations;

namespace Expense_Management_System_MVC.DTO.User
{
    public class CreateUserRequest
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must contain at least 8 characters, one uppercase letter, one lowercase letter, one number, and one special character."
        )]
        public string Password { get; set; } = string.Empty;

        [Range(1, 4, ErrorMessage = "Invalid Role")]
        public int RoleId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Invalid Department")]
        public int DepartmentId { get; set; }

        public int? ManagerId { get; set; }
    }
}