using Expense_Management_System.DTOs.Auth;

namespace Expense_Management_System.Services.Auth
{
    public interface IAuthService
    {
        string Login(LoginDto loginDto);
    }
}