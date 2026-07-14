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

            if (createUserDto.RoleId < 1 || createUserDto.RoleId > 4)
            {
                return "Invalid Role";
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

                // Must be Manager Role
                if (manager.RoleId != 2)
                {
                    return "Selected user is not a Manager";
                }

                // Manager must be Head of selected Department
                if (department.HeadUserId != manager.Id)
                {
                    return "Selected Manager is not assigned as Head of this Department";
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

            // Only One Admin Allowed
            if (createUserDto.RoleId == 4)
            {
                var adminExists = _context.Users
                    .Any(u => u.RoleId == 4);

                if (adminExists)
                {
                    return "Only one Admin is allowed";
                }
            }

            // Only One Finance Allowed
            if (createUserDto.RoleId == 3)
            {
                var financeExists = _context.Users
                    .Any(u => u.RoleId == 3);

                if (financeExists)
                {
                    return "Only one Finance user is allowed";
                }
            }

            _context.Users.Add(user);
            _context.SaveChanges();

            return "User Created Successfully";
        }

        public List<UserResponseDto> GetAllUsers

           (
             string? search,
             int pageNumber,
             int pageSize,
             out int totalRecords
           )

        {
            var query = _context.Users.AsQueryable();

            // Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();

                query = query.Where(u =>
                    u.Name.ToLower().Contains(search) ||
                    u.Email.ToLower().Contains(search) ||
                    u.Id.ToString().Contains(search) ||
                    u.RoleId.ToString().Contains(search) ||
                    u.DepartmentId.ToString().Contains(search) ||
                    (u.ManagerId != null && u.ManagerId.ToString().Contains(search))
                );
            }

            // Total Records
            totalRecords = query.Count();

            // Pagination
            var users = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
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

        public string UpdateUser(int id, UpdateUserDto updateUserDto)
        {
            // User Not Found
            var user = _context.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return "User Not Found";
            }

            // Email Already Exists (Except Current User)
            var existingUser = _context.Users
                .FirstOrDefault(u => u.Email == updateUserDto.Email && u.Id != id);

            if (existingUser != null)
            {
                return "Email already exists";
            }

            // Department Not Found
            var department = _context.Departments
                .FirstOrDefault(d => d.Id == updateUserDto.DepartmentId);

            if (department == null)
            {
                return "Department Not Found";
            }

            if (updateUserDto.RoleId < 1 || updateUserDto.RoleId > 4)
            {
                return "Invalid Role";
            }

            // Employee Validation
            if (updateUserDto.RoleId == 1)
            {
                // Employee must have Manager
                if (updateUserDto.ManagerId == null)
                {
                    return "ManagerId is required for Employees";
                }

                // Manager Exists
                var manager = _context.Users
                    .FirstOrDefault(u => u.Id == updateUserDto.ManagerId);

                if (manager == null)
                {
                    return "Manager Not Found";
                }

                // Temporary validation
                if (manager.RoleId != 2)
                {
                    return "Selected user is not a Manager";
                }

                if (department.HeadUserId != manager.Id)
                {
                    return "Selected Manager is not assigned as Head of this Department";
                }

            }

            // Non Employees should not have Manager
            if (updateUserDto.RoleId != 1 && updateUserDto.ManagerId != null)
            {
                return "ManagerId should only be assigned to Employees";
            }


            // Only One Admin Allowed
            if (updateUserDto.RoleId == 4)
            {
                var adminExists = _context.Users
                    .Any(u => u.RoleId == 4 && u.Id != id);

                if (adminExists)
                {
                    return "Only one Admin is allowed";
                }
            }

            // Only One Finance Allowed
            if (updateUserDto.RoleId == 3)
            {
                var financeExists = _context.Users
                    .Any(u => u.RoleId == 3 && u.Id != id);

                if (financeExists)
                {
                    return "Only one Finance user is allowed";
                }
            }


            // Update User
            user.Name = updateUserDto.Name;
            user.Email = updateUserDto.Email;
            user.RoleId = updateUserDto.RoleId;
            user.DepartmentId = updateUserDto.DepartmentId;
            user.ManagerId = updateUserDto.ManagerId;

            _context.SaveChanges();

            return "User Updated Successfully";
        }

        public string DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return "User Not Found";
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            return "User Deleted Successfully";
        }


    }
}