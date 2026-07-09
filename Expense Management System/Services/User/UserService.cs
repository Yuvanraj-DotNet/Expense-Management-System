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
            var existingUser = _context.Users
                .FirstOrDefault(u => u.Email == createUserDto.Email);

            if (existingUser != null)
            {
                return "Email already exists";
            }


            var department = _context.Departments
            .FirstOrDefault(d => d.Id == createUserDto.DepartmentId);

            if (department == null)
            {
                return "Department Not Found";
            }


            // Employee Validation
            if (createUserDto.RoleId == 1)
            {
                // Employee must have Manager
                if (createUserDto.ManagerId == null)
                {
                    return "ManagerId is required for Employees";
                }

                // Manager Exists
                var manager = _context.Users
                    .FirstOrDefault(u => u.Id == createUserDto.ManagerId);

                if (manager == null)
                {
                    return "Manager Not Found";
                }

                // TEMPORARY
                // This will be replaced later with Department.HeadUserId validation
                if (manager.RoleId != 2)
                {
                    return "Selected user is not a Manager";
                }
            }

            // Non Employees should not have Manager
            if (createUserDto.RoleId != 1 && createUserDto.ManagerId != null)
            {
                return "ManagerId should only be assigned to Employees";
            }


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

        public List<UserResponseDto> GetAllUsers()
        {
            var users = _context.Users
                .Select(u => new UserResponseDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    RoleId = u.RoleId,
                    DepartmentId = u.DepartmentId,
                    ManagerId = u.ManagerId
                })
                .ToList();

            return users;
        }

        public UserResponseDto? GetUserById(int id)
        {
            var user = _context.Users
                .Where(u => u.Id == id)
                .Select(u => new UserResponseDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    RoleId = u.RoleId,
                    DepartmentId = u.DepartmentId,
                    ManagerId = u.ManagerId
                })
                .FirstOrDefault();

            return user;
        }
    }
}