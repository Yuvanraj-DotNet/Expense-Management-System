namespace Expense_Management_System_MVC.DTO.User
{
    public class UserResponse
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public int RoleId { get; set; }

        public int DepartmentId { get; set; }

        public int? ManagerId { get; set; }
    }
}