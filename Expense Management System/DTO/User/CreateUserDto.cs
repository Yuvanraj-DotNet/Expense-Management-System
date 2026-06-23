namespace Expense_Management_System.DTOs.User
{
    public class CreateUserDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public int DepartmentId { get; set; }
        public int? ManagerId { get; set; }
    }
}