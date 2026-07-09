using Expense_Management_System.DTOs.User;

namespace Expense_Management_System.Services.User
{
    public interface IUserService
    {
        string CreateUser(CreateUserDto createUserDto);
        List<UserResponseDto> GetAllUsers
        (
         string? search,
         int pageNumber,
         int pageSize,
         out int totalRecords
        );

        UserResponseDto? GetUserById(int id);

        string UpdateUser(int id, UpdateUserDto updateUserDto);

    }
}
