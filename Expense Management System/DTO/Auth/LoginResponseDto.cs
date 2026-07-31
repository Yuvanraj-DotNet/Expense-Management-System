namespace Expense_Management_System.DTOs.Auth
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public int RoleId { get; set; }

        public int UserId { get; set; }
        public int DepartmentId { get; set; }
    }
}