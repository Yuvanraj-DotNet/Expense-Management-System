using Expense_Management_System_MVC.DTO.Auth;

namespace Expense_Management_System_MVC.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
    }
}