using Expense_Management_System.Data;
using Expense_Management_System.DTOs.Expense;

namespace Expense_Management_System.Services.Expense
{
    public class ExpenseService : IExpenseService
    {
        private readonly ApplicationDbContext _context;

        public ExpenseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public string CreateExpense(CreateExpenseDto createExpenseDto)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Id == createExpenseDto.UserId);

            if (user == null)
            {
                return "User Not Found";
            }

            var category = _context.ExpenseCategories
                .FirstOrDefault(c => c.Id == createExpenseDto.CategoryId);

            if (category == null)
            {
                return "Expense Category Not Found";
            }

            var expense = new Models.Expense
            {
                UserId = createExpenseDto.UserId,
                CategoryId = createExpenseDto.CategoryId,
                Title = createExpenseDto.Title,
                Amount = createExpenseDto.Amount,
                ExpenseDate = createExpenseDto.ExpenseDate,
                Description = createExpenseDto.Description
            };

            _context.Expenses.Add(expense);
            _context.SaveChanges();

            return "Expense Created Successfully";
        }

        public string UpdateExpense(int id, UpdateExpenseDto updateExpenseDto)
        {
            var expense = _context.Expenses
                .FirstOrDefault(e => e.Id == id);

            if (expense == null)
            {
                return "Expense Not Found";
            }

            expense.CategoryId = updateExpenseDto.CategoryId;
            expense.Title = updateExpenseDto.Title;
            expense.Amount = updateExpenseDto.Amount;
            expense.ExpenseDate = updateExpenseDto.ExpenseDate;
            expense.Description = updateExpenseDto.Description;

            _context.SaveChanges();

            return "Expense Updated Successfully";
        }

        public List<ExpenseResponseDto> GetMyExpenses(int userId)
        {
            var expenses = _context.Expenses
                .Where(e => e.UserId == userId)
                .Select(e => new ExpenseResponseDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Amount = e.Amount,
                    Status = e.Status,
                    ExpenseDate = e.ExpenseDate
                })
                .ToList();

            return expenses;
        }

        public string SubmitExpense(int id)
        {
            var expense = _context.Expenses
                .FirstOrDefault(e => e.Id == id);

            if (expense == null)
            {
                return "Expense Not Found";
            }

            if (expense.Status != "Draft")
            {
                return "Only Draft Expenses Can Be Submitted";
            }

            expense.Status = "Submitted";

            _context.SaveChanges();

            return "Expense Submitted Successfully";
        }

        public List<ExpenseResponseDto> GetPendingApprovals()
        {
            var expenses = _context.Expenses
                .Where(e => e.Status == "Submitted")
                .Select(e => new ExpenseResponseDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Amount = e.Amount,
                    Status = e.Status,
                    ExpenseDate = e.ExpenseDate
                })
                .ToList();

            return expenses;
        }

        public string ApproveExpense(int id, ApproveExpenseDto approveExpenseDto)
        {
            var expense = _context.Expenses
                .FirstOrDefault(e => e.Id == id);

            if (expense == null)
            {
                return "Expense Not Found";
            }

            if (expense.Status != "Submitted")
            {
                return "Only Submitted Expenses Can Be Approved";
            }

            // Update Expense Status
            expense.Status = "Approved";

            // Save Approval History
            var approval = new Models.ExpenseApproval
            {
                ExpenseId = expense.Id,
                ApproverId = approveExpenseDto.ManagerId,
                Action = "Approved",
                Comment = approveExpenseDto.Comment,
                ActionDate = DateTime.Now
            };

            _context.ExpenseApprovals.Add(approval);

            _context.SaveChanges();

            return "Expense Approved Successfully";
        }

        public string RejectExpense(int id, RejectExpenseDto rejectExpenseDto)
        {
            var expense = _context.Expenses
                .FirstOrDefault(e => e.Id == id);

            if (expense == null)
            {
                return "Expense Not Found";
            }

            if (expense.Status != "Submitted")
            {
                return "Only Submitted Expenses Can Be Rejected";
            }

            expense.Status = "Rejected";

            var expenseApproval = new Models.ExpenseApproval
            {
                ExpenseId = expense.Id,
                ApproverId = rejectExpenseDto.ManagerId,
                Action = "Rejected",
                Comment = rejectExpenseDto.Comment,
                ActionDate = DateTime.Now
            };

            _context.ExpenseApprovals.Add(expenseApproval);

            _context.SaveChanges();

            return "Expense Rejected Successfully";
        }

        public List<ApprovedExpenseDto> GetApprovedExpenses()
        {
            var approvedExpenses = (from expense in _context.Expenses
                                    join user in _context.Users
                                        on expense.UserId equals user.Id
                                    join category in _context.ExpenseCategories
                                        on expense.CategoryId equals category.Id
                                    join approval in _context.ExpenseApprovals
                                        on expense.Id equals approval.ExpenseId
                                    where expense.Status == "Approved"
                                    select new ApprovedExpenseDto
                                    {
                                        ExpenseId = expense.Id,
                                        ApproverId = approval.ApproverId,
                                        EmployeeName = user.Name,
                                        CategoryName = category.Name,
                                        Amount = expense.Amount,
                                        ExpenseDate = expense.ExpenseDate,
                                        Status = expense.Status
                                    }).ToList();

            return approvedExpenses;
        }

        public string ReimburseExpense(int id, ReimburseExpenseDto reimburseExpenseDto)
        {
            var expense = _context.Expenses
                .FirstOrDefault(e => e.Id == id);

            if (expense == null)
            {
                return "Expense Not Found";
            }

            if (expense.Status != "Approved")
            {
                return "Only Approved Expenses Can Be Reimbursed";
            }

            var reimbursement = new Models.Reimbursement
            {
                ExpenseId = expense.Id,
                ProcessedBy = reimburseExpenseDto.ProcessedBy,
                PaymentDate = DateTime.Now,
                ReferenceNumber = reimburseExpenseDto.ReferenceNumber,
                Amount = expense.Amount
            };

            _context.Reimbursements.Add(reimbursement);

            expense.Status = "Reimbursed";

            _context.SaveChanges();

            return "Expense Reimbursed Successfully";
        }



    }
}
