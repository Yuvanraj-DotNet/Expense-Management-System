using Expense_Management_System.Data;
using Expense_Management_System.DTOs.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Expense_Management_System.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<Expense_Management_System.Models.User> _passwordHasher;


        public AuthService(ApplicationDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<Expense_Management_System.Models.User>();
        }

        public string Login(LoginDto loginDto)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Email == loginDto.Email);

            if (user == null)
            {
                return "Invalid Email";
            }

            var passwordResult = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                loginDto.Password
            );

            if (passwordResult == PasswordVerificationResult.Failed)
            {
                return "Invalid Password";
            }

            return "Login Successful";
        }
    }
}