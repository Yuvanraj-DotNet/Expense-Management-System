namespace Expense_Management_System.Models
{
    public class Department
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public int? HeadUserId { get; set; }
    }
}