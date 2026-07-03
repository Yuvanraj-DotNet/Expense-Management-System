using Expense_Management_System.Data;
using Expense_Management_System.DTOs.Department;
using Expense_Management_System.Services.Department;

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
            var departmentExists = _context.Departments
                .Any(d => d.Name == createDepartmentDto.Name);

            if (departmentExists)
            {
                return "Department Already Exists";
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

        public List<Expense_Management_System.Models.Department> GetAllDepartments()
        {
            return _context.Departments.ToList();
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