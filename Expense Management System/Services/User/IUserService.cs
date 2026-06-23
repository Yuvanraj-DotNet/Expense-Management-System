using Expense_Management_System.DTOs.User;

namespace Expense_Management_System.Services.User
{
    public interface IUserService
    {
        string CreateUser(CreateUserDto createUserDto);
    }
}
