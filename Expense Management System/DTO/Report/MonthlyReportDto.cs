namespace Expense_Management_System.DTOs.Reports
{
    public class MonthlyReportDto
    {
        public string Month { get; set; } = string.Empty;

        public string DepartmentName { get; set; } = string.Empty;

        public int TotalExpenses { get; set; }

        public decimal TotalAmount { get; set; }

        public int ApprovedExpenses { get; set; }

        public int RejectedExpenses { get; set; }

        public int ReimbursedExpenses { get; set; }
    }
}