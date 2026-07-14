using Expense_Management_System.Data;
using Expense_Management_System.DTOs.Department;
using Expense_Management_System.Services.Department;
using Microsoft.AspNetCore.Mvc;

namespace Expense_Management_System.Services.Department
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ApplicationDbContext _context;

        public DepartmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public string CreateDepartment(CreateDepartmentDto createDepartmentDto)
        {
            // Department Name Validation
            var departmentExists = _context.Departments
                .Any(d => d.Name == createDepartmentDto.Name);

            if (departmentExists)
            {
                return "Department Already Exists";
            }

            // Department Code Validation
            var codeExists = _context.Departments
                .Any(d => d.Code == createDepartmentDto.Code);

            if (codeExists)
            {
                return "Department Code Already Exists";
            }

            // HeadUser Validation (Only if HeadUserId is provided)
            if (createDepartmentDto.HeadUserId != null)
            {
                var user = _context.Users
                    .FirstOrDefault(u => u.Id == createDepartmentDto.HeadUserId);

                if (user == null)
                {
                    return "Selected Department Head Not Found";
                }

                if (user.RoleId != 2)
                {
                    return "Only Managers can be assigned as Department Head";
                }

                var alreadyAssigned = _context.Departments
                    .FirstOrDefault(d => d.HeadUserId == createDepartmentDto.HeadUserId);

                if (alreadyAssigned != null)
                {
                    return "This Manager is already assigned as another Department Head";
                }
            }

            var department = new Expense_Management_System.Models.Department
            {
                Name = createDepartmentDto.Name,
                Code = createDepartmentDto.Code,
                HeadUserId = createDepartmentDto.HeadUserId
            };

            _context.Departments.Add(department);
            _context.SaveChanges();

            return "Department Created Successfully";
        }

        public List<DepartmentResponseDto> GetAllDepartments

             (
               string? search,
               int pageNumber,
               int pageSize,
               out int totalRecords
             )

        {
            var query = _context.Departments.AsQueryable();

            // Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();

                query = query.Where(d =>
                    d.Id.ToString().Contains(search) ||
                    d.Name.ToLower().Contains(search) ||
                    d.Code.ToLower().Contains(search) ||
                    (d.HeadUserId != null && d.HeadUserId.ToString().Contains(search))
                );
            }

            totalRecords = query.Count();

            var departments = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(d => new DepartmentResponseDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Code = d.Code,
                    HeadUserId = d.HeadUserId
                })
                .ToList();

            return departments;
        }

        public Expense_Management_System.Models.Department? GetDepartmentById(int id)
        {
            return _context.Departments
                .FirstOrDefault(d => d.Id == id);
        }

        public string UpdateDepartment(int id, UpdateDepartmentDto updateDepartmentDto)
        {
            var department = _context.Departments
                .FirstOrDefault(d => d.Id == id);

            if (department == null)
            {
                return "Department Not Found";
            }

            if (updateDepartmentDto.HeadUserId != null)
            {
                var user = _context.Users
                    .FirstOrDefault(u => u.Id == updateDepartmentDto.HeadUserId);

                if (user == null)
                {
                    return "Selected Department Head Not Found";
                }

                if (user.RoleId != 2)
                {
                    return "Only Managers can be assigned as Department Head";
                }

                var alreadyAssigned = _context.Departments
                 .FirstOrDefault(d =>
                 d.HeadUserId == updateDepartmentDto.HeadUserId &&
                 d.Id != id);

                if (alreadyAssigned != null)
                {
                    return "This Manager is already assigned as another Department Head";
                }
            }

            department.Name = updateDepartmentDto.Name;
            department.Code = updateDepartmentDto.Code;
            department.HeadUserId = updateDepartmentDto.HeadUserId;

            _context.SaveChanges();

            return "Department Updated Successfully";
        }

        public string DeleteDepartment(int id)
        {
            var department = _context.Departments
                .FirstOrDefault(d => d.Id == id);

            if (department == null)
            {
                return "Department Not Found";
            }

            _context.Departments.Remove(department);

            _context.SaveChanges();

            return "Department Deleted Successfully";
        }
    }
}