using Expense_Management_System.Data;
using Expense_Management_System.DTOs.Auth;
using Microsoft.EntityFrameworkCore;

namespace Expense_Management_System.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public string Login(LoginDto loginDto)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Email == loginDto.Email);

            if (user == null)
            {
                return "Invalid Email";
            }

            return "Login Successful";
        }
    }
}