using Expense_Management_System.DTOs.Auth;

namespace Expense_Management_System.Services.Auth
{
    public class AuthService : IAuthService
    {
        public string Login(LoginDto loginDto)
        {
            return "Login Successful";
        }
    }
}
