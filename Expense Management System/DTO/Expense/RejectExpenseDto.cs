namespace Expense_Management_System.DTOs.Expense
{
    public class RejectExpenseDto
    {
        public int ManagerId { get; set; }

        public string Comment { get; set; } = string.Empty;
    }
}