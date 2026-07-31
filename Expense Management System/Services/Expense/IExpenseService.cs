using Expense_Management_System.DTOs.Expense;
using Expense_Management_System.DTOs.Reports;

namespace Expense_Management_System.Services.Expense
{
    public interface IExpenseService
    {
        string CreateExpense(CreateExpenseDto createExpenseDto, int userId);

        string UpdateExpense(int id, int userId, UpdateExpenseDto updateExpenseDto);

        // Employee only
        List<ExpenseResponseDto> GetMyExpenses(int userId);

        // NEW (Manager / Finance / Admin)
        List<ExpenseListResponseDto> GetAllExpenses
        (
            string? search,
            int pageNumber,
            int pageSize,
            out int totalRecords
        );

        string SubmitExpense(int id, int userId);

        List<ExpenseResponseDto> GetPendingApprovals(int managerId);

        string ApproveExpense(int id, int managerId, ApproveExpenseDto approveExpenseDto);

        string RejectExpense(int id, int managerId, RejectExpenseDto rejectExpenseDto);

        List<ApprovedExpenseDto> GetApprovedExpenses();

        string ReimburseExpense(
                  int id,
                  ReimburseExpenseDto reimburseExpenseDto,
                  int financeUserId);

        List<ReimbursementResponseDto> GetAllReimbursements

         (
               string? search,
               int pageNumber,
               int pageSize,
               out int totalRecords
         );

        List<MonthlyReportDto> GetMonthlyReport(int month, int year);

        byte[] ExportMonthlyReport(int month, int year);
    }
}