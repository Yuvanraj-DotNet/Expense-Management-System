using Expense_Management_System.DTOs.Department;

namespace Expense_Management_System.Services.Department
{
    public interface IDepartmentService
    {
        string CreateDepartment(CreateDepartmentDto createDepartmentDto);
        List<DepartmentResponseDto> GetAllDepartments

         (
            string? search,
            int pageNumber,
            int pageSize,
            out int totalRecords
         );

        Expense_Management_System.Models.Department? GetDepartmentById(int id);
        string UpdateDepartment(int id, UpdateDepartmentDto updateDepartmentDto);
        string DeleteDepartment(int id);
        
    }
}