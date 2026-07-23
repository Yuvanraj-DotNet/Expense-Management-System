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
            // User Exists
            var user = _context.Users
                .FirstOrDefault(u => u.Id == createExpenseDto.UserId);

            if (user == null)
            {
                return "User Not Found";
            }

            // Only Employees can create Expense
            if (user.RoleId != 1)
            {
                return "Only Employees can create expenses";
            }

            // Category Exists
            var category = _context.ExpenseCategories
                .FirstOrDefault(c => c.Id == createExpenseDto.CategoryId);

            if (category == null)
            {
                return "Expense Category Not Found";
            }

            // Amount > 0
            if (createExpenseDto.Amount <= 0)
            {
                return "Amount must be greater than 0";
            }

            // Category Limit
            if (createExpenseDto.Amount > category.MaxAllowedAmount)
            {
                return $"Maximum allowed amount for {category.Name} is {category.MaxAllowedAmount}";
            }

            // Future Date
            if (createExpenseDto.ExpenseDate.Date > DateTime.Today)
            {
                return "Expense Date cannot be in the future";
            }

            var expense = new Models.Expense
            {
                UserId = createExpenseDto.UserId,
                CategoryId = createExpenseDto.CategoryId,
                Title = createExpenseDto.Title.Trim(),
                Amount = createExpenseDto.Amount,
                ExpenseDate = createExpenseDto.ExpenseDate,
                Description = createExpenseDto.Description.Trim()
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

            // Cannot Edit Submitted Expense
            if (expense.Status != "Draft")
            {
                return "Only Draft Expenses can be updated";
            }

            var category = _context.ExpenseCategories
                .FirstOrDefault(c => c.Id == updateExpenseDto.CategoryId);

            if (category == null)
            {
                return "Expense Category Not Found";
            }

            if (updateExpenseDto.Amount <= 0)
            {
                return "Amount must be greater than 0";
            }

            if (updateExpenseDto.Amount > category.MaxAllowedAmount)
            {
                return $"Maximum allowed amount for {category.Name} is {category.MaxAllowedAmount}";
            }

            if (updateExpenseDto.ExpenseDate.Date > DateTime.Today)
            {
                return "Expense Date cannot be in the future";
            }

            expense.CategoryId = updateExpenseDto.CategoryId;
            expense.Title = updateExpenseDto.Title.Trim();
            expense.Amount = updateExpenseDto.Amount;
            expense.ExpenseDate = updateExpenseDto.ExpenseDate;
            expense.Description = updateExpenseDto.Description.Trim();

            _context.SaveChanges();

            return "Expense Updated Successfully";
        }

        public List<ExpenseResponseDto> GetMyExpenses(int userId)
        {
            var expenses = (from expense in _context.Expenses

                            join approval in _context.ExpenseApprovals
                            on expense.Id equals approval.ExpenseId into approvalGroup

                            from approval in approvalGroup
                                .OrderByDescending(a => a.ActionDate)
                                .Take(1)
                                .DefaultIfEmpty()

                            where expense.UserId == userId

                            select new ExpenseResponseDto
                            {
                                Id = expense.Id,
                                Title = expense.Title,
                                Amount = expense.Amount,
                                Status = expense.Status,
                                ExpenseDate = expense.ExpenseDate,

                                ManagerComment = approval != null
                                    ? approval.Comment
                                    : null,

                                ActionDate = approval != null
                                    ? approval.ActionDate
                                    : null
                            }).ToList();

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

        public List<ExpenseResponseDto> GetPendingApprovals(int managerId)
        {
            var manager = _context.Users
                .FirstOrDefault(u => u.Id == managerId);

            // Manager must exist
            if (manager == null)
            {
                return new List<ExpenseResponseDto>();
            }

            // Only Managers can view pending approvals
            if (manager.RoleId != 2)
            {
                return new List<ExpenseResponseDto>();
            }

            var expenses = (from expense in _context.Expenses
                            join user in _context.Users
                                on expense.UserId equals user.Id
                            where expense.Status == "Submitted"
                                  && user.DepartmentId == manager.DepartmentId
                            select new ExpenseResponseDto
                            {
                                Id = expense.Id,
                                Title = expense.Title,
                                Amount = expense.Amount,
                                Status = expense.Status,
                                ExpenseDate = expense.ExpenseDate
                            }).ToList();

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

            if (string.IsNullOrWhiteSpace(rejectExpenseDto.Comment))
            {
                return "Reject Reason is Required";
            }

            expense.Status = "Rejected";

            var expenseApproval = new Models.ExpenseApproval
            {
                ExpenseId = expense.Id,
                ApproverId = rejectExpenseDto.ManagerId,
                Action = "Rejected",
                Comment = rejectExpenseDto.Comment.Trim(),
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
