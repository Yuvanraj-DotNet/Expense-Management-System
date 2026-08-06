namespace Expense_Management_System_MVC.DTO.Auth
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty; 
        public int UserId { get; set; } 
        public string Name { get; set; } = string.Empty; 
        public string Email { get; set; } = string.Empty; 
        public int RoleId { get; set; } 
        public int DepartmentId { get; set; }
    }
}
