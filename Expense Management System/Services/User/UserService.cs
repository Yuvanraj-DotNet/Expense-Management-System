using Expense_Management_System.Data;
using Expense_Management_System.DTOs.User;
using Expense_Management_System.Models;

namespace Expense_Management_System.Services.User
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public string CreateUser(CreateUserDto createUserDto)
        {
            var user = new Models.User
            {
                Name = createUserDto.Name,
                Email = createUserDto.Email,
                PasswordHash = createUserDto.Password,
                RoleId = createUserDto.RoleId,
                DepartmentId = createUserDto.DepartmentId,
                ManagerId = createUserDto.ManagerId
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return "User Created Successfully";
        }
    }
}